using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySaveV2.Model
{
    public class Extension
    {
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
            return _listExtension;
        }

        public void addExtensionToList(Extension extension)
        {
            _listExtension.Add(extension);
        }

        public void removeExtensionToList(string name)
        {
            foreach (Extension e in _listExtension)
            {
                if (e.Name == name)
                {
                    _listExtension.Remove(e);
                }
            }
        }
    }
}
