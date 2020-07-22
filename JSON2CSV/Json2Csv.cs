using Newtonsoft.Json;
using System;
using System.Data;
using System.IO;
using System.Text;

namespace JSON2CSV
{
    public class Json2Csv
    {
        public string PathTrabalho { get; private set; }
        public string PathJSON { get; private set; }
        public string PathCSV { get; private set; }
        public string PathLog { get; private set; }

        public Json2Csv(string diretorioBase)
        {
            try
            {
                if (!string.IsNullOrEmpty(diretorioBase))
                {
                    //Verificar se o diretório base existe
                    if (!Directory.Exists(diretorioBase))
                        Directory.CreateDirectory(diretorioBase);

                    PathTrabalho = diretorioBase + "\\JSON2CSV";
                    PathJSON = PathTrabalho + "\\JSON";
                    PathCSV = PathTrabalho + "\\CSV";
                    PathLog = PathTrabalho + "\\Log";

                    //Verificar se a pasta de trabalho JSON2CSV existe:
                    if (!Directory.Exists(PathTrabalho))
                        Directory.CreateDirectory(PathTrabalho);

                    //Verificar se a pasta do JSON existe:
                    if (!Directory.Exists(PathJSON))
                        Directory.CreateDirectory(PathJSON);

                    //Verificar se a pasta do CSV existe:
                    if (!Directory.Exists(PathCSV))
                        Directory.CreateDirectory(PathCSV);

                    //Verificar se a pasta do Log existe:
                    if (!Directory.Exists(PathLog))
                        Directory.CreateDirectory(PathLog);
                }
                else
                {
                    throw new Exception("Diretório base não pode estar em branco ou ser um diretório inválido.");
                }
            }
            catch
            {
                throw;
            }            
        }

        public string ConverterJson2Csv()
        {
            //Ler todos os arquivos do diretório
            DirectoryInfo diretorio = new DirectoryInfo(PathJSON);
            FileInfo[] arquivos = diretorio.GetFiles();

            if(arquivos.Length > 0)
            {
                StringBuilder sbLog = new StringBuilder();
                sbLog.Append(string.Format(@"{0} - Covertendo {1} arquivo(s).",
                             DateTime.Now.ToString(),
                             arquivos.Length));
                sbLog.AppendLine();

                foreach (var arquivo in arquivos)
                {
                    //Ler os arquivos
                    string conteudoJson = LerArquivo(arquivo.FullName);
                    string conteudoCsv = Converter(conteudoJson);

                    sbLog.Append(string.Format(@"{0} - Início da conversão do arquivo {1}.",
                             DateTime.Now.ToString(),
                             arquivo.Name));
                    sbLog.AppendLine();
                  
                    EscreverArquivo(conteudoCsv, PathCSV, arquivo.Name.Replace(".json", ".csv"));

                    sbLog.Append(string.Format(@"{0} - Fim da conversão do arquivo {1}.",
                             DateTime.Now.ToString(),
                             arquivo.Name));
                    sbLog.AppendLine();
                    sbLog.AppendLine();
                }

                EscreverArquivo(sbLog.ToString(), PathLog, DateTime.Now.ToString("yyyy-mm-dd_HH-mm-ss") + ".log");
            }
            else
            {
                throw new Exception("O diretório não contém nenhum arquivo para conversão.");
            }
            
            return "Arquivos convertidos com sucesso.";
        }

        private static bool EscreverArquivo(string conteudo, string path, string fileNane)
        {
            bool sucesso = false;
            try
            {
                if (File.Exists(path + "\\" + fileNane))
                {
                    File.Delete(path + "\\" + fileNane);
                }
                else
                {
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);
                }

                File.WriteAllText(path + "\\" + fileNane, conteudo);
                sucesso = true;
            }
            catch (Exception ex)
            {
                throw;
            }

            return sucesso;
        }

        private string LerArquivo(string path)
        {
            string conteudo = "";

            try
            {
                using (StreamReader r = new StreamReader(path))
                {
                    conteudo = r.ReadToEnd();
                }   
            }
            catch (Exception ex)
            {
                conteudo = "";
                throw;
            }
            return conteudo;
        }

        private string Converter(string json)
        {

            DataTable dt = JsonConvert.DeserializeObject<DataTable>(json);
            StringBuilder sb = new StringBuilder();

            //Escreve os títulos das colunas
            foreach (DataColumn coluna in dt.Columns)
            {
                sb.Append(coluna.ColumnName + ',');
            }
            sb.Remove(sb.Length - 1, 1);
            sb.AppendLine();

            //Escreve os conteúdos das colunas
            foreach (DataRow row in dt.Rows)
            {
                for (var i = 0; i < dt.Columns.Count; i++)
                {
                    sb.Append(row[i].ToString() + ',');
                }
                sb.Remove(sb.Length - 1, 1);
                sb.AppendLine();
            }
            return sb.ToString();
        }      
    }
}
