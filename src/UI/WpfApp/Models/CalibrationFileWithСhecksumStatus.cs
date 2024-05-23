using ChecksumCorrector.Core;

namespace WpfApp.Models
{
    public class CalibrationFileWithСhecksumStatus : CalibrationFile
    {
        private Checksum? checksumStatus = null;

        public Checksum? ChecksumStatus
        {
            get { return checksumStatus; }
            set
            {
                checksumStatus = value;
                OnPropertyChanged("ChecksumStatus");
            }
        }
    }
}
