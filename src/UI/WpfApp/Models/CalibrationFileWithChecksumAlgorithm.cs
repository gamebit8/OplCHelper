using ChecksumCorrector.Core;

namespace WpfApp.Models
{
    public class CalibrationFileWithChecksumAlgorithm : CalibrationFile
    {
        private ChecksumAlgorithm? checksumAlgorithm = null;

        public ChecksumAlgorithm? ChecksumAlgorithm
        {
            get { return checksumAlgorithm; }
            set
            {
                checksumAlgorithm = value;
                OnPropertyChanged("ChecksumAlgorithm");
            }
        }
    }
}
