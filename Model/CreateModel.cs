using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave.Model
{
    public class CreateModel
    {
      LogJsonModel LogJsonModel = new LogJsonModel();
        LangHelper langHelper = new LangHelper();

        public CreateModel()
        {

        }

        public string CreateSave (string backupName, string sourceDirectory, string targetDirectory, string backupType)
        {
            
                var config = new Config();
                config.BackupName = backupName;
                config.SourceDirectory = sourceDirectory;
                config.TargetDirectory = targetDirectory;
                config.BackupType = backupType;

                var backupConfigs = new List<Config>();
                string backupConfigFile = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Easysave";

                if (!Directory.Exists(backupConfigFile))
                {
                    Directory.CreateDirectory(backupConfigFile);
                }

                string file = Path.Combine(backupConfigFile, "config.json");

                if (File.Exists(file))
                {
                    backupConfigs = JsonConvert.DeserializeObject<List<Config>>(File.ReadAllText(file));
                }
                backupConfigs.Add(config);
                File.WriteAllText(file, JsonConvert.SerializeObject(backupConfigs, Formatting.Indented));
                       throw new Exception("SaveCreated");
        }
    }
}
