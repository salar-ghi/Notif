
//var builder = WebApplication.CreateBuilder(args);

//var _applicationExtenderSetting = new ApplicationSettingExtenderModel();
// Add services to the container.

//builder.Services.AddExceptionHandler<GlobalErrorHandler>();
//builder.Services.AddProblemDetails();

//IConfiguration configuration = new ConfigurationBuilder()
//    .SetBasePath(Directory.GetCurrentDirectory())
//    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
//    .AddEnvironmentVariables()
//    .AddCommandLine(args)
//    .Build();
//var SqlConnection = configuration.GetConnectionString("SqlConnection");

//builder.Services.AddDbContext<NotifContext>(options =>
//{
//    //options.UseSqlServer(configuration.GetConnectionString("SqlConnection"),
//    options.UseSqlServer(SqlConnection,
//    //options.EnableSensitiveDataLogging(),
//    sqlServerOptionsAction: sqlOptions =>
//    {
//        sqlOptions.EnableRetryOnFailure(
//        maxRetryCount: 3,
//        maxRetryDelay: TimeSpan.FromMilliseconds(10),
//        errorNumbersToAdd: null);
//    });
//}, ServiceLifetime.Scoped); //, ServiceLifetime.Transient  

//builder.Services.AddMemoryCache();
//builder.Services.AddStackExchangeRedisCache(options =>
//{
//    options.Configuration = "loclahost:6379";
//    options.InstanceName = "Notification";
//});

//builder.Services.AddDistributedMemoryCache();

//builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

//builder.Services.AddControllers();

//builder.Services.AddServices(_applicationExtenderSetting);
//builder.Services.ConfigHangfire(configuration, "Nitro_Notif", builder.Environment);



//builder.Services.AddHangfire((sp, config) =>
//{
//    var connection = sp.GetRequiredService<IConfiguration>().GetConnectionString("HangFireConnection");
//    config.UseSqlServerStorage(connection);
//});
//builder.Services.AddHangfireServer();

//builder.Services.ConfigHangfire(configuration, "Nitro_Notif", builder.Environment);

//builder.Services.AddApiVersioning();
//builder.Services.AddOpenApiDocument(config =>
//{
//    config.DocumentName = "v1";
//    config.Title = "My API";
//    config.Version = "v1";
//});

//var app = builder.Build();

//ServiceLocator.Configure(app ) ??????????

// Configure the HTTP request pipeline.

//if (app.Environment.IsDevelopment())
//{
//    app.UseDeveloperExceptionPage();
//    app.UseOpenApi();
//    app.UseSwaggerUi();
//    app.UseReDoc();
//}
//else
//{
//    app.UseExceptionHandler();
//    app.UseHsts();
//}

//app.UseHttpsRedirection();
//app.UseCors(Constants.CorsPolicyName);
//app.UseAuthorization();
//app.MapControllers();

//app.UseEndpoints(endpoints =>
//{
//endpoints.MapControllers();

//endpoints.MapHealthChecks(GlobalConstants.ApiHealthCheckRoute);
//endpoints.MapHangfireDashboard();
//});

//app.UseHangfireDashboard("/hangfire", new DashboardOptions
//{
//    DashboardTitle = "Notification Job",
//    DarkModeEnabled = true,
//    DisplayStorageConnectionString = false,
//    Authorization = new[]
//    {
//        new HangfireCustomBasicAuthenticationFilter
//        {
//            User = "Admin",
//            Pass = "adminNitro"
//        }
//    }

//});

//using(var server  = new BackgroundJobServer())
//{

//    RecurringJob.AddOrUpdate<ICacheMessage>("Notif-job", x => x.GetAllMessages(), "*/12 * * * * *");
//    //BackgroundJob.Schedule<ICacheMessage>(x => x.GetAllMessages(), TimeSpan.FromMilliseconds(200));
//    Console.ReadKey();
//}

//app.Run();

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


