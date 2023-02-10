using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasySave.View_Model;
using System.Globalization;
using EasySave.Model;

namespace EasySave.View
{
    public class DeleteView
    {
        LangHelper langHelper = new LangHelper();

        public void Show()
        {
            var deleteViewModel = new DeleteViewModel();
            List<string> listConfig;
            
            listConfig = deleteViewModel.GetListConfig();

            foreach (string x in listConfig)
            {
                Console.WriteLine(x);
            }

            Console.Write($"{langHelper._rm.GetString("Delete", CultureInfo.CurrentUICulture)}\n");

            int numConfig = Int32.Parse(Console.ReadLine());

            Console.WriteLine(deleteViewModel.SetNumConfigModel(numConfig));
        }
    }
}
