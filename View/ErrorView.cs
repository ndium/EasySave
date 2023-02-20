using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasySaveV2.Model;

namespace EasySaveV2.View
{
    public class ErrorView
    {
        LangHelper langHelper = new LangHelper();
        public ErrorView()
        {

        }
        
        public void ShowError(string message) 
        {
            Console.Write($"{langHelper._rm.GetString(message, CultureInfo.CurrentUICulture)}\n");
            Console.ReadKey();
            //SaveView mainView = new SaveView();
        }

    }
}
