using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using EI.SI;
using System.Security.Cryptography;
using System.IO;
using System.Text;

namespace servidor
{
    internal class Program
    {        
        static List<TcpClient> clientesConectados = new List<TcpClient>();
        static readonly object listaLock = new object(); // Objeto para evitar problemas de concorrência na lista

        static Dictionary<TcpClient, string> chavesPublicas = new Dictionary<TcpClient, string>();

        static AesCryptoServiceProvider aes;

        static void Main(string[] args)
        {
            aes = new AesCryptoServiceProvider();

            Console.WriteLine($"Servidor Foi Iniciado");
            Logger.Gravar("Servidor Foi Iniciado"); 

            TcpListener listener = new TcpListener(IPAddress.Any, 4000);
            listener.Start();

            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();
            
                string ipCliente = ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString();
                Console.WriteLine($"Cliente [{ipCliente}] conectado");
                Logger.Gravar($"Cliente [{ipCliente}] conectado"); 
                
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
                    int pacoteLido = networkStream.Read(protocolSI.Buffer, 0, protocolSI.Buffer.Length);
                    if (pacoteLido <= 0) break;

                    switch (protocolSI.GetCmdType())
                    {                        
                        case ProtocolSICmdType.PUBLIC_KEY:
                            string chavePublicaCliente = protocolSI.GetStringFromData();

                            lock (listaLock) { chavesPublicas[client] = chavePublicaCliente; }

                            Console.WriteLine($"Chave Pública recebida do cliente [{ipUser}]");
                            Logger.Gravar($"Chave Pública recebida do cliente [{ipUser}]");
                            
                            using (RSACryptoServiceProvider rsaCliente = new RSACryptoServiceProvider())
                            {
                                rsaCliente.FromXmlString(chavePublicaCliente);
                                
                                byte[] keyCifrada = rsaCliente.Encrypt(aes.Key, true);
                                byte[] ivCifrado = rsaCliente.Encrypt(aes.IV, true);
                                
                                byte[] pacoteKey = protocolSI.Make(ProtocolSICmdType.SECRET_KEY, keyCifrada);
                                networkStream.Write(pacoteKey, 0, pacoteKey.Length);

                                // timer de 100milisecs
                                Thread.Sleep(100);
                                
                                byte[] pacoteIV = protocolSI.Make(ProtocolSICmdType.IV, ivCifrado);
                                networkStream.Write(pacoteIV, 0, pacoteIV.Length);

                                Console.WriteLine($"Chave AES cifrada e enviada para [{ipUser}]");
                            }
                            break;

                        case ProtocolSICmdType.DATA:
                            string pacoteRecebido = protocolSI.GetStringFromData();
                            string[] partes = pacoteRecebido.Split(new string[] { "|#SIGN#|" }, StringSplitOptions.None);

                            if (partes.Length == 2)
                            {
                                string msgCifrada = partes[0];
                                string assinaturaBase64 = partes[1];
                                
                                string msgDecifrada = "";
                                using (ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                                using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(msgCifrada)))
                                using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                                using (StreamReader sr = new StreamReader(cs))
                                {
                                    msgDecifrada = sr.ReadToEnd();
                                }
                                
                                bool assinaturaValida = false;
                                using (RSACryptoServiceProvider rsaVerify = new RSACryptoServiceProvider())
                                {                                    
                                    rsaVerify.FromXmlString(chavesPublicas[client]);

                                    byte[] hashOriginal = Encoding.UTF8.GetBytes(msgDecifrada);
                                    byte[] assinaturaBytes = Convert.FromBase64String(assinaturaBase64);
                                    
                                    assinaturaValida = rsaVerify.VerifyData(hashOriginal, CryptoConfig.MapNameToOID("SHA256"), assinaturaBytes);
                                }

                                if (assinaturaValida)
                                {
                                    Console.WriteLine($"[ASSINATURA VÁLIDA] Cliente {ipUser}: {msgDecifrada}");
                                    Logger.Gravar($"[ASSINATURA VÁLIDA] Cliente {ipUser}: {msgDecifrada}");

                                    // Retransmitir o pacote inteiro (cifra + assinatura) aos outros clientes
                                    byte[] pacoteRetransmissao = protocolSI.Make(ProtocolSICmdType.DATA, pacoteRecebido);
                                    lock (listaLock)
                                    {
                                        foreach (TcpClient c in clientesConectados)
                                        {
                                            if (c.Connected)
                                            {
                                                try { c.GetStream().Write(pacoteRetransmissao, 0, pacoteRetransmissao.Length); }
                                                catch { }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    Console.WriteLine($"[ALERTA!] Assinatura inválida do cliente {ipUser}. Mensagem bloqueada!");
                                }
                            }
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erro com o cliente [{ipUser}]: " + e.Message);
                Logger.Gravar($"Erro com o cliente [{ipUser}]: " + e.Message);
            }
            finally
            {                
                lock (listaLock)
                {
                    clientesConectados.Remove(client);
                }
                client.Close();
                Console.WriteLine($"Cliente [{ipUser}] desconectado.");
                Logger.Gravar($"Cliente [{ipUser}] desconectado."); 
            }
        }
    }
}