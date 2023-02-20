using EasySave.Model;
using EasySave.View_Model;
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
using System.Windows.Shapes;

namespace EasySaveV2.View
{
    /// <summary>
    /// Logique d'interaction pour LoadingBar.xaml
    /// </summary>
    public partial class LoadingBar : Window
    {
        public LoadingBar()
        {
            InitializeComponent();
            DataContext = new CopyViewModel();
            //BackgroundWorker worker = new BackgroundWorker();
            //worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
            //worker.WorkerReportsProgress = true;
            //worker.DoWork += Worker_DoWork;
            //worker.ProgressChanged += Worker_ProgressChanged;
            //worker.RunWorkerAsync();
        }

        //private void Worker_ProgressChanged(object? sender, ProgressChangedEventArgs e)
        //{
        //    copyProgressBar.Value = e.ProgressPercentage;
        //    numberProgressBar.Text = (string)e.UserState;

        //}

        //private void Worker_DoWork(object? sender, DoWorkEventArgs e)
        //{
        //    var worker = sender as BackgroundWorker;
        //    worker.ReportProgress(0, )
        //}

        //private void Worker_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
