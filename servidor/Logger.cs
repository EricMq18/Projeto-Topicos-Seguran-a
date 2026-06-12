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
                //Serve para mostrar onde foi salvo arquivo a verde e a mensagem a vermelho
                string registo = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {mensagem}";
                
                string caminhoExato = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log_sistema.txt");

                File.AppendAllText(caminhoExato, registo + Environment.NewLine, Encoding.UTF8);
                
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"[LOG GUARDADO EM: {caminhoExato}]");
                Console.ResetColor();
            }
            catch (Exception ex)
            {                
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[ERRO AO GRAVAR LOG]: {ex.Message}");
                Console.ResetColor();
            }
        }
    }
}