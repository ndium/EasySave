using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace EasySaveV2.Model
{
    public class Statelog : LogJsonModel
    {
        public string State { get; set; }
        public int TotalFilesToCopy = 0;
        public long TotalFilesSize = 0;
        public int totalNbFilesleftToDo = 0;
        public int Progression = 0;



        public Statelog() : base() { }




        public void SaveLog(long filesize, double transfertTime, long totalfilessize, int totalnbfiles, Config config, bool IsActived)
        {

            string backupConfigFile = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\EasySaveV2";
            string file = Path.Combine(backupConfigFile, "config.json");
            List<Statelog> listLog = new List<Statelog>();
            string logFilePath = Path.Combine(backupConfigFile, "statelog.json");

            //vérification de l'existence du fichier
            if (!File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\EasysaveV2\statelog.json"))
            {
                using (StreamWriter sw = File.CreateText(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\EasysaveV2\statelog.json"))
                {
                    sw.Write("[]");
                }
            }
            
            

                // Lire le fichier statelog.json existant
                string json = File.ReadAllText(logFilePath);
                //string json = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Easysave\statelog.json";

                // Désérialiser le JSON en objet C#
                listLog = JsonConvert.DeserializeObject<List<Statelog>>(json);

                // Ajouter un nouvel objet à la liste
                if (IsActived == true)
                {
                    listLog.Add(new Statelog
                    {
                        Name = config.BackupName,
                        FileSource = config.SourceDirectory,
                        FileTarget = config.TargetDirectory,
                        FileSize = (int)filesize,
                        TransfertTime = (int)transfertTime,
                        TimeStamp = DateTime.Now.ToString(),
                        State = "ACTIVE",
                        TotalFilesToCopy = totalnbfiles,
                        TotalFilesSize = totalfilessize,
                        totalNbFilesleftToDo = totalnbfiles,
                        Progression = (int)totalfilessize



                    });

                }
                else
                {
                    listLog.Add(new Statelog
                    {
                        Name = config.BackupName,
                        FileSource = config.SourceDirectory,
                        FileTarget = config.TargetDirectory,
                        FileSize = (int)filesize,
                        TransfertTime = (int)transfertTime,
                        TimeStamp = DateTime.Now.ToString(),
                        State = "END",
                    });
                }
                // Sérialiser l'objet mis à jour en JSON
                var updatedJson = JsonConvert.SerializeObject(listLog, Formatting.Indented);

                // Écrire le JSON mis à jour dans le fichier
                File.WriteAllText(logFilePath, updatedJson);
            

        }

    }
}
  