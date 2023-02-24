using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    public static class Global
    {
    //ON MET UNE VARIABLE ACCESSIBLE DANS TOUT LE PROJET AFIN D'EVITER LA REDONDANCE DE SA DECLARATION
        public static readonly string JSON_PATH = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\EasySaveV2";
    }
