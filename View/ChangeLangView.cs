using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasySaveV2.Model;
using Newtonsoft.Json;
using EasySaveV2.View_Model;



namespace EasySaveV2.View
{
    public class ChangeLangView
    {
        public ChangeLangView() { } 
        public void Show()
        {
            LangHelper langHelper = new LangHelper();
            Console.Write($"{ langHelper._rm.GetString("LangSelectorTitle")}\n");
            langHelper.ChangeLanguage(Console.ReadLine());
            //SaveView saveView= new SaveView();
            //saveView.Welcome();
        }
    }
}
