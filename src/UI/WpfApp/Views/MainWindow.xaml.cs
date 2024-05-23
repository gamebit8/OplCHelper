using ChecksumCorrector.Core;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Input;
using WpfApp.ViewModels;

namespace WpfApp
{
    public partial class MainWindow : Window
    {
        public ApplicationViewModel ApplicationVM;

        public MainWindow(IChecksumService checksumService)
        {
            InitializeComponent();
            ApplicationVM = new ApplicationViewModel(checksumService);
            DataContext = ApplicationVM;

            //AddDynamicResources();
            ShowWarningAboutUsingAlphaVersion();
        }

        //private void AddDynamicResources()
        //{
        //    var stringResources = GetStringResources();
        //    var styleResources = GetStyleResources();
        //    this.Resources.MergedDictionaries.Add(stringResources);
        //    this.Resources.MergedDictionaries.Add(styleResources);
        //}

        private static ResourceDictionary GetStringResources()
        {
            var dictionary = new ResourceDictionary
            {
                Source = new Uri(@"..\Resources\StringResources.ru.xaml", UriKind.Relative)
            };

            return dictionary;
        }

        private static ResourceDictionary GetStyleResources()
        {
            var dictionary = new ResourceDictionary
            {
                Source = new Uri(@"..\Resources\StyleResources.xaml", UriKind.Relative)
            };

            return dictionary;
        }

        private void ShowWarningAboutUsingAlphaVersion()
        {
            var warning = (string)this.TryFindResource("wawrning");
            var message = (string)this.TryFindResource("warningMessage");

            if (MessageBox.Show(message,
                    warning,
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question) == MessageBoxResult.No)
            {
                System.Windows.Application.Current.Shutdown();
            }
        }

        private void Correct_Сhecksum_File_Drop(object sender, DragEventArgs e)
        {
            if (!ApplicationVM.CorrectChecksumFiles.IsEmpty)
                return;

            var filePaths = GetColibrationFromDrop(e);


            ApplicationVM.AddPathsInCorrectСhecksumСalibrationFiles(filePaths);
        }

        private void Define_Checksum_File_Drop(object sender, DragEventArgs e)
        {
            if (!ApplicationVM.DefineTheChecksumAlgorithmFiles.IsEmpty)
                return;

            var filePaths = GetColibrationFromDrop(e);

            ApplicationVM.AddPathsInDefineTheСhecksumСalibrationFiles(filePaths);
        }

        private static string[] GetColibrationFromDrop(DragEventArgs e)
        {
            var filePaths = (string[])e.Data.GetData(DataFormats.FileDrop);

            return filePaths;
        }

        private void Correct_Сhecksum_Open_File(object sender, MouseButtonEventArgs e)
        {
            if (!ApplicationVM.CorrectChecksumFiles.IsEmpty)
                return;

            var filePaths = GetCalibrationFilePathsFromDialog();

            ApplicationVM.AddPathsInCorrectСhecksumСalibrationFiles(filePaths);
        }

        private void Define_Checksum_Open_File(object sender, MouseButtonEventArgs e)
        {
            if (!ApplicationVM.DefineTheChecksumAlgorithmFiles.IsEmpty)
                return;

            var filePaths = GetCalibrationFilePathsFromDialog();

            ApplicationVM.AddPathsInDefineTheСhecksumСalibrationFiles(filePaths);
        }

        private static string[] GetCalibrationFilePathsFromDialog()
        {
            OpenFileDialog openFileDialog = new()
            {
                Multiselect = true,
                Filter = "All files (*.*)|*.*|Bin (*.bin)|*.bin",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            };

            if (openFileDialog.ShowDialog() == true && openFileDialog.FileNames is string[] filePaths && filePaths.Length > 0)
                return filePaths;

            return [];
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.HeightChanged)
                ApplicationVM.WindowHeight = e.NewSize.Height;
        }
    }
}