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
        Conectar conectar = new Conectar();
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
                //ConectarServidor();
                conectar.Conexao(); 
            }            
        }

        private void btnEnviar_Click(object sender, EventArgs e)
        {
            //if (networkStream != null && client.Connected && !string.IsNullOrWhiteSpace(txtChat.Text))
            //{                
                string msg = txtChat.Text;
                conectar.LerMensagem(msg);
                //byte[] packet = protocolSI.Make(ProtocolSICmdType.DATA, msg);                
                //networkStream.Write(packet, 0, packet.Length);                
                lstMensagens.Items.Add(msg);
                txtChat.Clear();
            //}
        }
    }
}
