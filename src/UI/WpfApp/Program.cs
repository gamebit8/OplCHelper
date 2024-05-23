using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WpfApp
{
    public class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var configuration = CreateConfiguration(args);
            var services = CreateServices(configuration);
            var app = services.GetRequiredService<App>();
            app.Run();
        }

        static ServiceProvider CreateServices(IConfiguration configuration)
        {
            var services = new ServiceCollection();
            var startup = new Startup(configuration);
            startup.ConfigurationServices(services);

            return services.BuildServiceProvider();
        }

        static IConfigurationRoot CreateConfiguration(string[] args)
        {
            return new ConfigurationBuilder()
                .AddJsonFile("appSettings.json", true, false)
                .AddInMemoryCollection()
                .Build();
        }
    }
}
