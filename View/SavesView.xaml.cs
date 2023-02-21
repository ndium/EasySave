

using EasySaveV2.Model;
using EasySaveV2.View_Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Logique d'interaction pour SavesView.xaml
    /// </summary>
    public partial class SavesView : Page
    {
        public BackgroundWorker worker { get; set; }
        public CopyViewModel _copyViewModel { get; set; }
        private List<Config> selectedConfigs { get; set; }
        private LoadingBar loadingBar { get; set; }
        public SavesView()
        {
            InitializeComponent();
            DataContext = this;
            _copyViewModel = new CopyViewModel();
            List<Config> list = _copyViewModel.GetConfigs();
            SaveGrid.ItemsSource = list;
            Translation();
        }

        public void Translation()
        {
            LangHelper langHelper = new LangHelper();
            AddButton.Content = $"{langHelper._rm.GetString("AddButton")}";
            EditButton.Content = $"{langHelper._rm.GetString("Editbutton")}";
            DeleteButton.Content = $"{langHelper._rm.GetString("DeleteButton")}";
            launchButton.Content = $"{langHelper._rm.GetString("Launch")}";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            worker = new BackgroundWorker();
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
            worker.WorkerReportsProgress= true;
            worker.WorkerSupportsCancellation= true;
            worker.DoWork += Worker_DoWork;
            worker.ProgressChanged += Worker_ProgressChanged;
            worker.RunWorkerAsync();
            
            selectedConfigs = new List<Config>();
            
            foreach(var item in SaveGrid.SelectedItems)
            {
                if (item is Config config) 
                { 
                    selectedConfigs.Add(config);
                }
            }
            loadingBar = new LoadingBar();
            loadingBar.Show();




        }

        private void Worker_ProgressChanged(object? sender, ProgressChangedEventArgs e)
        {
            loadingBar.copyProgressBar.Value = e.ProgressPercentage;
            loadingBar.numberProgressBar.Text = (string)e.UserState;
        }

        private void Worker_DoWork(object? sender, DoWorkEventArgs e)
        {
            try
            {
                _copyViewModel.GetCopyModel(selectedConfigs, worker);
                if (worker.CancellationPending == true)
                {
                    e.Cancel = true;
                    return;
                }
            }
            catch(Exception ex)
            {
                //On rejette l'erreur pour activer e.Error != null
                //C'EST NORMAL QUE CA BLOQUE AU DEBUGGING !!!
                // |
                // |
                // |
                // V
                throw ex;
            }
        }

        private void Worker_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
                loadingBar.Close();

            }
            else if (e.Cancelled)
            {
                MessageBox.Show("L'opération a été annulée.");
                loadingBar.Close();

            }
            else
            {
                loadingBar.copyProgressBar.Value = 100;
                MessageBox.Show("L'opération est terminée.");
                loadingBar.Close();
            }

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            AddSaveWindow addSaveWindow = new AddSaveWindow();
            addSaveWindow.ShowDialog();
            List<Config> list = _copyViewModel.GetConfigs();
            SaveGrid.ItemsSource = list;

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            List<Config> selectedConfigs = new List<Config>();

            foreach (var item in SaveGrid.SelectedItems)
            {
                if (item is Config config)
                {
                    selectedConfigs.Add(config);
                }
            }
            DeleteViewModel deleteViewModel= new DeleteViewModel();
            deleteViewModel.GetDeleteModel(selectedConfigs);
            List<Config> list = _copyViewModel.GetConfigs();
            SaveGrid.ItemsSource = list;
        }
    }
}
