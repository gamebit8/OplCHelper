using ChecksumCorrector.Core;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using WpfApp.Commands;
using WpfApp.Models;

namespace WpfApp.ViewModels
{
    public class ApplicationViewModel : INotifyPropertyChanged
    {
        private CalibrationFilesWithСhecksumStatus correctChecksumFiles = [];
        private CalibrationFilesWithChecksumAlgorithm defineTheChecksumAlgorithmFiles = [];
        private string title;
        private double maxHeightDataGrid = 250;
        private double windowHeight;
        private ChecksumAlgorithm selectedChecksumAlgorithm;
        private ICommand? setSelectedChecksumAlgorithmCommand;
        private ICommand? correctChecksumsCommand;
        private ICommand? clearCorrectСhecksumСalibrationFilesCommand;
        private ICommand? defineChecksumsCommand;
        private ICommand? clearDefineСhecksumСalibrationFilesCommand;
        private readonly IChecksumService _checksumService;

        public ApplicationViewModel(IChecksumService checksumService)
        {
            _checksumService = checksumService;
            title = GetTilte();
        }

        private static string GetTilte()
        {
            var appVersion = Assembly.GetExecutingAssembly().GetName().Version;
            return "OplСHelper " + appVersion;
        }

        public CalibrationFilesWithСhecksumStatus CorrectChecksumFiles
        {
            get { return correctChecksumFiles; }
            private set
            {
                correctChecksumFiles = value;
                OnPropertyChanged("CorrectChecksumFiles");
            }
        }

        public CalibrationFilesWithChecksumAlgorithm DefineTheChecksumAlgorithmFiles
        {
            get { return defineTheChecksumAlgorithmFiles; }
            private set
            {
                defineTheChecksumAlgorithmFiles = value;
                OnPropertyChanged("DefineTheChecksumAlgorithmFiles");
            }
        }

        public string Title
        {
            get => title;
            private set
            {
                title = value;
                OnPropertyChanged("Title");
            }
        }

        public double MaxHeightDataGrid
        {
            get { return maxHeightDataGrid; }
            private set
            {
                maxHeightDataGrid = value;
                OnPropertyChanged("MaxHeightDataGrid");
            }
        }

        public double WindowHeight
        {
            get => windowHeight;
            set
            {
                windowHeight = value;
                ChangeMaxHeightDataGridAfterWindowsResize(value);
                OnPropertyChanged("WindowHeight");
            }
        }

        private void ChangeMaxHeightDataGridAfterWindowsResize(double NewWindowHeight)
        {
            maxHeightDataGrid = NewWindowHeight - 200;
        }

        public ChecksumAlgorithm SelectedChecksumAlgorithm
        {
            get { return selectedChecksumAlgorithm; }
            set
            {
                selectedChecksumAlgorithm = value;
                OnPropertyChanged("SelectedChecksumAlgorithm");
            }
        }

        public ICommand SetSelectedChecksumAlgorithmCommand
        {
            get
            {
                return setSelectedChecksumAlgorithmCommand ??= new RelayCommand(obj =>
                {
                    selectedChecksumAlgorithm = (ChecksumAlgorithm)obj;
                });
            }
        }

        public ICommand CorrectChecksumsCommand
        {
            get
            {
                return correctChecksumsCommand ??= new RelayCommand(async obj =>
                {
                    if (CorrectChecksumFiles.Count > 0 && selectedChecksumAlgorithm != ChecksumAlgorithm.Unknown)
                    {
                        switch (SelectedChecksumAlgorithm)
                        {
                            case ChecksumAlgorithm.SumOfBigEndian16BitNot:
                                await CorrectCheksumFilesWithSumOfBigEndian16BitAlgorithmAsync(CorrectChecksumFiles);
                                break;
                            case ChecksumAlgorithm.SumOfBigEndian16BitNotPlusOne:
                                await CorrectCheksumFilesWithSumOfBigEndian16BitPlusOneAlgorithmAsync(CorrectChecksumFiles);
                                break;
                        }
                    }
                });
            }
        }

        private async Task CorrectCheksumFilesWithSumOfBigEndian16BitAlgorithmAsync(ObservableCollection<CalibrationFileWithСhecksumStatus> files)
        {
            foreach (var file in files)
                file.ChecksumStatus = await _checksumService.ChecksumIsCorrectAsync(file.Path, ChecksumAlgorithm.SumOfBigEndian16BitNot, true);
        }

        private async Task CorrectCheksumFilesWithSumOfBigEndian16BitPlusOneAlgorithmAsync(ObservableCollection<CalibrationFileWithСhecksumStatus> files)
        {
            foreach (var file in files)
                file.ChecksumStatus = await _checksumService.ChecksumIsCorrectAsync(file.Path, ChecksumAlgorithm.SumOfBigEndian16BitNotPlusOne, true);
        }

        public ICommand ClearCorrectСhecksumСalibrationFilesCommand
        {
            get
            {
                return clearCorrectСhecksumСalibrationFilesCommand ??= new RelayCommand(obj =>
                {
                    CorrectChecksumFiles.Clear();
                });
            }
        }

        public ICommand DefineChecksumsCommand
        {
            get
            {
                return defineChecksumsCommand ??= new RelayCommand(async obj =>
                {
                    if (DefineTheChecksumAlgorithmFiles.Count > 0)
                    {
                        foreach (var file in DefineTheChecksumAlgorithmFiles)
                        {
                            file.ChecksumAlgorithm = await _checksumService.GetChecksumAlgorithmFromOriginalFileAsync(file.Path);
                        }
                    }
                });
            }
        }

        public ICommand ClearDefineСhecksumСalibrationFilesCommand
        {
            get
            {
                return clearDefineСhecksumСalibrationFilesCommand ??= new RelayCommand(obj =>
                {
                    DefineTheChecksumAlgorithmFiles.Clear();
                });
            }
        }

        public void AddPathsInCorrectСhecksumСalibrationFiles(string[] paths)
        {
            foreach (var path in paths)
            {
                CorrectChecksumFiles.Add(new CalibrationFileWithСhecksumStatus { Path = path });
            }
        }

        public void AddPathsInDefineTheСhecksumСalibrationFiles(string[] paths)
        {
            foreach (var path in paths)
            {
                DefineTheChecksumAlgorithmFiles.Add(new CalibrationFileWithChecksumAlgorithm { Path = path });
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = " ")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
