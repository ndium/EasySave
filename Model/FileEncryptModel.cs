using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace EasySaveV2.Model
{
    public class Extension
    {
        public Extension(string extension)
        {
            Name = extension;
        }

        public string Name { get; set; }
    }
    public class FileEncryptModel
    {
        public List<Extension> _listExtension;
        public FileEncryptModel()
        {
            _listExtension = new List<Extension>();
        }

        public List<Extension> GetList()
        {
            string filePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\EasySave\extension.json";
            if (!File.Exists(filePath))
            {
                using (StreamWriter sw = File.CreateText(filePath))
                {
                    sw.Write("[]");
                }
            }
            string json = File.ReadAllText(filePath);
            _listExtension = JsonConvert.DeserializeObject<List<Extension>>(json);
            return _listExtension;
        }

        public void addExtensionToList(string extension)
        {
            _listExtension.Add(new Extension(extension));
            this.createJson();
        }

        public void removeExtensionToList(string name)
        {
            try
            {
                foreach (Extension e in _listExtension)
                {
                    if (e.Name == name)
                    {
                        _listExtension.Remove(e);
                    }
                }
            }
            catch (Exception e)
            {
            }
            
            this.createJson();
        }

        public void createJson()
        {
            string json = JsonConvert.SerializeObject(_listExtension, Formatting.Indented);
            string filePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\EasySaveV2\extension.json";

            //Si le fichier existe on le supprime pour un nouveau
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            File.WriteAllText(filePath, json);
        }
    }
}
