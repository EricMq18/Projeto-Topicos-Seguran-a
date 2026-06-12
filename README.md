# 🔒 Projeto Tópicos de Segurança - Chat Seguro (Cliente/Servidor)

Este projeto consiste numa aplicação de chat baseada na arquitetura **Cliente/Servidor** desenvolvida em C#. O foco principal da aplicação é a implementação de mecanismos de segurança da informação, garantindo a **confidencialidade**, **integridade** e **autenticidade** das mensagens trocadas entre os utilizadores através de criptografia híbrida.

## 🚀 Funcionalidades e Mecanismos de Segurança

* **Comunicação em Tempo Real:** Utilização de Sockets TCP (`TcpClient` e `TcpListener`) na porta `4000`.
* **Criptografia Assimétrica (RSA):** Utilizada para a troca segura das chaves simétricas. Cada cliente gera um par de chaves RSA de 2048 bits no momento em que a aplicação é iniciada.
* **Criptografia Simétrica (AES):** Utilizada para encriptar o conteúdo das mensagens do chat, garantindo uma comunicação rápida e confidencial. O servidor gera a chave AES e o respetivo vetor de inicialização (IV).
* **Assinaturas Digitais (SHA256 + RSA):** Cada mensagem enviada pelo cliente é assinada digitalmente com a sua chave privada. O servidor verifica a assinatura para garantir que a mensagem não foi adulterada e provém realmente do cliente indicado.
* **Sistema de Logging:** O servidor regista todos os eventos importantes (conexões, desconexões, verificação de assinaturas e erros) num ficheiro de texto local (`log_sistema.txt`).

## ⚙️ Arquitetura e Fluxo de Comunicação Seguro

A comunicação estabelece-se seguindo um protocolo rigoroso para garantir a segurança antes da troca de qualquer mensagem de texto:

1. **Handshake (Troca de Chaves):**
   * O **Cliente** conecta-se ao Servidor e envia a sua **Chave Pública RSA**.
   * O **Servidor** recebe a chave pública, encripta a **Chave Secreta AES** e o **IV** (Initialization Vector) com essa mesma chave pública, e envia-os de volta ao Cliente.
   * O **Cliente** recebe os pacotes, utiliza a sua **Chave Privada RSA** para os desencriptar e guarda a Chave AES e o IV. O canal seguro está agora estabelecido.

2. **Envio de Mensagens (Cifra e Assinatura):**
   * O **Cliente** escreve uma mensagem. O texto é encriptado com **AES**.
   * O texto original é assinado digitalmente utilizando a **Chave Privada RSA** do cliente e o algoritmo de hash **SHA256**.
   * O Cliente envia a mensagem encriptada e a assinatura separadas por um delimitador (`|#SIGN#|`).

3. **Verificação e Retransmissão:**
   * O **Servidor** recebe o pacote, separa a mensagem da assinatura, e desencripta a mensagem usando a chave **AES**.
   * Em seguida, o Servidor verifica a assinatura utilizando a **Chave Pública** daquele cliente específico.
   * Se a assinatura for **VÁLIDA**, o servidor faz o *broadcast* da mensagem para todos os outros clientes conectados. Caso seja inválida, a mensagem é descartada e o alerta é registado.

## 📁 Estrutura do Código

A solução está dividida em duas partes principais:

### 🖥️ Servidor (`servidor/`)
* `Program.cs`: Gere as conexões TCP de múltiplos clientes através de Threads (`lerMensagens`). Processa as chaves públicas, faz a validação das assinaturas digitais e retransmite as mensagens seguras.
* `Logger.cs`: Classe utilitária responsável por guardar o histórico da atividade do servidor num ficheiro `.txt` para efeitos de auditoria.

### 📱 Cliente (`TopicosSeguranca/`)
* `Form1.cs`: Interface Gráfica (Windows Forms) do chat. Gera o par de chaves RSA na inicialização da janela.
* `Conectar.cs`: Classe que gere a lógica de rede do cliente, incluindo a receção da chave AES, a cifragem/decifragem de mensagens e a geração das assinaturas digitais.

*Nota: Ambas as aplicações utilizam a biblioteca de rede `EI.SI` (ProtocolSI) para estruturação e tipagem dos pacotes de dados (ex: `ProtocolSICmdType.PUBLIC_KEY`, `ProtocolSICmdType.DATA`).*

## 🛠️ Pré-requisitos

* Visual Studio (ou outro IDE compatível com .NET).
* .NET Framework / .NET Core (versão compatível com a biblioteca `EI.SI`).
* A DLL `ProtocolSI.dll` devidamente referenciada nos dois projetos (Cliente e Servidor).

## ▶️ Como Executar

1. Abra a solução (ficheiro `.sln`) no Visual Studio.
2. Certifique-se de que a biblioteca `ProtocolSI.dll` está corretamente referenciada no projeto do Cliente e do Servidor.
3. Defina o projeto do **Servidor** como o projeto de arranque principal e inicie-o (`F5`). Deverá ver uma janela de consola a indicar: `Servidor Foi Iniciado`.
4. De seguida, inicie uma ou várias instâncias do projeto do **Cliente**.
5. Na interface do cliente, clique no botão para se conectar. Verá o registo na consola do Servidor a indicar que um novo cliente se ligou e enviou a sua chave pública.
6. A partir deste momento, as mensagens escritas no chat são enviadas de forma 100% segura.
