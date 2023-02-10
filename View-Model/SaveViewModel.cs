using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using NewtonsoftJson = Newtonsoft.Json;
using EasySave.Model;

//Import des éléments du namespace EasySave
using EasySave.View;
using System.Xml;
using System.Security.Cryptography.X509Certificates;



namespace EasySave.View_Model
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
