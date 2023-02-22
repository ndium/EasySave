

using EasySaveV2.View_Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
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
            DataContext = this;


        }
        public async void AddProgressBar()
        {
            Thread myThread = Thread.CurrentThread;
            ProgressBar progressBar;
            Label middle_label;


            Dispatcher.Invoke(() =>
            {
                progressBar = new ProgressBar();
                progressBar.Minimum = 0;
                progressBar.Maximum = 100;
                progressBar.Width = 500;
                progressBar.Height = 20;
                progressBar.Margin = new Thickness(10, 10, 10, 10);
                progressBar.Name = myThread.Name;
                MyStackPanel.Children.Add(progressBar);

                middle_label = new Label();
                middle_label.Height = 30;
                middle_label.HorizontalAlignment = HorizontalAlignment.Center;
                middle_label.VerticalAlignment = VerticalAlignment.Center;
                middle_label.FontSize = 18;
                middle_label.Margin = new Thickness(20, -45, 20, 0);
                middle_label.Name = $"{myThread.Name}label1";
                MyStackPanel.Children.Add(middle_label);
            });

        }

        public void UpdateProgressBar(ProgressChangedEventArgs e)
        {
            ProgressBar progressBar = null;
            Label middle_label = null;

            foreach (var child in MyStackPanel.Children)
            {
                if ((child as FrameworkElement)?.Name == e.UserState.ToString())
                {
                    progressBar = child as ProgressBar;
                    progressBar.Value = e.ProgressPercentage;
                    

                    
                }
                else if((child as FrameworkElement)?.Name == $"{e.UserState}label1")
                {
                    middle_label = child as Label;
                    middle_label.Content = $"{progressBar.Value}%";
                }
            }
        }
    }
}
