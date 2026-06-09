using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using EI.SI;
using System.Security.Cryptography;
using System.IO;

namespace servidor
{
    internal class Program
    {
        // Lista estática para armazenar todos os clientes ativos
        static List<TcpClient> clientesConectados = new List<TcpClient>();
        static readonly object listaLock = new object(); // Objeto para evitar problemas de concorrência na lista

        static AesCryptoServiceProvider aes;

        static void Main(string[] args)
        {
            aes = new AesCryptoServiceProvider();

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
                        // 4. para lidar com a Chave Pública
                        case ProtocolSICmdType.PUBLIC_KEY:
                            string chavePublicaCliente = protocolSI.GetStringFromData();
                            Console.WriteLine($"Chave Pública recebida do cliente [{ipUser}]");
                            Logger.Gravar($"Chave Pública recebida do cliente [{ipUser}]");

                            // 4.1. Usar a chave pública que acabámos de receber
                            using (RSACryptoServiceProvider rsaCliente = new RSACryptoServiceProvider())
                            {
                                rsaCliente.FromXmlString(chavePublicaCliente);

                                // 4.2. Cifrar a Chave AES e o IV usando o RSA do cliente (true = OAEP padding)
                                byte[] keyCifrada = rsaCliente.Encrypt(aes.Key, true);
                                byte[] ivCifrado = rsaCliente.Encrypt(aes.IV, true);

                                // 4.3. Enviar a Chave AES cifrada para o cliente
                                byte[] pacoteKey = protocolSI.Make(ProtocolSICmdType.SECRET_KEY, keyCifrada);
                                networkStream.Write(pacoteKey, 0, pacoteKey.Length);

                                // 4.4. Pausa de 100ms para evitar que os pacotes se colem na rede
                                Thread.Sleep(100);

                                // 4.5. Enviar o IV cifrado para o cliente
                                byte[] pacoteIV = protocolSI.Make(ProtocolSICmdType.IV, ivCifrado);
                                networkStream.Write(pacoteIV, 0, pacoteIV.Length);

                                Console.WriteLine($"Chave AES cifrada e enviada para [{ipUser}]");
                            }
                            break;

                        case ProtocolSICmdType.DATA:
                            string msgCifrada = protocolSI.GetStringFromData();

                            // 1. Decifrar a mensagem para o Servidor a conseguir ler/logar em texto limpo
                            string msgDecifrada = "";
                            using (ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                            using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(msgCifrada)))
                            using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                            using (StreamReader sr = new StreamReader(cs))
                            {
                                msgDecifrada = sr.ReadToEnd();
                            }

                            Console.WriteLine($"Cliente {ipUser} disse: {msgDecifrada}");
                            Logger.Gravar($"Cliente {ipUser} disse: {msgDecifrada}");

                            // 2. Retransmitir a mensagem CIFRADA ORIGINAL para que os outros clientes a possam decifrar
                            byte[] pacoteRetransmissao = protocolSI.Make(ProtocolSICmdType.DATA, msgCifrada);

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
                                        catch { }
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