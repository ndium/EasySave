using EasySave.Model;
using EasySave.View;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave.View_Model
{
    public class CopyViewModel
    {
        string _backupConfigFile;
        string _file;
        public CopyViewModel()
        {
            string backupConfigFile = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Easysave";
            string file = Path.Combine(backupConfigFile, "config.json");
            _backupConfigFile = backupConfigFile;
            _file = file;
        }

        public void GetCopyModel(List<int> selectedWorks)
        {
            var copyModel = new CopyModel();
            foreach (int index in selectedWorks)
            {
                var config = GetConfigInfo(index);
                if (config.BackupType == "1")
                {
                    copyModel.FullCopy(config);
                }
                else if (config.BackupType == "2")
                {
                    copyModel.DifferentialCopy(config);
                }
            }
        }

        public Config GetConfigInfo(int index)
        {
            var jsonModel = new LogJsonModel();
            Config obj = jsonModel.ReadJsonConfig(_file, index);
            return obj;

        }
        public List<Config> GetConfigs()
        {

            var jsonModel = new LogJsonModel();
            List<Config> obj = jsonModel.GetConfigFile(_file);
            return obj;
        }
    }
}
