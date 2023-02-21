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
using System.Windows.Threading;

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
        }

       


        public void GetCopyModel(List<Config> selectedWorks, object sender)
        {
            
                foreach (var config in selectedWorks)
                {


                    if ((SaveType)Enum.Parse(typeof(SaveType), config.BackupType.ToString()) == SaveType.Complete)
                    {
                        _copyModel.FullCopy(config, sender);
                    }
                    else if ((SaveType)Enum.Parse(typeof(SaveType), config.BackupType.ToString()) == SaveType.Differential)
                    {
                        _copyModel.DifferentialCopy(config, sender);
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
