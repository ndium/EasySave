using EasySaveV2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySaveV2.View_Model
{
    public class FiltersViewModel
    {
        public BusinessAppModel _businessAppModel { get; set; }
        public FiltersViewModel() 
        {
            _businessAppModel= new BusinessAppModel();
        }

        public void SaveApp(string AppName)
        {
            _businessAppModel.SaveApp(AppName);
        }

        public bool AppExists(string AppName) 
        {
            return _businessAppModel.AppExists(AppName);
        }

        public string GetJson()
        {
            return _businessAppModel.GetJson();
        }
    }
}
