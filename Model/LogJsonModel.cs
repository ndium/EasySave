using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//NewtonsoftJson
using NewtonsoftJson = Newtonsoft.Json;
//serialisation
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;
using static EasySaveV2.Model.SaveModel;
using Newtonsoft.Json;
using System.Security.Principal;
using System.Net;
using System.Xml.Linq;
using System.Reflection;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;

namespace EasySaveV2.Model
{
    public class LogJsonModel
    {
        LangHelper langHelper = new LangHelper();
        public string Name { get; set; }
        public string FileSource { get; set; }

        public string FileTarget { get; set; }

        public int FileSize { get; set; }

        public double TransfertTime { get; set; }

        public string TimeStamp { get; set; }


        public List<Config> GetConfigFile(string path)
        {
            var filePath = Path.Combine(path, "config.json");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(filePath);
            }

            if (!File.Exists(filePath))
            {
                using (StreamWriter sw = File.CreateText(filePath))
                {
                    sw.Write("[]");
                }
            }

            string fileContent = File.ReadAllText(filePath);

            List<Config> JsonConfig = JsonConvert.DeserializeObject<List<Config>>(fileContent);
            return JsonConfig;
        }
        public Config ReadJsonConfig(string path, int index)
        {
            var filePath = Path.Combine(path, "config.json");
            string fileContent = File.ReadAllText(filePath);

            List<Config> JsonConfig = JsonConvert.DeserializeObject<List<Config>>(fileContent);
            var obj = JsonConfig[index];
            return obj;
        }
        public LogJsonModel getLogJsonModel() => this;

        public List<LogJsonModel> getListLog(string path)
        {
            string fileContent = File.ReadAllText(path);
            List<LogJsonModel> logJsonModels = JsonConvert.DeserializeObject<List<LogJsonModel>>(fileContent);


            return logJsonModels;
        }

        public void SaveLog(long filesize, double transfertTime, Config config)
        {


            string backupConfigFile = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\EasySaveV2";
            string file = Path.Combine(backupConfigFile, "config.json");
            string logFilePath = Path.Combine(backupConfigFile, "log.json");
            var listLog = new List<LogJsonModel>();

            //vérification de l'existence du fichier
            if (!File.Exists(@$"{logFilePath}"))
            {

                try
                {
                    //création de la liste
                    listLog.Add(new LogJsonModel
                    {
                        Name = config.BackupName,
                        FileSource = config.SourceDirectory,
                        FileTarget = config.TargetDirectory,
                        FileSize = (int)filesize,
                        TransfertTime = (int)transfertTime,
                        TimeStamp = DateTime.Now.ToString(),
                    });

                    string logjson = JsonConvert.SerializeObject(listLog);
                    File.WriteAllText(logFilePath, logjson);


                }
                catch (Exception ex)
                {
                    Console.Write($"{langHelper._rm.GetString("Error Daily Logs", CultureInfo.CurrentUICulture)}" + ex.Message);
                }
            }
            else
            {

                // Lire le fichier log.json existant
                var json = File.ReadAllText(logFilePath);

                // Désérialiser le JSON en objet C#
                var log = JsonConvert.DeserializeObject<List<LogJsonModel>>(json);

                // Ajouter un nouvel objet à la liste
                log.Add(new LogJsonModel
                {
                    Name = config.BackupName,
                    FileSource = config.SourceDirectory,
                    FileTarget = config.TargetDirectory,
                    FileSize = (int)filesize,
                    TransfertTime = transfertTime,
                    TimeStamp = DateTime.Now.ToString()
                });

                // Sérialiser l'objet mis à jour en JSON
                var updatedJson = JsonConvert.SerializeObject(log, Formatting.Indented);

                // Écrire le JSON mis à jour dans le fichier
                File.WriteAllText(logFilePath, updatedJson);
            }



        }
    }

}
