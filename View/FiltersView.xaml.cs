using EasySaveV2.Model;
using System.IO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using EasySaveV2.View_Model;

namespace EasySaveV2.View
{
    /// <summary>
    /// Logique d'interaction pour FiltersView.xaml
    /// </summary>
    public partial class FiltersView : Page
    {
        public FiltersViewModel _filtersViewModel { get; set; }
        public FiltersView()
        {
            InitializeComponent();
            DataContext = this;
            _filtersViewModel = new FiltersViewModel();
            Refresh();
        }

        private void OnSaveApp(object sender, RoutedEventArgs e)
        {
            string AppName = txtAppName.Text;
            try
            {
                if (_filtersViewModel.AppExists(AppName))
                {
                    MessageBox.Show("L'application existe déjà.");
                }
                _filtersViewModel.SaveApp(AppName);
                MessageBox.Show("Application sauvegardée avec succès.");
            }
            catch (Exception)
            {
                MessageBox.Show("Erreur lors de la sauvegarde de l'application : " + AppName);
            }
            Refresh();
        }

        public void Refresh()
        {
            // Afficher le contenu dans une zone de texte
            myTextBox.Text = _filtersViewModel.GetJson();
        }


    }
}