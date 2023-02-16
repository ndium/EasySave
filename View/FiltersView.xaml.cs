using EasySaveV2.Model;
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

namespace EasySaveV2.View
{
    /// <summary>
    /// Logique d'interaction pour FiltersView.xaml
    /// </summary>
    public partial class FiltersView : Page
    {
        public FiltersView()
        {
            InitializeComponent();
        }

        private void OnSaveApp(object sender, RoutedEventArgs e)
        {
            string appName = txtAppName.Text;
            try
            {
                var businessAppModel = new BusinessAppModel();
                businessAppModel.SaveApp(appName);
                MessageBox.Show("Application sauvegardée avec succès.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la sauvegarde de l'application : " + ex.Message);
            }
        }
    }
}
