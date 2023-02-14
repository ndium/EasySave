using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave.Model
{
    public class Config : INotifyPropertyChanged
    {
        public string BackupName { get; set; }
        public string SourceDirectory { get; set; }
        public string TargetDirectory { get; set; }
        public string BackupType { get; set; }

        public string Name
        {
            get { return BackupName; }
            set
            {
                BackupName = value;
                OnPropertyChanged("Name");
            }
        }

        public string Source
        {
            get { return SourceDirectory; }
            set
            {
                SourceDirectory = value;
                OnPropertyChanged("Source");
            }
        }
        public string Target
        {
            get { return TargetDirectory; }
            set
            {
                TargetDirectory = value;
                OnPropertyChanged("Target");
            }
        }
        public string Type
        {
            get { return BackupType; }
            set
            {
                BackupType = value;
                OnPropertyChanged("Type");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
