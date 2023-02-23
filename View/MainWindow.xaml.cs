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
using System.Threading;
using System.Diagnostics.Metrics;
using System.ComponentModel;

namespace EasySaveV2.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private Mutex mutex;
        public event PropertyChangedEventHandler PropertyChanged;

        private string mySavesMenu;
        public string MySavesMenu
        {
            get { return mySavesMenu; }
            set
            {
                mySavesMenu = value;
                RaisePropertyChanged("MySaves");
            }
        }
        private string logsMenu;
        public string LogsMenu
        {
            get { return logsMenu; }
            set
            {
                logsMenu = value;
                RaisePropertyChanged("Logs");
            }
        }
        private string encryptionMenu;
        public string EncryptionMenu
        {
            get { return encryptionMenu; }
            set
            {
                encryptionMenu = value;
                RaisePropertyChanged("Encryption");
            }
        }
        private string filtersMenu;
        public string FilterssMenu
        {
            get { return filtersMenu; }
            set
            {
                filtersMenu = value;
                RaisePropertyChanged("Filters");
            }
        }
        private string languageMenu;
        public string LanguageMenu
        {
            get { return languageMenu; }
            set
            {
                languageMenu = value;
                RaisePropertyChanged("Language");
            }
        }
        private string exitMenu;
        public string ExitMenu
        {
            get { return exitMenu; }
            set
            {
                exitMenu = value;
                RaisePropertyChanged("Exit");
            }
        }


        public MainWindow()
        {
            InitializeComponent();
            Translation();

            bool createdNew;
            mutex = new Mutex(true, "NomUniqueDeVotreMutex", out createdNew);

            if (!createdNew)
            {
                // une instance de l'application est déjà en cours d'exécution
                // vous pouvez par exemple afficher un message d'erreur et fermer l'application
                MessageBox.Show("L'application est déjà en cours d'exécution.");
                Application.Current.Shutdown();
            }
            Server server = new Server();
        }
        private void RaisePropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
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
            WelcomeEasySave.Text = $"{langHelper._rm.GetString("Welcome")}";
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
            WelcomeEasySave.Visibility = Visibility.Collapsed;
        }

        //bouton log
        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            MySaves.Content = new LogsView();
            WelcomeEasySave.Visibility = Visibility.Collapsed;
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
            WelcomeEasySave.Visibility = Visibility.Collapsed;
        }

        private void NewApplicationMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MySaves.Content = new FiltersView();
            WelcomeEasySave.Visibility = Visibility.Collapsed;
        }

        private void MenuItem_Click_4(object sender, RoutedEventArgs e)
        {
            MySaves.Content = new LanguageView(this);
            WelcomeEasySave.Visibility = Visibility.Collapsed;
        }
        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            mutex.ReleaseMutex();
        }
    }
}
