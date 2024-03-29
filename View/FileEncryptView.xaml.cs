﻿using System;
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
using EasySaveV2.Model;
using EasySaveV2.View_Model;

namespace EasySaveV2.View
{
    /// <summary>
    /// Logique d'interaction pour FileEncryptView.xaml
    /// </summary>
    public partial class FileEncryptView : Page
    {
        public FileEncryptViewModel _fileEncryptViewModel { get; set; }
        public List<Extension> list;
        public FileEncryptView()
        {
            InitializeComponent();
            DataContext = this;
            _fileEncryptViewModel = new FileEncryptViewModel();
            refresh();
            Translation();
        }
        public void Translation()
        {
            LangHelper langHelper = new LangHelper();
            Extension.Text = $"{langHelper._rm.GetString("Extension")}";
            AddExtension.Content = $"{langHelper._rm.GetString("AddButton")}";
            DeleteExtension.Content = $"{langHelper._rm.GetString("DeleteButton")}";
        }

        //Button ajouter
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string extensionName = InputExt.Text;
            _fileEncryptViewModel.addExtension(extensionName);
            InputExt.Text = "";
            refresh();
        }

        //Bouton supprimer
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //Pour pouvoir supprimer en selectionnant
            foreach (Extension item in ExtendGrid.SelectedItems)
            {
                _fileEncryptViewModel.removeExtension(item.Name);
            }

            //Supprimer grace au input
            string extensionName = InputExt.Text;
            _fileEncryptViewModel.removeExtension(extensionName);
            InputExt.Text = "";
            refresh();
        }

        //Refresh la page
        private void refresh()
        {
            list = _fileEncryptViewModel.GetList();
            ExtendGrid.ItemsSource = list;
        }
    }
}
