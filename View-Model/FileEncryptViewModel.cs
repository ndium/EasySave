using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasySave.Model;
using EasySave.View_Model;
using EasySaveV2.Model;

namespace EasySaveV2.View_Model
{
    public class FileEncryptViewModel
    {
        public FileEncryptModel _fileEncryptModel { get; set; }
        public FileEncryptViewModel()
        {
            _fileEncryptModel = new FileEncryptModel();
        }

        public List<Extension> GetList()
        {
            return _fileEncryptModel.GetList();
        }
    }
}
