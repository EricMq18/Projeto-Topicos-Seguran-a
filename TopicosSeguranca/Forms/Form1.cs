using System;
using System.Windows.Forms;
using System.Net.Sockets;
using EI.SI;

namespace TopicosSeguranca
{
    public partial class Form1 : Form
    {
        bool JaConectou = false;
        Conectar conectar = new Conectar();

        public Form1()
        {
            InitializeComponent();
        }

        private void btnConectar_Click(object sender, EventArgs e)
        {
            if (JaConectou == false)
            {
                JaConectou = true;
                // Passa o 'this' (este formulário) para a classe Conectar
                conectar.Conexao(this);
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
    }
}