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
            Translation();
        }

        public void Translation()
        {
            LangHelper langHelper = new LangHelper();
            AppList.Text = $"{langHelper._rm.GetString("AppList")}";
            AddApp.Text = $"{langHelper._rm.GetString("AddApp")}";
            SaveFilters_Click.Content = $"{langHelper._rm.GetString("SaveAppButton")}";
        }

        private void OnSaveApp(object sender, RoutedEventArgs e)
        {
            LangHelper langHelper = new LangHelper();
            string AppName = txtAppName.Text;
            try
            {
                if (_filtersViewModel.AppExists(AppName))
                {
                    MessageBox.Show($"{langHelper._rm.GetString("ErrorApp")}");
                }
                _filtersViewModel.SaveApp(AppName);
                MessageBox.Show($"{langHelper._rm.GetString("SaveApp")}");
            }
            catch (Exception)
            {

                MessageBox.Show($"{langHelper._rm.GetString("ErrorApp")}" + AppName);
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