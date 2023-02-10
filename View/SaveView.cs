using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Globalization;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

//Import des éléments du namespace EasySave
using EasySave.View_Model;
using System.Text.RegularExpressions;

namespace EasySave.View
{
    public class SaveView
    {
        private CopyView _copyView;
        private CreateView _createSaveView;
        private EditView _editSaveView;
        private DeleteView _deleteSaveView;
        private SaveViewModel _saveViewModel;
        private ChangeLangView _changeLangView;

        public SaveView()
        {
            _copyView = new CopyView();
            _createSaveView = new CreateView();
            _editSaveView = new EditView();
            _deleteSaveView = new DeleteView();
            _changeLangView = new ChangeLangView();
            int numOption = 0;
            numOption = Welcome();

            while (numOption != 9)
            {
                switch(numOption)
                    {
                    case 1:
                        Console.WriteLine("choix du changement de langue");
                        _changeLangView.Show();
                        break;

                    case 2:
                        Console.WriteLine("choix de la création de sauvegarde");
                        _createSaveView.Show();
                        numOption = Welcome();
                        break;

                    case 3:
                        Console.WriteLine("choix de la modification de sauvegarde");
                        _editSaveView.Show();
                        numOption = Welcome();
                        break;

                    case 4:
                        Console.WriteLine("choix de la suppression de sauvegarde");
                        _deleteSaveView.Show();
                        numOption = Welcome();
                        break;

                    case 5:
                        Console.WriteLine("Lancement d'une sauvegarde");
                        _copyView.Show();
                        numOption = Welcome();
                        break;
                    case 9:
                        Environment.Exit(0);
                        break;
                    default:
                        numOption = Welcome();
                        break;
                }
            }
        }


        public int Welcome()
        {
            Console.Title = "EasySave";
            Console.Clear();

            ResourceManager rm = new ResourceManager("EasySave.Lang.Resources", Assembly.GetExecutingAssembly());

            Console.WriteLine(" _______     ___           _______.____    ____  _______.     ___   ____    ____  _______ \r\n|   ____|   /   \\         /       |\\   \\  /   / /       |    /   \\  \\   \\  /   / |   ____|\r\n|  |__     /  ^  \\       |   (----` \\   \\/   / |   (----`   /  ^  \\  \\   \\/   /  |  |__   \r\n|   __|   /  /_\\  \\       \\   \\      \\_    _/   \\   \\      /  /_\\  \\  \\      /   |   __|  \r\n|  |____ /  _____  \\  .----)   |       |  | .----)   |    /  _____  \\  \\    /    |  |____ \r\n|_______/__/     \\__\\ |_______/        |__| |_______/    /__/     \\__\\  \\__/     |_______|\r\n\n                                                                                          ");
            Console.WriteLine($"{rm.GetString("Welcome", CultureInfo.CurrentUICulture)}\n");
            Console.WriteLine($"{rm.GetString("MenuSelectTitle", CultureInfo.CurrentUICulture)}\n");
            Console.Write($"{rm.GetString("FirstOption", CultureInfo.CurrentUICulture)}\n");
            Console.Write($"{rm.GetString("SecondOption", CultureInfo.CurrentUICulture)}\n");
            Console.Write($"{rm.GetString("ThirdOption", CultureInfo.CurrentUICulture)}\n");
            Console.Write($"{rm.GetString("FourthOption", CultureInfo.CurrentUICulture)}\n");
            Console.WriteLine($"{rm.GetString("FifthOption", CultureInfo.CurrentUICulture)}\n");
            Console.Write($"{rm.GetString("SelectOption", CultureInfo.CurrentUICulture)}");
            string ans = Console.ReadLine();
            int numOption = 0;
            if (int.TryParse(ans, out _))
            {
                numOption = Int32.Parse(ans);
                return numOption;
            }
            else
            {
                numOption = Welcome();
                return numOption;

            }

        }

    }
}
