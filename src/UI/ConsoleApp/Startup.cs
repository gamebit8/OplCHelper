using ChecksumCorrector.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ChecksumCorrector.ConsoleApp
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        public Startup(IConfiguration configuration) => _configuration = configuration;

        public void ConfigurationServices(IServiceCollection services)
        {
            services.Configure<AppSettings>(_configuration.GetSection(nameof(AppSettings)));

            services.AddScoped<IChecksumService, ChecksumService>()
                .AddScoped<ILoggerFactory, LoggerFactory>()
                .AddScoped(typeof(ILogger<>), typeof(Logger<>))
                .AddScoped<IApp, App>();
        }
    }
}
