using Microsoft.Extensions.Configuration;

namespace ChecksumCorrector.ConsoleApp
{
    public static class CustomCommandLineConfigurationExtensions
    {
        public static IConfigurationBuilder AddCustomCommandLine(this IConfigurationBuilder builder, string key, string[] args)
        {
            var provider = new CustomCommandLineConfigurationSource(key, args);
            builder.Add(provider);

            return builder;
        }
    }
}
