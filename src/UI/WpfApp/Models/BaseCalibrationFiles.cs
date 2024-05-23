using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfApp.Models
{
    public class BaseCalibrationFiles<T> : ObservableCollection<T>, INotifyPropertyChanged
    {
        private bool isEmpty = true;
        public BaseCalibrationFiles() : base()
        {
            this.CollectionChanged += this.CalibrationFilesWithСhecksumStatus_CollectionChanged;
        }

        public bool IsEmpty
        {
            get => isEmpty;
            private set
            {
                isEmpty = value;
                OnPropertyChanged("IsEmpty");
            }
        }

        public new event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        private void CalibrationFilesWithСhecksumStatus_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    IsEmpty = false;
                    break;
                case NotifyCollectionChangedAction.Remove:
                    if (this.Count == 0)
                        IsEmpty = true;
                    break;
                case NotifyCollectionChangedAction.Reset:
                    IsEmpty = true;
                    break;
            }
        }
    }
}
