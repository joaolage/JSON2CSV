using System;
using System.IO;

namespace JSON2CSV
{
    class Program
    {
        static void Main(string[] args)
        {
            string mensagem = string.Empty;

            try
            {
                // Utilizar o diretório da aplicação como diretório de trabalho
                DirectoryInfo diretorioBase = new DirectoryInfo(Directory.GetCurrentDirectory());

                //Instancia a classe responsável pela conversão

                Json2Csv json2csv = new Json2Csv(diretorioBase.ToString());
                mensagem = json2csv.ConverterJson2Csv();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Ocorretu um erro: " + ex.Message);
            }

            Console.WriteLine(mensagem);

            Console.ReadKey();
        }

        
    }
}
