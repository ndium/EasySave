using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;


namespace EasySave.Model
{
    public class Statelog : LogJsonModel
    {
        public string State { get; set; }
        public int TotalFilesToCopy = 0;
        public int TotalFilesSize = 0;
        public int totalNbFilesleftToDo = 0;
        public int Progression = 0;



        public Statelog() : base() { }




        public void Savelog(long filesize, double transfertTime, int totalfilessize, int totalnbfiles, Config config, bool IsActived)
        {

            string backupConfigFile = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Easysave";
            string file = Path.Combine(backupConfigFile, "config.json");
            var listLog = new List<Statelog>();


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
                    Progression = totalfilessize



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



                string logjson = JsonConvert.SerializeObject(listLog);
                File.WriteAllText("statelog.json", logjson);
            }


        }

    }
}
  