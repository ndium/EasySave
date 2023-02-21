﻿using EasySaveV2.Model;
using EasySaveV2.View;
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

namespace EasySaveV2.View_Model
{
    public class CopyViewModel
    {
        string LocalPath;
        public CopyModel _copyModel { get; set; }
        public CopyViewModel()
        {
             
            LocalPath = Global.JSON_PATH;
            _copyModel = new CopyModel();
            _copyModel.ProgressChanged += _copyModel_ProgressChanged;
        }
        static double Progress { get; set; }

        private void _copyModel_ProgressChanged(object sender, int e)
        {
            Progress = e;
            

        }


        public async Task GetCopyModel(List<Config> selectedWorks)
        {
            var i = 0;

            LoadingBar progressBar = new LoadingBar();
            progressBar.Show();
            if (progressBar.Dispatcher.CheckAccess())
            {
                progressBar.copyProgressBar.Value = Progress;
            }
            else
            {
                await progressBar.Dispatcher.BeginInvoke(new Action(() =>
                {
                    progressBar.copyProgressBar.Value = Progress;
                }));
            }









            foreach (var config in selectedWorks)
            {
                i++;


                if ((SaveType)Enum.Parse(typeof(SaveType), config.BackupType.ToString()) == SaveType.Complete)
                {
                    await _copyModel.FullCopy(config);
                }
                else if ((SaveType)Enum.Parse(typeof(SaveType), config.BackupType.ToString()) == SaveType.Differential)
                {
                    await _copyModel.DifferentialCopy(config);
                }
            }
        }


        public Config GetConfigInfo(int index)
        {
            var jsonModel = new LogJsonModel();
            Config obj = jsonModel.ReadJsonConfig(LocalPath, index);
            return obj;

        }
        public List<Config> GetConfigs()
        {

            var jsonModel = new LogJsonModel();
            List<Config> obj = jsonModel.GetConfigFile(LocalPath);
            return obj;
        }
    }
}
