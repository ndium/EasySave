using EasySave.Model;
using EasySave.View_Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace EasySave.View
{

    public class CopyView
    {
        LangHelper langHelper = new LangHelper();

        public CopyView()
        {
        }
        public void Show()
        {
            //var viewModel = new CopyViewModel();
            //List<string> list = viewModel.GetConfigs();

            //List<int> selectedWorks = new List<int>();
            //bool done = false;
            //int i = 0;
            //string line = $"{i} \n";
            //string count = "";

            //while (!done)
            //{
            //    i = 0;
            //    Console.WriteLine("Choisissez un ou plusieurs travaux de sauvegarde (5 pour terminer) : \n");
            //    Console.WriteLine($"Actuellement selectionnés :{count}");
            //    foreach (var config in list)
            //    {
            //        line = $"{i} \n";
            //        Console.WriteLine($"{line}{config}");
            //        i++;

            //    }



            //    string input = Console.ReadLine();
            //    int option;


            //    if (int.TryParse(input, out option) && option >= 0 && option <= list.Count() - 1 || option == 5)
            //    {
            //        if (option == 5)
            //        {
            //            done = true;
            //        }
            //        else
            //        {
            //            selectedWorks.Add(option);
            //            count += $"[{option}],  ";
            //        }
            //    }
            //    else
            //    {
            //        Console.WriteLine("Entrée non valide. Veuillez réessayer.");
            //    }


                //    Console.WriteLine("Choisissez le type de sauvegarde : \n [1] - Complète \n [2] - Différentielle \n ");
                //    string sType = Console.ReadLine();
                //    int iType = 0;
                //    bool isParsed = int.TryParse(sType, out iType);
                //    while (isParsed == false || !Enum.IsDefined(typeof(SaveType), iType))
                //    {
                //        Console.WriteLine("Renseignez une valeur existante :");
                //        sType = Console.ReadLine();
                //        isParsed = int.TryParse(sType, out iType);

                //    }
                //    iType = Int32.Parse(sType);


                //}


            //}

            //viewModel.GetCopyModel(selectedWorks);
        }
    }
}
