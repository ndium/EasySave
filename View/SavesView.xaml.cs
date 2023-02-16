using EasySave.Model;
using EasySave.View_Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace EasySaveV2.View
{
    /// <summary>
    /// Logique d'interaction pour SavesView.xaml
    /// </summary>
    public partial class SavesView : Page
    {
        public CopyViewModel _copyViewModel { get; set; }
        public SavesView()
        {
            InitializeComponent();
            DataContext = this;
            _copyViewModel = new CopyViewModel();
            List<Config> list = _copyViewModel.GetConfigs();
            SaveGrid.ItemsSource = list;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            List<Config> selectedConfigs = new List<Config>();
            
            foreach(var item in SaveGrid.SelectedItems)
            {
                if (item is Config config) 
                { 
                    selectedConfigs.Add(config);
                }
            }
            
            _copyViewModel.GetCopyModel(selectedConfigs);




        }
    }
}
