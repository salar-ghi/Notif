using Presentation;
using Serilog;
using WebCore.Loggers.Serilog.Configuration;

public class Program
{
    public static void Main(string[] args)
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", true, true)
            .Build();

        Log.Logger = SerilogConfiguration.CreateLogger(config);
        try
        {
            Log.Information("Starting web host");
            CreateHostBuilder(args,config).Build().Run();
        }
        catch (Exception ex)
        {
            Log.Logger.Error(ex, "Error occured");
            Log.Fatal(ex, "Host terminated unexpectedly");
        }
        finally 
        {
            Log.CloseAndFlush();
        }
    }

    public static IHostBuilder CreateHostBuilder(string[] args, IConfigurationRoot config) =>
                Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {

                    // Add services to the container.
                    //webBuilder.UseUrls("https://*:5001", "http://*:5000");

                    var url = config.GetSection("App:Url").Value;

                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseUrls(url);
                    webBuilder.UseKestrel(option => option.AddServerHeader = false);

                })
                .UseSerilog((context, services, configuration) =>
                    configuration.ReadFrom.Services(services)
                );
}


