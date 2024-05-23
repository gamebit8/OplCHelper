using Microsoft.Extensions.Configuration;

namespace ChecksumCorrector.ConsoleApp
{
    internal class CustomCommandLineConfigurationProvider : ConfigurationProvider
    {
        private readonly string[] _args;
        private readonly string _key;
        public CustomCommandLineConfigurationProvider(string key, string[] args)
        {
            _args = args;
            _key = key;
        }

        public override void Load()
        {
            var data = new Dictionary<string, string>
            {
                { _key, _key }
            };

            for (int i = 0; i < _args.Length; i++)
            {
                data.Add($"{_key}:{i}", _args[i]);
            }

            Data = data;
        }
    }
}
