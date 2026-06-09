using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using EI.SI;

namespace servidor
{
    internal class Program
    {
        // Lista estática para armazenar todos os clientes ativos
        static List<TcpClient> clientesConectados = new List<TcpClient>();
        static readonly object listaLock = new object(); // Objeto para evitar problemas de concorrência na lista

        static void Main(string[] args)
        {
            Console.WriteLine($"Servidor Foi Iniciado");
            Logger.Gravar("Servidor Foi Iniciado"); //Log para inicialização do servidor

            TcpListener listener = new TcpListener(IPAddress.Any, 4000);
            listener.Start();

            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();

                // Pega o IP real do cliente que acabou de se conectar
                string ipCliente = ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString();
                Console.WriteLine($"Cliente [{ipCliente}] conectado");
                Logger.Gravar($"Cliente [{ipCliente}] conectado"); //Log cliente connectado

                // Adiciona o cliente na lista de forma segura
                lock (listaLock)
                {
                    clientesConectados.Add(client);
                }

                Thread clientThread = new Thread(() => lerMensagens(client, ipCliente));
                clientThread.Start();
            }
        }

        static void lerMensagens(TcpClient client, string ipUser)
        {
            NetworkStream networkStream = client.GetStream();
            ProtocolSI protocolSI = new ProtocolSI();

            try
            {
                while (true)
                {
                    // Se o Read retornar 0 ou menor, significa que o cliente desconectou
                    int pacoteLido = networkStream.Read(protocolSI.Buffer, 0, protocolSI.Buffer.Length);
                    if (pacoteLido <= 0) break;

                    switch (protocolSI.GetCmdType())
                    {
                        case ProtocolSICmdType.DATA:
                            string msg = protocolSI.GetStringFromData();
                            Console.WriteLine($"Cliente {ipUser}: {msg}");
                            Logger.Gravar($"Cliente {ipUser}: {msg}"); //Log mensagem recebida do cliente

                            // Prepara o pacote para retransmitir para TODOS os clientes
                            byte[] pacoteRetransmissao = protocolSI.Make(ProtocolSICmdType.DATA, msg);

                            // Envia para todo mundo que está na lista
                            lock (listaLock)
                            {
                                foreach (TcpClient c in clientesConectados)
                                {
                                    if (c.Connected)
                                    {
                                        try
                                        {
                                            c.GetStream().Write(pacoteRetransmissao, 0, pacoteRetransmissao.Length);
                                        }
                                        catch { /* Falha ao enviar para um cliente específico */ }
                                    }
                                }
                            }
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erro com o cliente [{ipUser}]: " + e.Message);
                Logger.Gravar($"Erro com o cliente [{ipUser}]: " + e.Message); //Log erro com cliente
            }
            finally
            {
                // Remove o cliente da lista ao desconectar e fecha a conexão
                lock (listaLock)
                {
                    clientesConectados.Remove(client);
                }
                client.Close();
                Console.WriteLine($"Cliente [{ipUser}] desconectado.");
                Logger.Gravar($"Cliente [{ipUser}] desconectado."); //Log cliente desconectado
            }
        }
    }
}