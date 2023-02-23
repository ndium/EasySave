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
        public FiltersModel _filtersModel { get; set; }
        public FiltersViewModel() 
        {
            _filtersModel= new FiltersModel();
        }

        public void SaveApp(string AppName)
        {
            _filtersModel.SaveApp(AppName);
        }
        public void SaveSize(double Size)
        {
            _filtersModel.SaveSize(Size);
        }

        public void Priority(string Extension)
        {
            _filtersModel.Priority(Extension);
        }

        public bool AppExists(string AppName) 
        {
            return _filtersModel.AppExists(AppName);
        }

        public string GetJson()
        {
            return _filtersModel.GetJson();
        }
        public string GetSizeJson()
        {
            return _filtersModel.GetSizeJson();
        }
        public string GetExtJson()
        {
            return _filtersModel.GetExtJson();
        }

        
        public void RemoveApp(string AppName)
        {
            _filtersModel.RemoveApp(AppName);
        }
        public void RemoveExt(string Extension)
        {
            _filtersModel.RemoveExt(Extension);
        }
    }
    
}
