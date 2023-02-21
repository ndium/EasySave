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
        public LogJsonModel logJsonModel;
        private string LocalPath;
        public DeleteViewModel()
        {
            logJsonModel= new LogJsonModel();
            deleteModel = new DeleteModel();
            LocalPath = Global.JSON_PATH;
        }


        public List<Config> GetListConfig()
        {
            return logJsonModel.GetConfigFile(LocalPath);
        }

        public void GetDeleteModel(List<Config> configs)
        {
            try
            {
                 deleteModel.DeleteSave(configs);
            }
            catch (Exception ex)
            {
                var error = new ErrorView();
                error.ShowError(ex.Message);
            }

        }
    }


}
