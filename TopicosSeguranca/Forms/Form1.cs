using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using EI.SI;
using System.Net;
using System.Security.Cryptography;

namespace TopicosSeguranca
{    
    public partial class Form1 : Form
    {
        bool JaConectou = false;
        TcpClient client;
        NetworkStream networkStream;
        ProtocolSI protocolSI;
        public Form1()
        {
            InitializeComponent();
            protocolSI = new ProtocolSI();            
        }

        private void btnConectar_Click(object sender, EventArgs e)
        {           
            if(JaConectou == false)
            {
                JaConectou = true;
                ConectarServidor();             
            }            
        }

        private void ConectarServidor()
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

        private void btnEnviar_Click(object sender, EventArgs e)
        {
            if (networkStream != null && client.Connected && !string.IsNullOrWhiteSpace(txtChat.Text))
            {                
                string msg = txtChat.Text;
                byte[] packet = protocolSI.Make(ProtocolSICmdType.DATA, msg);                
                networkStream.Write(packet, 0, packet.Length);                
                lstMensagens.Items.Add(msg);
                txtChat.Clear();
            }
        }
    }
}
