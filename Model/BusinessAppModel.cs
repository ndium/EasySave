using EasySave.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySaveV2.Model
{
    public class BusinessAppModel
    {
        public BusinessAppModel()
        {

        }

        public string SaveApp(string AppName)
        {

            var BusinessApp = new BusinessApp();
            BusinessApp.AppName = AppName;

            var AppConfigs = new List<BusinessApp>();
            string AppConfigFile = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Easysave";

            if (!Directory.Exists(AppConfigFile))
            {
                Directory.CreateDirectory(AppConfigFile);
            }

            string file = Path.Combine(AppConfigFile, "applications.json");

            if (File.Exists(file))
            {
                AppConfigs = JsonConvert.DeserializeObject<List<BusinessApp>>(File.ReadAllText(file));
            }
            AppConfigs.Add(BusinessApp);
            File.WriteAllText(file, JsonConvert.SerializeObject(AppConfigs, Formatting.Indented));
            throw new Exception("BusinessAppSave");
        }
    }
}