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
using System.Windows.Navigation;
using System.Windows.Shapes;
using EasySave.Model;
using EasySave.View_Model;
using EasySaveV2.View_Model;

namespace EasySaveV2.View
{
    /// <summary>
    /// Logique d'interaction pour FileEncryptView.xaml
    /// </summary>
    public partial class FileEncryptView : Page
    {
        public FileEncryptViewModel _fileEncryptViewModel { get; set; }
        public FileEncryptView()
        {
            InitializeComponent();
            DataContext = this;
            List<Extension> list = _fileEncryptViewModel.GetList();
            ExtendGrid.ItemsSource = list;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
