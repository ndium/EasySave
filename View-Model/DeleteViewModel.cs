using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasySaveV2.Model;
using EasySaveV2.View;

namespace EasySaveV2.View_Model
{
    public class DeleteViewModel
    {
        public DeleteModel deleteModel;
        public DeleteViewModel()
        {
            deleteModel = new DeleteModel();
        }

        public List<string> GetListConfig()
        {
            return deleteModel.GetListConfig();
        }

        public string SetNumConfigModel(int numConfig)
        {
            try
            {
                var message = deleteModel.DeleteSave(numConfig);
                return message;
            }
            catch (Exception ex)
            {
                var error = new ErrorView();
                error.ShowError(ex.Message);
                return null;
            }

        }
    }


}
