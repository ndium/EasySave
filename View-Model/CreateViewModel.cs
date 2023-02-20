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

        public void GetCreateModel(Config config)
        {
            try
            {
                
                CreateModel createModel = new CreateModel();
                var message = createModel.CreateSave(config);
            }
            catch (Exception ex)
            {
                ErrorView error = new ErrorView();
                error.ShowError(ex.Message);
               
            }
        }
    }
}
