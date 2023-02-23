

using EasySaveV2.Model;
using EasySaveV2.View_Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
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
        public CopyViewModel _copyViewModel { get; set; }
        private LoadingBar _loadingBar { get; set; }
        public SavesView()
        {
            InitializeComponent();
            DataContext = _loadingBar;
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

            _loadingBar = new LoadingBar();
            _loadingBar.Show();
            _loadingBar.Activate();
            _loadingBar.Focus();


            foreach (var item in SaveGrid.SelectedItems)
            {
                if (item is Config config)
                {
                    Dispatcher.BeginInvoke(() =>
                    {
                        Thread thread = new Thread(_loadingBar.AddProgressBar);
                        thread.Name = config.BackupName;
                        thread.Start();
                    });

                    BackgroundWorker worker = new BackgroundWorker();
                    worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
                    worker.WorkerReportsProgress = true;
                    worker.WorkerSupportsCancellation = true;
                    worker.DoWork += Worker_DoWork;
                    worker.ProgressChanged += Worker_ProgressChanged;
                    worker.RunWorkerAsync(argument: config);

                }
            }


        }

        private void Worker_ProgressChanged(object? sender, ProgressChangedEventArgs e)
        {
            
            _loadingBar.UpdateProgressBar(e);
        }

        private void Worker_DoWork(object? sender, DoWorkEventArgs e)
        {
            try
            {
                var config = e.Argument as Config;
                var localdowork = sender as BackgroundWorker;
                _copyViewModel.GetCopyModel(config, localdowork);
                if (localdowork.CancellationPending == true)
                {
                    e.Cancel = true;
                    return;
                }
            }
            catch (Exception ex)
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
                //MessageBox.Show(e.Error.Message);

            }
            else if (e.Cancelled)
            {
                //MessageBox.Show("L'opération a été annulée.");


            }
            else
            {
                
                //MessageBox.Show("L'opération est terminée.");
            }

        }

        private  void Button_Click_1(object sender, RoutedEventArgs e)
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
            DeleteViewModel deleteViewModel = new DeleteViewModel();
            deleteViewModel.GetDeleteModel(selectedConfigs);
            List<Config> list = _copyViewModel.GetConfigs();
            SaveGrid.ItemsSource = list;
        }
    }
}
