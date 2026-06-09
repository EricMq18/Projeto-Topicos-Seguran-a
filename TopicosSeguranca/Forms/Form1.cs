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

            // REMOVIDO: lstMensagens.Items.Add(msg); 
            // Motivo: O servidor vai ecoar de volta para você, caindo no método abaixo.

            txtChat.Clear();
        }

        // Método Público e Thread-Safe para adicionar as mensagens vindas do servidor
        public void AdicionarMensagemNoChat(string msg)
        {
            if (lstMensagens.InvokeRequired)
            {
                // Se veio de outra thread (da classe Conectar), sincroniza com a thread da UI
                lstMensagens.Invoke(new Action(() => lstMensagens.Items.Add(msg)));
            }
            else
            {
                // Se já estiver na thread principal
                lstMensagens.Items.Add(msg);
            }
        }

        private void txtChat_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Inicializar o RSA com um tamanho de chave de 2048 bits
            rsa = new RSACryptoServiceProvider(2048);

            // Extrair a Chave Pública (false = não inclui a privada)
            minhaChavePublica = rsa.ToXmlString(false);

            // Extrair a Chave Privada (true = inclui a privada)
            minhaChavePrivada = rsa.ToXmlString(true);

            // Só para testares e veres se funcionou (podes apagar isto depois):
            //MessageBox.Show("Chaves RSA geradas com sucesso ao iniciar o cliente!");
        }
    }
}