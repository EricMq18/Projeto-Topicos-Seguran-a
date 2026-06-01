using EI.SI;
using System;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace TopicosSeguranca
{
    internal class Conectar
    {
        TcpClient client;
        ProtocolSI protocolSI = new ProtocolSI();
        NetworkStream networkStream;

        // Passamos o Form1 como parâmetro para conseguir atualizar a interface gráfica
        public void Conexao(Form1 form)
        {
            try
            {
                client = new TcpClient("127.0.0.1", 4000);
                networkStream = client.GetStream();

                // Cria e inicia a Thread de escuta do Servidor
                Thread threadEscuta = new Thread(() => OuvirServidor(form));
                threadEscuta.IsBackground = true; // Para fechar a thread junto com a aplicação
                threadEscuta.Start();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                throw;
            }
        }

        private void OuvirServidor(Form1 form)
        {
            ProtocolSI protocolReader = new ProtocolSI();
            try
            {
                while (true)
                {
                    int bytesRead = networkStream.Read(protocolReader.Buffer, 0, protocolReader.Buffer.Length);
                    if (bytesRead <= 0) break;

                    if (protocolReader.GetCmdType() == ProtocolSICmdType.DATA)
                    {
                        string msg = protocolReader.GetStringFromData();

                        // Envia a mensagem recebida para a função segura do Form
                        form.AdicionarMensagemNoChat(msg);
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Conexão com o servidor perdida.");
            }
        }

        public void LerMensagem(string msg)
        {
            if (networkStream != null && client.Connected && !string.IsNullOrWhiteSpace(msg))
            {
                byte[] packet = protocolSI.Make(ProtocolSICmdType.DATA, msg);
                networkStream.Write(packet, 0, packet.Length);
            }
        }
    }
}