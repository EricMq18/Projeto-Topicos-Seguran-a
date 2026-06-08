using System;
using System.IO;
using System.Text;

namespace servidor
{
    public static class Logger
    {
        public static void Gravar(string mensagem)
        {
            try
            {
                string registo = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {mensagem}";

                // Força a buscar o caminho exato onde o executável está a correr
                string caminhoExato = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log_sistema.txt");

                File.AppendAllText(caminhoExato, registo + Environment.NewLine, Encoding.UTF8);

                // Imprime a verde na consola o local exato onde o ficheiro foi criado/atualizado
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"[LOG GUARDADO EM: {caminhoExato}]");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                // Se der erro, agora imprime a vermelho na consola para sabermos porquê
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[ERRO AO GRAVAR LOG]: {ex.Message}");
                Console.ResetColor();
            }
        }
    }
}