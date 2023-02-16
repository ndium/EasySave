using EasySave.Model;
using EasySave.View_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EasySaveV2.View
{
    /// <summary>
    /// Logique d'interaction pour LoadingBar.xaml
    /// </summary>
    public partial class LoadingBar : Window
    {
        public LoadingBar()
        {
            InitializeComponent();
            DataContext = new CopyViewModel();
        }
    }
}
