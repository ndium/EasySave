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
using System.Windows;
using System.Xml;
using System.Xml.Serialization;
using System.Threading;

namespace EasySaveV2.Model
{
    public class LogJsonModel
    {
        static private SemaphoreSlim semaphore = new SemaphoreSlim(1);
        public string Name { get; set; }
        public string FileSource { get; set; }

        public string FileTarget { get; set; }

        public int FileSize { get; set; }

        public double TransfertTime { get; set; }

        public string TimeStamp { get; set; }

        public string CryptageTime { get; set; }

        public LogJsonModel()
        {

        }
        public  List<Config> GetConfigFile(string path)
        {

        
            var filePath = Path.Combine(path, "config.json");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
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
        public async Task<Config> ReadJsonConfig(string path, int index)
        {
            await semaphore.WaitAsync();

            var filePath = Path.Combine(path, "config.json");
            string fileContent = File.ReadAllText(filePath);

            List<Config> JsonConfig = JsonConvert.DeserializeObject<List<Config>>(fileContent);
            var obj = JsonConfig[index];
            return obj;
            semaphore.Release();
        }
        

        public List<LogJsonModel> getListLog()
        {
            string backupConfigFile = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\EasysaveV2";
            string logFilePath = Path.Combine(backupConfigFile, "log.json");

            string fileContent = File.ReadAllText(logFilePath);
            
           
            return JsonConvert.DeserializeObject<List<LogJsonModel>>(fileContent);
        }

        public string getListLogJson()
        {
            string backupConfigFile = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\EasysaveV2";
            string logFilePath = Path.Combine(backupConfigFile, "log.json");

            string fileContent = File.ReadAllText(logFilePath);
            return fileContent;
        }
        public string getListLogXML()
        {
            string backupConfigFile = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\EasysaveV2";
            string logFilePath = Path.Combine(backupConfigFile, "log.xml");


            string FileContent = "LOGS.XML NOT FOUND";

            // Check if the file exists
            if (File.Exists(logFilePath))
            {
                // Load the XML document
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(logFilePath);

                // Get the XML string from the document
                using (StringWriter stringWriter = new StringWriter())
                {
                    XmlTextWriter xmlWriter = new XmlTextWriter(stringWriter);
                    xmlDoc.WriteTo(xmlWriter);
                    FileContent = stringWriter.ToString();
                }

            }

            return FileContent;
        }

        public async Task ConvertLogs(string format)
        {
            await semaphore.WaitAsync();

            // Load the log data from the JSON file
            string backupConfigFile = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\EasysaveV2";
            string logFilePath = Path.Combine(backupConfigFile, "log.json");
            List<LogJsonModel> logData = JsonConvert.DeserializeObject<List<LogJsonModel>>(File.ReadAllText(logFilePath));

            if(format == "xml")
            { 
            // Create a serializer for the LogJsonModel class
            XmlSerializer serializer = new XmlSerializer(typeof(List<LogJsonModel>));

            // Serialize the log data to XML
            using (StreamWriter writer = new StreamWriter(Path.Combine(backupConfigFile, "log.xml")))
            {
                serializer.Serialize(writer, logData);
            }
            }
            
            semaphore.Release();

        }

        public async Task SaveLog(long filesize, double transfertTime,TimeSpan cryptotime,Config config)
        {
            await semaphore.WaitAsync();

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
                        CryptageTime = cryptotime.TotalMilliseconds.ToString()
                });

                    string logjson = JsonConvert.SerializeObject(listLog);
                    File.WriteAllText(logFilePath, logjson);


                }
                catch (Exception ex)
                {
                    //Console.Write($"{langHelper._rm.GetString("Error Daily Logs", CultureInfo.CurrentUICulture)}" + ex.Message);
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
                    TimeStamp = DateTime.Now.ToString(),
                    CryptageTime = cryptotime.TotalMilliseconds.ToString()
            });

                // Sérialiser l'objet mis à jour en JSON
                var updatedJson = JsonConvert.SerializeObject(log, NewtonsoftJson.Formatting.Indented);

                // Écrire le JSON mis à jour dans le fichier
                File.WriteAllText(logFilePath, updatedJson);
            }
            
          
              semaphore.Release();
           

        }
    }

}
