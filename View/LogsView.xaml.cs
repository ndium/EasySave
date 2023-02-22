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
        static readonly SemaphoreSlim _semaphore = new System.Threading.SemaphoreSlim(1);
        public LogJsonModel logs { get; set; }

        public LogsView()
        {
            InitializeComponent();
            DataContext = this;
            logs = new LogJsonModel();
            Translation();


            try
            {
               
                List<LogJsonModel> list = logs.getListLog();

                // Utilisation du sémaphore pour protéger l'accès à l'instance de LogJsonModel
                _semaphore.Wait();

                LogGrid.ItemsSource = list;
            }
            catch
            {
                MessageBox.Show("Error logs not found", "Error Message", MessageBoxButton.OK);

            }
            finally
            {
                _semaphore.Release();
            }




        }
        public void Translation()
        {
            LangHelper langHelper = new LangHelper();
            SelectFile.Text = $"{langHelper._rm.GetString("SelectFile")}";
            Convert.Content = $"{langHelper._rm.GetString("Convert")}";
        }

        private void ConvertToXml_Checked(object sender, RoutedEventArgs e)
        {
            try { logs.ConvertLogs("xml"); }
            catch { MessageBox.Show("already exist", "Error Message", MessageBoxButton.OK); }
        }

        private void ConvertToJson_Checked(object sender, RoutedEventArgs e)
        {
            try { logs.ConvertLogs("json"); }
            catch { MessageBox.Show("already exist", "Error Message", MessageBoxButton.OK); }
        }



    }
}
