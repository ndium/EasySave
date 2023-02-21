using EasySaveV2.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EasySaveV2.Model
{
    public class BusinessAppModel
    {
        public BusinessAppModel()
        {

        }

        public void SaveApp(string AppName)
        {

            var BusinessApp = new BusinessApp();
            BusinessApp.AppName = AppName;

            var AppConfigs = new List<BusinessApp>();
            string AppConfigFile = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\EasySaveV2";

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
        }
        public bool AppExists(string appName)
        {
            string appPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\EasySaveV2";
            string file = Path.Combine(appPath, "applications.json");
            if (!File.Exists(file))
            {
                return false;
            }
            string json = File.ReadAllText(file);
            var applications = JsonConvert.DeserializeObject<string[]>(json);
            return applications.Contains(appName);
        }

        public string GetJson()
        {
            string AppConfigFile = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\EasySaveV2";

            if (!Directory.Exists(AppConfigFile))
            {
                Directory.CreateDirectory(AppConfigFile);
            }
            if (!File.Exists(AppConfigFile + @"\applications.json"))
            {
                using (StreamWriter sw = File.CreateText(AppConfigFile + @"\applications.json"))
                {
                    sw.Write("[]");
                }
            }

            string file = System.IO.Path.Combine(AppConfigFile, "applications.json");

            string json = File.ReadAllText(file);

            // Désérialiser le contenu JSON en un objet dynamique
            dynamic data = JsonConvert.DeserializeObject(json);

            // Convertir l'objet JSON en une chaîne JSON formatée
            string formattedJson = JsonConvert.SerializeObject(data, Formatting.Indented);

            // Supprimer les caractères spéciaux de la chaîne JSON
            string cleanedJson = Regex.Replace(formattedJson, @"[{}[\],""]+", "");

            return cleanedJson;
        }

    }
}