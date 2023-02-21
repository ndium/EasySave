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
            Translation();

        }

        public void Translation()
        {
            LangHelper langHelper = new LangHelper();
            Name.Text = $"{langHelper._rm.GetString("Name")}\n";
            SourcePath.Text = $"{langHelper._rm.GetString("sourceDirectory")}\n";
            DestinationPath.Text = $"{langHelper._rm.GetString("targetDirectory")}\n";
            backuptype.Text = $"{langHelper._rm.GetString("backuptype")} \n";
            SaveButton.Content = $"{langHelper._rm.GetString("SaveAppButton")}";
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
            this.Close();
        }
    }
}