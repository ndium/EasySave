using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasySave.View_Model;

namespace EasySave.View
{
    public class EditView
    {
        public EditView()
        {

        }

        public void Show()
        {
            var saveView = new SaveView();
            saveView.Welcome();

            /*while (true)
            {
                Console.WriteLine("1. Modifier une sauvegarde");
                Console.WriteLine("2. Quitter");
                Console.Write("Quel est votre choix : ");

                int choice = int.Parse(Console.ReadLine());
                if (choice == 2)
                {
                    break;
                }

                if (choice == 1)
                {
                    var editViewModel = new EditViewModel();

                    editViewModel.GetListConfig();
                    Console.Write("Quelle config voulez vous modifier : ");

                    int configToModify = int.Parse(Console.ReadLine());

                    Console.Write("Nouveau nom : ");
                    string newBackupName = Console.ReadLine();

                    Console.Write("Nouveau chemin source : ");
                    string newSourceDirectory = Console.ReadLine();

                    Console.Write("Nouveau chemin destination : ");
                    string newTargetDirectory = Console.ReadLine();

                    Console.Write("Nouveau type de sauvegarde : ");
                    string newBackupType = Console.ReadLine();

                    editViewModel.GetEditModel(newBackupName, newSourceDirectory, newTargetDirectory, newBackupType, configToModify);
                }


            }*/
        }
    }
}
