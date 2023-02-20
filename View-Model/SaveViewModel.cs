using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using NewtonsoftJson = Newtonsoft.Json;
using EasySaveV2.Model;

//Import des éléments du namespace EasySave
using EasySaveV2.View;
using System.Xml;
using System.Security.Cryptography.X509Certificates;



namespace EasySaveV2.View_Model
{
    public class SaveViewModel
    {
        private SaveModel _saveModel;

        public SaveViewModel()
        {
            _saveModel = new SaveModel(this);
        }
    }
}
