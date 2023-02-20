using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasySave.Model;

namespace EasySaveV2.View_Model
{
    public class LanguageViewModel
    {
        public LangHelper _langHelper { get; set; }
        public LanguageViewModel()
        {
            _langHelper = new LangHelper();
        }

        public void ChangeLanguageFrench()
        {
            _langHelper.ChangeLanguageFrench();
        }

        public void ChangeLanguageEnglish()
        {
            _langHelper.ChangeLanguageEnglish();
        }


    }
}
