using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySaveV2.Model
{
    public enum SaveType
    {
        Complete = 1,
        Differential = 2
    }

    public enum Lang
    {

        [Description("English")]
        en,

        [Description("French")]
        fr

    }
}
