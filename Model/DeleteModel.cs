using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace EasySaveV2.Model
{
    public class DeleteModel
    {
        LangHelper langHelper = new LangHelper();
        public DeleteModel()
        {

        }



        public void DeleteSave(List<Config> configsToDelete)
        {
            if (configsToDelete == null)
            {
                throw new Exception("NotFound");

            }
            string backupConfigFile = Global.JSON_PATH;
            var logJsonModel = new LogJsonModel();
            var allConfigs = logJsonModel.GetConfigFile(backupConfigFile);
            for (int i = 0; i < configsToDelete.Count; i++)
            {
                for (int j = 0; j < allConfigs.Count; j++)
                {
                    if (configsToDelete[i].BackupName == allConfigs[j].BackupName && configsToDelete[i].SourceDirectory == allConfigs[j].SourceDirectory
                        && configsToDelete[i].TargetDirectory == allConfigs[j].TargetDirectory && configsToDelete[i].BackupType == allConfigs[j].BackupType)
                    {
                        allConfigs.RemoveAt(j);
                    }
                }
            }

            string file = Path.Combine(backupConfigFile, "config.json");


            File.WriteAllText(file, JsonConvert.SerializeObject(allConfigs, Formatting.Indented));

        }

    }
}

