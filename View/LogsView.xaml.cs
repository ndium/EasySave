using EasySaveV2.Model;
using EasySaveV2.View_Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.IO;
using Newtonsoft.Json;
using Path = System.IO.Path;
using System.Reflection;
using EasySaveV2.View_Model;

namespace EasySaveV2.View
{
    /// <summary>
    /// Logique d'interaction pour LogsView.xaml
    /// </summary>
    public partial class LogsView : Page
    {

        public LogsView()
        {
            InitializeComponent();
            Translation();

            var logs = new LogViewModel();
            /*List<Config> list = LogJson.log
            SaveGrid.ItemsSource = list;*/
            DataContext = this;
            try
            {


            }
            catch
            {
                MessageBox.Show("Error logs not found", "Error Message", MessageBoxButton.OK);

            }




        }
        public void Translation()
        {
            LangHelper langHelper = new LangHelper();
            SelectFile.Text = $"{langHelper._rm.GetString("SelectFile")}";
            Convert.Content = $"{langHelper._rm.GetString("Convert")}";
        }



        public void ConvertButton_Click(object sender, RoutedEventArgs e)
        {
            //string filePath = "path/to/file";
            //bool useXml = xmlRadio.IsChecked ?? false;

            //if (useXml)
            //{
            //    // convert file to XML
            //}
            //else
            //{
            //    // convert file to JSON
            //}
        }

    }
}
