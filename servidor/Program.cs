using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EI.SI;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace servidor
{
    internal class Program
    {
        static int users = 0;        
        static void Main(string[] args)
        {
            Console.WriteLine($"Servidor Foi Iniciado");
            String ipUser = "";
            string hostName = Dns.GetHostName();            

            IPHostEntry hostEntry = Dns.GetHostEntry(hostName);            
            foreach (IPAddress address in hostEntry.AddressList)
            {
                if (address.AddressFamily == AddressFamily.InterNetwork)
                {                  
                    ipUser = address.ToString();                                        
                    break;
                }
            }
            
            TcpListener listener = new TcpListener(IPAddress.Any, 4000);
            listener.Start();

            while (true)
            {                
                TcpClient client = listener.AcceptTcpClient();                                

                Console.WriteLine($"Cliente [{ipUser}] conectado");
                
                Thread clientThread = new Thread(() => lerMensagens(client, ipUser));
                clientThread.Start();
            }
        }

        static void lerMensagens(TcpClient client, string ipUser)
        {
            NetworkStream networkStream = client.GetStream();
            ProtocolSI protocolSI = new ProtocolSI();
                      
            while (true)
            {
                try
                {
                    int pacoteLido = networkStream.Read(protocolSI.Buffer, 0, protocolSI.Buffer.Length);

                    if (pacoteLido > 0) {
                        Console.WriteLine($"{pacoteLido.ToString()}");
                    }

                    switch (protocolSI.GetCmdType())
                    {
                        case ProtocolSICmdType.DATA:
                            string msg = protocolSI.GetStringFromData();
                            Console.WriteLine($"Cliente{ipUser}:{msg}");
                            break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine (e.ToString());
                    throw;
                }
            }

        }
    }
}
