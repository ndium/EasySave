﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasySaveV2.Model;
using EasySaveV2.View;

namespace EasySaveV2.View_Model
{
    public class CreateViewModel
    {
        public CreateViewModel()
        {
        }

        public void GetCreateModel(Config config)
        {
            try
            {
                
                CreateModel createModel = new CreateModel();
                 createModel.CreateSave(config);
            }
            catch (Exception ex)
            {
                ErrorView error = new ErrorView();
                error.ShowError(ex.Message);
               
            }
        }
    }
}
