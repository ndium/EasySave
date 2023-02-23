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
using System.Xml;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.Xml.Linq;
using System.Threading;

namespace EasySaveV2.View
{
    /// <summary>
    /// Logique d'interaction pour LogsView.xaml
    /// </summary>
    public partial class LogsView : Page
    {
        //  static readonly SemaphoreSlim _semaphore = new System.Threading.SemaphoreSlim(1);
        public LogJsonModel Logs = new LogJsonModel();

        public LogsView()
        {
            InitializeComponent();
          
            DataContext = this;
            



            try
            {

                List<LogJsonModel> list = Logs.getListLog();

                // Utilisation du sémaphore pour protéger l'accès à l'instance de LogJsonModel


                LogGrid.ItemsSource = list;
            }
            catch
            {
                MessageBox.Show("Error logs not found", "Error Message", MessageBoxButton.OK);

            }





        }


        private void ConvertToXml_Checked(object sender, RoutedEventArgs e)
        {
            Logs.ConvertLogs("xml");
            Txtlog.Text = Logs.getListLogXML(); 


        }

        private void ConvertToJson_Checked(object sender, RoutedEventArgs e)
        {
      
      
            Txtlog.Text = Logs.getListLogJson(); 


        }




    }
}
