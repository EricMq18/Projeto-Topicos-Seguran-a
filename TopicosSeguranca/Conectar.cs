using EI.SI;
using System;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Threading;
using System.Windows.Forms;
using System.IO;

namespace TopicosSeguranca
{
    internal class Conectar
    {
        TcpClient client;
        ProtocolSI protocolSI = new ProtocolSI();
        NetworkStream networkStream;

        private RSACryptoServiceProvider rsaCliente;
        private byte[] aesKey;
        private byte[] aesIV;

        // Passamos o Form1 como parâmetro para conseguir atualizar a interface gráfica
        public void Conexao(Form1 form)
        {
            try
            {
                client = new TcpClient("127.0.0.1", 4000);
                networkStream = client.GetStream();

                // Cria o pacote com o comando PUBLIC_KEY contendo a chave pública em XML
                byte[] pacoteChave = protocolSI.Make(ProtocolSICmdType.PUBLIC_KEY);

                // Envia o pacote para o servidor
                networkStream.Write(pacoteChave, 0, pacoteChave.Length);

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

        public void Conexao(Form1 form, RSACryptoServiceProvider rsa)
        {
            try
            {
                client = new TcpClient("127.0.0.1", 4000);
                networkStream = client.GetStream();

                // 1. Guardar a instância do RSA para usar mais tarde na decifragem
                this.rsaCliente = rsa;

                // 2. Extrair a chave pública em formato string XML
                string chavePublica = rsa.ToXmlString(false);

                // 3. Criar o pacote com o comando PUBLIC_KEY contendo a chave pública
                byte[] pacoteChave = protocolSI.Make(ProtocolSICmdType.PUBLIC_KEY, chavePublica);

                // 4. Enviar o pacote para o servidor
                networkStream.Write(pacoteChave, 0, pacoteChave.Length);

                // Cria e inicia a Thread de escuta do Servidor
                Thread threadEscuta = new Thread(() => OuvirServidor(form));
                threadEscuta.IsBackground = true;
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
                        // 1. Lê a mensagem cifrada que chegou
                        string msgCifrada = protocolReader.GetStringFromData();

                        // 2. DECIFRA A MENSAGEM usando a nossa chave AES
                        string msgDecifrada = DecifrarAES(msgCifrada);

                        // 3. Envia o texto limpo para o chat
                        form.AdicionarMensagemNoChat(msgDecifrada);
                    }
                    else if (protocolReader.GetCmdType() == ProtocolSICmdType.SECRET_KEY)
                    {
                        // 1. Receber os bytes cifrados da chave AES
                        byte[] keyCifrada = protocolReader.GetData();
                        // 2. Decifrar usando o nosso RSA (true activa o OAEP padding exigido)
                        aesKey = rsaCliente.Decrypt(keyCifrada, true);
                    }
                    else if (protocolReader.GetCmdType() == ProtocolSICmdType.IV)
                    {
                        // 1. Receber os bytes cifrados do Vetor de Inicialização (IV)
                        byte[] ivCifrado = protocolReader.GetData();
                        // 2. Decifrar usando o nosso RSA
                        aesIV = rsaCliente.Decrypt(ivCifrado, true);

                        MessageBox.Show("Chave AES e IV recebidos. Canal de comunicação seguro estabelecido!");
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
                // 1. CIFRAMOS A MENSAGEM ANTES DE ENVIAR
                string msgCifrada = CifrarAES(msg);

                // 2. ENVIAMOS O TEXTO CIFRADO (em Base64)
                byte[] packet = protocolSI.Make(ProtocolSICmdType.DATA, msgCifrada);
                networkStream.Write(packet, 0, packet.Length);
            }
        }

        private string CifrarAES(string textoPlano)
        {
            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = aesKey;
                aesAlg.IV = aesIV;
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(textoPlano);
                        }
                        return Convert.ToBase64String(msEncrypt.ToArray());
                    }
                }
            }
        }

        private string DecifrarAES(string textoCifradoBase64)
        {
            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = aesKey;
                aesAlg.IV = aesIV;
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(textoCifradoBase64)))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}