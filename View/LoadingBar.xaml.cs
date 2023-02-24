

using EasySaveV2.Model;
using EasySaveV2.View_Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
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
        CopyViewModel _copyViewModel;
        public LoadingBar()
        {
            InitializeComponent();
            DataContext = this;
            _copyViewModel = new CopyViewModel();


        }
        public async void AddProgressBar()
        {
            Thread myThread = Thread.CurrentThread;
            ProgressBar progressBar;
            Label labelPercent;
            Label labelName;
            Button stopButton;
            Button pauseButton;
            StackPanel buttonStack;

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

                labelName = new Label();
                labelName.Height = 30;
                labelName.HorizontalAlignment = HorizontalAlignment.Left;
                labelName.VerticalAlignment = VerticalAlignment.Center;
                labelName.FontSize = 15;
                labelName.Margin = new Thickness(60, -40, 20, 0);
                labelName.Content = myThread.Name;
                labelName.Name = myThread.Name + "Title";
                labelName.Foreground = Brushes.White;
                labelName.MaxWidth = 50;
                MyStackPanel.Children.Add(labelName);

                labelPercent = new Label();
                labelPercent.Height = 30;
                labelPercent.HorizontalAlignment = HorizontalAlignment.Center;
                labelPercent.VerticalAlignment = VerticalAlignment.Center;
                labelPercent.FontSize = 18;
                labelPercent.Margin = new Thickness(20, -45, 20, 0);
                labelPercent.Name = $"{myThread.Name}label1";
                MyStackPanel.Children.Add(labelPercent);

                buttonStack = new StackPanel();
                buttonStack.Orientation = Orientation.Horizontal;
                buttonStack.HorizontalAlignment = HorizontalAlignment.Center;
                MyStackPanel.Children.Add(buttonStack);

                pauseButton = new Button();
                pauseButton.Height = 30;
                pauseButton.Width = 60;
                pauseButton.Content = $"Pause";
                pauseButton.Name = $"Pause{myThread.Name}";
                pauseButton.Click += PauseButton_Click;
                buttonStack.Children.Add(pauseButton);

                stopButton = new Button();
                stopButton.Height = 30;
                stopButton.Width = 60;
                stopButton.Content = $"Stop";
                stopButton.Name = $"Stop{myThread.Name}";
                stopButton.Click += StopButton_Click;
                buttonStack.Children.Add(stopButton);

            });

        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
           
            string ConfigName = button.Name.Replace("Stop", "");
            _copyViewModel.StopThread(ConfigName);


        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            string ConfigName = button.Name.Replace("Pause", "");
            switch (button.Content)
            {
                case "Pause":
                    _copyViewModel.PauseThread(ConfigName);
                    button.Content = "Resume";
                    break;
                case "Resume":
                    button.Content = "Pause";
                    _copyViewModel.PauseThread(ConfigName);
                    break;
            }

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
                else if ((child as FrameworkElement)?.Name == $"{e.UserState}label1")
                {
                    middle_label = child as Label;
                    middle_label.Content = $"{progressBar.Value}%";
                }
            }
        }
    }
}
