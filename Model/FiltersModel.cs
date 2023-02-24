using EasySaveV2.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EasySaveV2.Model
{
    public class FiltersModel
    {
        string LocalPath;
        public class Size
        {
            public double Value { get; set; }
        }

        public FiltersModel()
        {
            LocalPath = Global.JSON_PATH;
        }

        public List<FileInfo> CheckSizeAndPriority(string sourceFolder)
        {
            string PriorityPath = LocalPath + @"\Priority.json";
            FileInfo PriorityInfo = new FileInfo(PriorityPath);

            if (!PriorityInfo.Exists || PriorityInfo.Length == 0)
            {
                using (StreamWriter sw = File.CreateText(PriorityPath))
                {
                    sw.Write("[]");
                }
            }

            string SizePath = LocalPath + @"\size.json";
            FileInfo SizeInfo = new FileInfo(SizePath);

            if (!SizeInfo.Exists || SizeInfo.Length == 0)
            {
                using (StreamWriter sw = File.CreateText(SizePath))
                {
                    sw.Write("{\"Value\":100.0}");
                }
            }

            // Chargement des données de taille enregistrées
            string sizeJson = File.ReadAllText(Global.JSON_PATH + @"\size.json");
            Size sizeData = JsonConvert.DeserializeObject<Size>(sizeJson);
            double sizeLimit = sizeData.Value;

            // Chargement des extensions prioritaires
            string priorityJson = File.ReadAllText(Global.JSON_PATH + @"\Priority.json");
            List<ExtensionApp> priorityData = JsonConvert.DeserializeObject<List<ExtensionApp>>(priorityJson);

            // Chemin du répertoire à parcourir
            string directoryPath = sourceFolder;

            // Liste pour stocker les fichiers à traiter
            List<FileInfo> filesToProcess = new List<FileInfo>();
            List<FileInfo> oversizedFiles = new List<FileInfo>();

            // Parcours de tous les fichiers du répertoire
            foreach (string filePath in Directory.GetFiles(directoryPath))
            {
                // Récupération de l'extension du fichier
                string extension = Path.GetExtension(filePath).ToLower();

                // Vérification de la taille du fichier
                FileInfo fileInfo = new FileInfo(filePath);
                double fileSize = fileInfo.Length;

                if (fileSize <= sizeLimit)
                {
                    // Vérification de l'extension du fichier
                    int priority = priorityData.FindIndex(x => x.Extension == extension);
                    if (priority == -1)
                    {
                        priority = priorityData.Count;
                    }

                    // Ajout du fichier à traiter dans la liste
                    int insertIndex = filesToProcess.Count;
                    for (int i = 0; i < filesToProcess.Count; i++)
                    {
                        FileInfo existingFile = filesToProcess[i];
                        string existingExtension = existingFile.Extension.ToLower();
                        int existingPriority = priorityData.FindIndex(x => x.Extension == existingExtension);
                        if (existingPriority == -1)
                        {
                            existingPriority = priorityData.Count;
                        }
                        if (priority > existingPriority ||
                            (priority == existingPriority && fileSize > existingFile.Length))
                        {
                            insertIndex = i;
                            break;
                        }
                    }
                    filesToProcess.Insert(insertIndex, fileInfo);
                }
                else
                {
                    oversizedFiles.Add(fileInfo);
                }
            }

            // Ajout des fichiers dépassant la limite de taille à la fin de la liste
            oversizedFiles.AddRange(filesToProcess);

            return oversizedFiles;
        }

        public void SaveSize(double Size)
        {

            if (!Directory.Exists(LocalPath))
            {
                Directory.CreateDirectory(LocalPath);
            }

            if (!File.Exists(LocalPath + @"\size.json"))
            {
                using (StreamWriter sw = File.CreateText(LocalPath + @"\size.json"))
                {
                    sw.Write("[]");
                }
            }

            Size size = new Size();
            size.Value = Size;
            string file = Path.Combine(LocalPath, "size.json");

            File.WriteAllText(file, JsonConvert.SerializeObject(size));
        }

        public void SaveApp(string AppName)
        {

            var BusinessApp = new BusinessApp();
            BusinessApp.AppName = AppName;

            var AppConfigs = new List<BusinessApp>();

            if (!Directory.Exists(LocalPath))
            {
                Directory.CreateDirectory(LocalPath);
            }

            string file = Path.Combine(LocalPath, "applications.json");

            if (File.Exists(file))
            {
                AppConfigs = JsonConvert.DeserializeObject<List<BusinessApp>>(File.ReadAllText(file));
            }
            AppConfigs.Add(BusinessApp);
            File.WriteAllText(file, JsonConvert.SerializeObject(AppConfigs, Formatting.Indented));
        }

        public void RemoveApp(string AppName)
        {
            var BusinessApp = new BusinessApp();
            BusinessApp.AppName = AppName;

            List<BusinessApp> AppConfigs = new List<BusinessApp>();

            string file = Path.Combine(LocalPath, "applications.json");

            if (File.Exists(file))
            {
                AppConfigs = JsonConvert.DeserializeObject<List<BusinessApp>>(File.ReadAllText(file));
                /*                AppConfigs.Remove(BusinessApp);*/
                try
                {
                    foreach (BusinessApp app in AppConfigs)
                    {
                        if (app.AppName == AppName)
                        {
                            AppConfigs.Remove(app);
                        }
                    }
                }
                catch { }
            }
            File.WriteAllText(file, JsonConvert.SerializeObject(AppConfigs, Formatting.Indented));
        }

        public void RemoveExt(string Extension)
        {
            var ExtensionApp = new ExtensionApp();
            ExtensionApp.Extension = Extension;

            List<ExtensionApp> AppConfigs = new List<ExtensionApp>();

            string file = Path.Combine(LocalPath, "Priority.json");

            if (File.Exists(file))
            {
                AppConfigs = JsonConvert.DeserializeObject<List<ExtensionApp>>(File.ReadAllText(file));
                /*                AppConfigs.Remove(BusinessApp);*/
                try
                {
                    foreach (ExtensionApp ext in AppConfigs)
                    {
                        if (ext.Extension == Extension)
                        {
                            AppConfigs.Remove(ext);
                        }
                    }
                }
                catch { }
            }
            File.WriteAllText(file, JsonConvert.SerializeObject(AppConfigs, Formatting.Indented));
        }


        public bool AppExists(string appName)
        {
            string file = Path.Combine(LocalPath, "applications.json");
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

            if (!Directory.Exists(LocalPath))
            {
                Directory.CreateDirectory(LocalPath);
            }
            if (!File.Exists(LocalPath + @"\applications.json"))
            {
                using (StreamWriter sw = File.CreateText(LocalPath + @"\applications.json"))
                {
                    sw.Write("[]");
                }
            }

            string file = System.IO.Path.Combine(LocalPath, "applications.json");

            string json = File.ReadAllText(file);

            // Désérialiser le contenu JSON en un objet dynamique
            dynamic data = JsonConvert.DeserializeObject(json);

            // Convertir l'objet JSON en une chaîne JSON formatée
            string formattedJson = JsonConvert.SerializeObject(data, Formatting.Indented);

            // Supprimer les caractères spéciaux de la chaîne JSON
            string cleanedJson = Regex.Replace(formattedJson, @"[{}[\],""]+", "");

            return cleanedJson;
        }
        public string GetSizeJson()
        {

            if (!Directory.Exists(LocalPath))
            {
                Directory.CreateDirectory(LocalPath);
            }
            if (!File.Exists(LocalPath + @"\size.json"))
            {
                using (StreamWriter sw = File.CreateText(LocalPath + @"\size.json"))
                {
                    sw.Write("{\"Value\":100.0}");
                }
            }

            string file = System.IO.Path.Combine(LocalPath, "size.json");

            string json = File.ReadAllText(file);

            // Désérialiser le contenu JSON en un objet dynamique
            Size data = JsonConvert.DeserializeObject<Size>(json);


            // Supprimer les caractères spéciaux de la chaîne JSON

            return data.Value.ToString();
        }

        public string GetExtJson()
        {

            if (!Directory.Exists(LocalPath))
            {
                Directory.CreateDirectory(LocalPath);
            }
            if (!File.Exists(LocalPath + @"\Priority.json"))
            {
                using (StreamWriter sw = File.CreateText(LocalPath + @"\Priority.json"))
                {
                    sw.Write("[]");
                }
            }

            string file = System.IO.Path.Combine(LocalPath, "Priority.json");

            string json = File.ReadAllText(file);

            // Désérialiser le contenu JSON en un objet dynamique
            dynamic data = JsonConvert.DeserializeObject(json);

            // Convertir l'objet JSON en une chaîne JSON formatée
            string formattedJson = JsonConvert.SerializeObject(data, Formatting.Indented);

            // Supprimer les caractères spéciaux de la chaîne JSON
            string cleanedJson = Regex.Replace(formattedJson, @"[{}[\],""]+", "");

            return cleanedJson;
        }

        public void Priority(string Extension)
        {

            var ExtensionApp = new ExtensionApp();
            ExtensionApp.Extension = Extension;

            var ExtensionConfigs = new List<ExtensionApp>();

            if (!Directory.Exists(LocalPath))
            {
                Directory.CreateDirectory(LocalPath);
            }

            if (!File.Exists(LocalPath + @"\Priority.json"))
            {
                using (StreamWriter sw = File.CreateText(LocalPath + @"\Priority.json"))
                {
                    sw.Write("[]");
                }
            }

            string file = Path.Combine(LocalPath, "Priority.json");

            if (File.Exists(file))
            {
                ExtensionConfigs = JsonConvert.DeserializeObject<List<ExtensionApp>>(File.ReadAllText(file));
            }
            ExtensionConfigs.Add(ExtensionApp);
            File.WriteAllText(file, JsonConvert.SerializeObject(ExtensionConfigs, Formatting.Indented));
        }

        public bool CheckBusinessApp()
        {
            // Charger les applications à partir du fichier JSON

            
            string file = Path.Combine(LocalPath, "applications.json");
            if (!File.Exists(file))
            {
                return true;
            }
            string json = File.ReadAllText(file);
            List<BusinessApp> applications = JsonConvert.DeserializeObject<List<BusinessApp>>(json);

            // Vérifier si les applications sont ouvertes
            bool canSave = true;
            foreach (var application in applications)
            {
                Process[] processList = Process.GetProcessesByName(application.AppName);
                if (processList.Length > 0)
                {
                    canSave = false;
                    break;
                }
            }
            return canSave;
        }

    }
}