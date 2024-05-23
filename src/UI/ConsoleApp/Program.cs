using ChecksumCorrector.ConsoleApp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var configuration = CreateConfiguration(args);
var services = CreateServices(configuration);
var app = services.GetRequiredService<IApp>();

await app.RunAsync();

static ServiceProvider CreateServices (IConfiguration configuration)
{
    var services = new ServiceCollection();
    var startup = new Startup(configuration);
    startup.ConfigurationServices(services);

    return services.BuildServiceProvider();
}


static IConfigurationRoot CreateConfiguration(string[] args)
{
    return new ConfigurationBuilder()
        //.SetBasePath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location ?? default))
        .AddJsonFile("appSettings.json", true, false)
        .AddInMemoryCollection()
        .AddCustomCommandLine("AppSettings:PathFiles", args)
        .Build();
}