using EasySaveV2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySaveV2.View_Model
{
    public class LangViewModel
    {
        
        public void ChangeLanguageVM(string language) 
        {
            LangHelper langHelper = new LangHelper();
            langHelper.ChangeLanguage(language);
        }  
    }
}
