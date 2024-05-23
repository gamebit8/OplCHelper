using Microsoft.Extensions.Configuration;

namespace ChecksumCorrector.ConsoleApp
{
    public class CustomCommandLineConfigurationSource : IConfigurationSource
    {
        private readonly string _key;
        private readonly string[] _args;

        public string FilePath { get; }
        public CustomCommandLineConfigurationSource(string key, string[] args)
        {
            _key = key;
            _args = args;
        }
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new CustomCommandLineConfigurationProvider(_key, _args);
        }
    }
}
