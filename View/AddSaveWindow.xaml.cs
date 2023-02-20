using EasySaveV2.Model;
using EasySaveV2.View_Model;
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
    /// Logique d'interaction pour AddSaveWindow.xaml
    /// </summary>
    public partial class AddSaveWindow : Window
    {
        public AddSaveWindow()
        {
            InitializeComponent();
            DataContext = this;

        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Config config = new Config();
            CreateViewModel createViewModel = new CreateViewModel();
            config.BackupName = SaveName.Text;
            config.SourceDirectory = Source.Text;
            config.TargetDirectory = Destination.Text;
            config.BackupType = (SaveType)Enum.Parse(typeof(SaveType), SaveType.Text);
            createViewModel.GetCreateModel(config);
        }
    }
}