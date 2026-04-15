using EI.SI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TopicosSeguranca
{
    internal class Conectar
    {
        TcpClient client;
        ProtocolSI protocolSI = new ProtocolSI();
        NetworkStream networkStream;        
        public void Conexao()
        {
            try
            {
                client = new TcpClient("127.0.0.1", 4000);
                networkStream = client.GetStream();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                throw;
            }
        }

        public void LerMensagem(string msg)
        {
            if (networkStream != null && client.Connected && !string.IsNullOrWhiteSpace(msg))
            {
                string message = msg;
                byte[] packet = protocolSI.Make(ProtocolSICmdType.DATA, msg);
                networkStream.Write(packet, 0, packet.Length);                
            }
        }
    }
}
