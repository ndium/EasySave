using EasySave.Model;
using EasySave.View;
using EasySaveV2.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;


namespace EasySave.View_Model
{
    public class CopyViewModel
    {
        string _backupConfigFile;
        string _file;
        public CopyModel _copyModel { get; set; }
        public CopyViewModel()
        {
            string backupConfigFile = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Easysave";
            string file = Path.Combine(backupConfigFile, "config.json");
            _backupConfigFile = backupConfigFile;
            _file = file;
            _copyModel = new CopyModel();
            _copyModel.ProgressChanged += _copyModel_ProgressChanged;
        }
        public double Progress { get; set; }

        private void _copyModel_ProgressChanged(object sender, EventArgs e)
        {
            Progress = _copyModel.Progress;
        }


        public void GetCopyModel(List<Config> selectedWorks)
        {
            var i =0;
            var progressBar = new LoadingBar();

            foreach (var config in selectedWorks)
            {
                i++;
                if (Thread.CurrentThread.GetApartmentState() != ApartmentState.STA)
                {
                    Thread thread = new Thread(() =>
                    {
                        var progressBar = new LoadingBar();
                        Application.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            progressBar.copyProgressBar.Value = Progress;
                            progressBar.numberProgressBar.Text = $"{i}/{selectedWorks.Count}";
                        }));
                        progressBar.Show();
                        Dispatcher.Run();
                    });
                    thread.SetApartmentState(ApartmentState.STA);
                    thread.Start();
                    Thread.Sleep(500);
                }
                else
                {
                    progressBar.copyProgressBar.Value = Progress;
                    progressBar.numberProgressBar.Text = $"{i}/{selectedWorks.Count}";
                    progressBar.Show();
                    Dispatcher.Run();
                }

                if ((SaveType)Enum.Parse(typeof(SaveType), config.BackupType.ToString()) == SaveType.Complete)
                {
                    _copyModel.FullCopy(config);
                }
                else if ((SaveType)Enum.Parse(typeof(SaveType), config.BackupType.ToString()) == SaveType.Differential)
                {
                    _copyModel.DifferentialCopy(config);
                }
            }
        }

      
        public Config GetConfigInfo(int index)
        {
            var jsonModel = new LogJsonModel();
            Config obj = jsonModel.ReadJsonConfig(_file, index);
            return obj;

        }
        public List<Config> GetConfigs()
        {

            var jsonModel = new LogJsonModel();
            List<Config> obj = jsonModel.GetConfigFile(_file);
            return obj;
        }
    }
}
