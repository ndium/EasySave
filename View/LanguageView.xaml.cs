using EasySaveV2.Model;
using EasySaveV2.View_Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace EasySaveV2.View
{
    /// <summary>
    /// Logique d'interaction pour LanguageView.xaml
    /// </summary>
    public partial class LanguageView : Page
    {
        private MainWindow _mainWindow { get; set; }
        public LanguageView(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
            InitializeComponent();
            DataContext = this;
            Translation();        
        }

        public void Translation()
        {
            LangHelper langHelper = new LangHelper();
            EnglishButton.Content = $"{langHelper._rm.GetString("ChangeButton")}";
            FrenchButton.Content = $"{langHelper._rm.GetString("ChangeButton")}";

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LangViewModel langViewModel = new LangViewModel();
            langViewModel.ChangeLanguageVM("en");
            Translation();
            _mainWindow.Translation();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            LangViewModel langViewModel = new LangViewModel();
            langViewModel.ChangeLanguageVM("fr");
            Translation();
            _mainWindow.Translation();
        }
    }
}
