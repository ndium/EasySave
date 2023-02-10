using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using EasySave.View;
using NewtonsoftJson = Newtonsoft.Json;

//Import des éléments du namespace EasySave
using EasySave.View_Model;
using Newtonsoft.Json;

//classe de log
using static EasySave.Model.LogJsonModel;
using System.IO;

namespace EasySave.Model
{
    public class SaveModel
    {
        public string Name { get; set; }
        public string SourceRepo { get; set; }
        public string DestinationRepo { get; set; }
        public SaveType Type { get; set; }

        private SaveViewModel saveViewModel;
        public CreateModel CreateModel { get; set; }

        private LogJsonModel Log = new LogJsonModel();

        public static string[] files = Directory.GetFiles(@"C:\..\..\AppData\Roaming\backupconfigs", "*.json");

        public SaveModel(SaveViewModel saveViewModel)
        {
            this.saveViewModel = saveViewModel;
        }

    }
}



