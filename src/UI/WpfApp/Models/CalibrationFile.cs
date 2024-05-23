using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfApp.Models
{
    public class CalibrationFile : INotifyPropertyChanged
    {
        private string path = string.Empty;

        public string Path
        {
            get { return path; }
            set
            {
                path = value;
                OnPropertyChanged("Path");
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
