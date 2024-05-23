using ChecksumCorrector.Core;
using Microsoft.Extensions.Options;

namespace ChecksumCorrector.ConsoleApp
{
    public class App : IApp
    {
        private readonly IChecksumService _checksumService;
        private readonly IOptions<AppSettings> _settings;

        public App(IChecksumService checksumService, IOptions<AppSettings> settings)
        {
            _checksumService = checksumService;
            _settings = settings;
        }

        public async Task RunAsync()
        {
            var paths = GetPathsFromConfiguration(_settings?.Value);

            while (!Environment.HasShutdownStarted)
            {
                foreach (string path in paths)
                {
                    if (!File.Exists(path))
                    {
                        Console.WriteLine($"Файла {path} не существует");
                        break;
                    }

                    var result = await _checksumService.ChecksumIsCorrectAsync(path, true);
                    var message = result switch
                    {
                        Checksum.Valid => $"{path}: чексумма верна",
                        Checksum.Invalid => $"{path}: чексумма неверна",
                        Checksum.Сhanged => $"{path}: чексумма изменена",
                        _ => $"{path}: не является файлом калибровкой"
                    };

                    Console.WriteLine(message);
                }

                paths = TryGetPathsFilesFromConsole();
            }
        }

        private IEnumerable<string> GetPathsFromConfiguration(AppSettings settings)
        {
            List<string> paths = new();
            foreach (var path in settings?.PathFiles)
            {
                paths.Add(path);
            }

            return paths;
        }

        private static IEnumerable<string> TryGetPathsFilesFromConsole()
        {
            Console.WriteLine("Укажите пути к файлам через запятую");
            var paths = new List<string>();

            try
            {
                var stringSeparators = new string[] {",  ", ", ", "," };
                paths = Console.ReadLine().Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries).ToList();
            }
            catch
            {
                Console.WriteLine("Неверно указаны пути");
            }

            return paths;
        }
    }
}
