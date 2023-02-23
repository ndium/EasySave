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
            SaveSize.Content = $"{langHelper._rm.GetString("SaveAppButton")}";
            SaveExtension.Content = $"{langHelper._rm.GetString("SaveAppButton")}";
            ExtensionText.Text = $"{langHelper._rm.GetString("ExtensionName")}";
            SizeText.Text = $"{langHelper._rm.GetString("Sizetext")}";
            ActualSizeText.Text = $"{langHelper._rm.GetString("ActualSize")}";
            ListExt.Text = $"{langHelper._rm.GetString("ListExst")}";
            RemoveFilters_Click.Content = $"{langHelper._rm.GetString("DeleteButton")}";
            RemoveExtButon.Content = $"{langHelper._rm.GetString("DeleteButton")}";
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
            ActualSizeBox.Text = _filtersViewModel.GetSizeJson();
            ExtensionBoxList.Text = _filtersViewModel.GetExtJson();
            myTextBox.IsReadOnly = true;
            ActualSizeBox.IsReadOnly = true;
            ExtensionBoxList.IsReadOnly = true;
            Translation();
        }

        private void SaveSize_Click(object sender, RoutedEventArgs e)
        {
            LangHelper langHelper = new LangHelper();
            double Size;
            if (Double.TryParse(SizeBox.Text, out Size))
            {
                _filtersViewModel.SaveSize(Size);
                MessageBox.Show($"{langHelper._rm.GetString("SaveSize")}");
                Refresh();
            }
            else 
            { 
            MessageBox.Show("Error");
            }
        }

        private void SaveExtension_Click(object sender, RoutedEventArgs e)
        {
            LangHelper langHelper = new LangHelper();
            string Extension = ExtensionBox.Text;
            try
            {
                _filtersViewModel.Priority(Extension);
                MessageBox.Show($"{langHelper._rm.GetString("SaveExtension")}");
            }
            catch (Exception)
            {

                MessageBox.Show($"{langHelper._rm.GetString("Error")}" + Extension);
            }
            Refresh();
        }

        private void RemoveApp(object sender, RoutedEventArgs e)
        {
            
                LangHelper langHelper = new LangHelper();
                string AppName = txtAppName.Text;
                try
                {
                    _filtersViewModel.RemoveApp(AppName);
                    MessageBox.Show($"{langHelper._rm.GetString("RemoveApp")}");
                }
                catch (Exception)
                {

                    MessageBox.Show($"{langHelper._rm.GetString("Error")}" + AppName);
                }
                Refresh();
            
        }

        private void RemoveExt(object sender, RoutedEventArgs e)
        {

            LangHelper langHelper = new LangHelper();
            string Extension = ExtensionBox.Text;
            try
            {
                _filtersViewModel.RemoveExt(Extension);
                MessageBox.Show($"{langHelper._rm.GetString("RemoveExt")}");
            }
            catch (Exception)
            {

                MessageBox.Show($"{langHelper._rm.GetString("Error")}" + Extension);
            }
            Refresh();

        }
    }
}