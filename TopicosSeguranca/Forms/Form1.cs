using EI.SI;
using System;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace TopicosSeguranca
{
    public partial class Form1 : Form
    {
        bool JaConectou = false;
        Conectar conectar = new Conectar();

        private RSACryptoServiceProvider rsa;
        private string minhaChavePublica;
        private string minhaChavePrivada;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnConectar_Click(object sender, EventArgs e)
        {
            if (JaConectou == false)
            {
                JaConectou = true;
                conectar.Conexao(this, rsa);
            }
        }

        private void btnEnviar_Click(object sender, EventArgs e)
        {
            string msg = txtChat.Text;
            conectar.LerMensagem(msg);
            txtChat.Clear();
        }
       
        public void AdicionarMensagemNoChat(string msg)
        {
            if (lstMensagens.InvokeRequired)
            {
                //Atualiza as mensagens em tempo real
                lstMensagens.Invoke(new Action(() => lstMensagens.Items.Add(msg)));
            }
            else
            {                
                lstMensagens.Items.Add(msg);
            }
        }

        private void txtChat_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {            
            rsa = new RSACryptoServiceProvider(2048);
            
            minhaChavePublica = rsa.ToXmlString(false);
            
            minhaChavePrivada = rsa.ToXmlString(true);

            // Só para testares e veres se funcionou (podes apagar isto depois):
            //MessageBox.Show("Chaves RSA geradas com sucesso ao iniciar o cliente!");
        }
    }
}