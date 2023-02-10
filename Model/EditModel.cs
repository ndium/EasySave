using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace EasySave.Model
{
    public class EditModel
    {
        public EditModel()
        {
        }

        public void GetListConfigToModify()
        {/*
            var backupConfigs = new List<Config>();
            var backupConfigFile = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Easysave\config.json";

            if (File.Exists(backupConfigFile))
            {
                backupConfigs = JsonConvert.DeserializeObject<List<Config>>(File.ReadAllText(backupConfigFile));
            }

            for (int k = 0; k < backupConfigs.Count; k++)
            {
                 backupConfigs.Add($"{k + 1}. {backupConfigs[k].BackupName}");
            }
            return backupConfigs;*/
        }

        public string EditSave(string newBackupName, string newSourceDirectory, string newTargetDirectory, string newBackupType, int configToModify)
        {
            try
            {
                var backupConfigs = new List<Config>();
                string backupConfigFile = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Easysave\config.json";

                if (File.Exists(backupConfigFile))
                {
                    backupConfigs = JsonConvert.DeserializeObject<List<Config>>(File.ReadAllText(backupConfigFile));
                }

                if (backupConfigs.Count == 0)
                {
                    throw new Exception("Il n'y a aucune configuration à modifier");
                }

                for (int i = 0; i < backupConfigs.Count; i++)
                {
                    for (int k = 0; k < backupConfigs.Count; k++)
                    {
                        throw new Exception($"{k + 1}. {backupConfigs[k].BackupName}");
                    }

                    if (configToModify > 0 && configToModify <= backupConfigs.Count)
                    {
                        backupConfigs[configToModify - 1].BackupName = newBackupName;
                        backupConfigs[configToModify - 1].SourceDirectory = newSourceDirectory;
                        backupConfigs[configToModify - 1].TargetDirectory = newTargetDirectory;
                        backupConfigs[configToModify - 1].BackupType = newBackupType;

                        File.WriteAllText(backupConfigFile,
                            JsonConvert.SerializeObject(backupConfigs, Formatting.Indented));
                    }
                    else
                    {
                        throw new Exception("La configuration n'a pas été trouvée");
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
