using EasySaveV2.View;
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
using System.IO;
using Newtonsoft.Json;
using EasySaveV2.Model;


namespace EasySaveV2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Translation();
        }

        public void Translation()
        {
            LangHelper langHelper = new LangHelper();
            Mysaves.Text = $"{langHelper._rm.GetString("FirstOption")}";
            Logs.Text = $"{langHelper._rm.GetString("SecondOption")}";
            Encryption.Text = $"{langHelper._rm.GetString("ThirdOption")}";
            Filters.Text = $"{langHelper._rm.GetString("FourthOption")}";
            Language.Text = $"{langHelper._rm.GetString("FifthOption")}";
            Exit.Text = $"{langHelper._rm.GetString("SixthOption")}";
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            // Begin dragging the window
            this.DragMove();
        }
        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        //bouton sauvegarde
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MySaves.Content = new SavesView();
        }

        //bouton log
        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            MySaves.Content = new LogsView();
        }

        //bouton close
        private void MenuItem_Click_Close(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MySaves_Navigated(object sender, NavigationEventArgs e)
        {
            
        }

        //bouton parametre chiffrement
        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            MySaves.Content = new FileEncryptView();
        }

        private void NewApplicationMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MySaves.Content = new FiltersView();
        }

        private void MenuItem_Click_4(object sender, RoutedEventArgs e)
        {
            MySaves.Content = new LanguageView();
        }
    }
}
