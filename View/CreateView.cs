using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasySave.Model;
using EasySave.View_Model;
using Newtonsoft.Json;

namespace EasySave.View
{

    public class CreateView
    {
        LangHelper langHelper = new LangHelper();

        public CreateView() { }

        public void Show()
        {
            Console.Write($"{langHelper._rm.GetString("Name")}\n");
            string backupName = Console.ReadLine();

            Console.Write($"{langHelper._rm.GetString("sourceDirectory")}\n");
            string sourceDirectory = Console.ReadLine();
            while (!Directory.Exists(sourceDirectory))
            {
                Console.Write("Le chemin de source n'existe pas, veuillez entrer un chemin à nouveau :");
                sourceDirectory = Console.ReadLine();
            }
            Console.Write($"{langHelper._rm.GetString("targetDirectory")}\n");
            string targetDirectory = Console.ReadLine();
            while (!Directory.Exists(targetDirectory))
            {
                Console.Write("Le chemin de destination n'existe pas, veuillez entrer un chemin à nouveau :");
                targetDirectory = Console.ReadLine();
            }
            Console.Write($"{langHelper._rm.GetString("backuptype")}\n");
            string backupType = Console.ReadLine();




            int iType = 0;
            bool isParsed = int.TryParse(backupType, out iType);
            while (isParsed == false || !Enum.IsDefined(typeof(SaveType), iType))
            {
                Console.Write("Renseignez 1 pour Complet ou 2 pour Differentiel : ");
                backupType = Console.ReadLine();
                isParsed = int.TryParse(backupType, out iType);
            }

            var createViewModel = new CreateViewModel();
            createViewModel.GetCreateModel(backupName, sourceDirectory, targetDirectory, backupType);
        }
    }

}
