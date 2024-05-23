using ChecksumCorrector.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace WpfApp
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        public Startup(IConfiguration configuration) => _configuration = configuration;

        public void ConfigurationServices(IServiceCollection services)
        {
            services.AddScoped<IChecksumService, ChecksumService>()
                .AddScoped<ILoggerFactory, LoggerFactory>()
                .AddScoped(typeof(ILogger<>), typeof(Logger<>))
                .AddScoped<App>()
                .AddScoped<MainWindow>();
        }
    }
}
