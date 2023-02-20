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
using EasySaveV2.View_Model;

namespace EasySaveV2.View
{
    /// <summary>
    /// Logique d'interaction pour LanguageView.xaml
    /// </summary>
    public partial class LanguageView : Page
    {
        public LanguageViewModel _languageViewModel { get; set; }
        public LanguageView()
        {
            InitializeComponent();
            DataContext = this;
            _languageViewModel = new LanguageViewModel();
        }

        public void ChangeLanguageFrench(object sender, RoutedEventArgs e)
        {
            _languageViewModel.ChangeLanguageFrench();
        }

        public void ChangeLanguageEnglish(object sender, RoutedEventArgs e)
        {
            _languageViewModel.ChangeLanguageEnglish();
        }
    }
}
