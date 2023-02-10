using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasySave.Model;
using EasySave.View;

namespace EasySave.View_Model
{
    public class CreateViewModel
    {
        public CreateViewModel()
        {
        }

        public void GetCreateModel(string backupName, string sourceDirectory, string targetDirectory, string backupType)
        {
            try
            {
                CreateModel createModel = new CreateModel();
                var message = createModel.CreateSave(backupName, sourceDirectory, targetDirectory, backupType);
            }
            catch (Exception ex)
            {
                ErrorView error = new ErrorView();
                error.ShowError(ex.Message);
               
            }
        }
    }
}
