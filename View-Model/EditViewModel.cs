using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasySaveV2.Model;
using EasySaveV2.View;

namespace EasySaveV2.View_Model
{
    public class EditViewModel
    {
        private EditModel editModel = new EditModel();
        public EditViewModel()
        {
        }

        public void GetEditModel(string newBackupName, string newSourceDirectory, string newTargetDirectory, string newBackupType, int configToModify)
        {
            //var editModel = new EditModel();
            /*var message = editModel.EditSave(newBackupName, newSourceDirectory, newTargetDirectory, newBackupType, configToModify);
            if (message != null)
            {
                var error = new ErrorView();
                error.ShowError(message);
                GetEditModel(newBackupName, newSourceDirectory, newTargetDirectory, newBackupType, configToModify);
            }*/
        }

        public void GetListConfig()
        {
            /*var message = editModel.GetListConfigToModify();
            if (message != null)
            {
                var error = new ErrorView();
                error.ShowError(message);
            }*/
        }
    }
}
