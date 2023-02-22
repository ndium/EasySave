using EasySaveV2.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySaveV2.View_Model
{
    class LogViewModel
    {
        LogJsonModel logJson = new LogJsonModel();
       

        public List<LogJsonModel> getListLog()
        {
            
            return logJson.getListLog();
        }
    }
}
