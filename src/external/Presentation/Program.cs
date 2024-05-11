var builder = WebApplication.CreateBuilder(args);

var _applicationExtenderSetting = new ApplicationSettingExtenderModel();
// Add services to the container.
builder.Services.AddExceptionHandler<GlobalErrorHandler>();
builder.Services.AddProblemDetails();

IConfiguration configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
    .AddCommandLine(args)
    .Build();
var SqlConnection = configuration.GetConnectionString("SqlConnection");

builder.Services.AddDbContext<NotifContext>(options =>
{
    //options.UseSqlServer(configuration.GetConnectionString("SqlConnection"),
    options.UseSqlServer(SqlConnection,
    //options.EnableSensitiveDataLogging(),
    sqlServerOptionsAction: sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure(
        maxRetryCount: 3,
        maxRetryDelay: TimeSpan.FromMilliseconds(10),
        errorNumbersToAdd: null);
    });
}, ServiceLifetime.Scoped); //, ServiceLifetime.Transient  

builder.Services.AddMemoryCache();
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "loclahost:6379";
    options.InstanceName = "Notification";
});

//builder.Services.AddDistributedMemoryCache();

builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

builder.Services.AddControllers();

builder.Services.AddServices(_applicationExtenderSetting);
builder.Services.ConfigHangfire(configuration, "Nitro_Notif", builder.Environment);



//builder.Services.AddHangfire((sp, config) =>
//{
//    var connection = sp.GetRequiredService<IConfiguration>().GetConnectionString("HangFireConnection");
//    config.UseSqlServerStorage(connection);
//});
//builder.Services.AddHangfireServer();

//builder.Services.ConfigHangfire(configuration, "Nitro_Notif", builder.Environment);

builder.Services.AddApiVersioning();
builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "v1";
    config.Title = "My API";
    config.Version = "v1";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi();
    app.UseReDoc();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    DashboardTitle = "Notification Job",
    DarkModeEnabled = true,
    DisplayStorageConnectionString = false,
    Authorization = new[]
    {
        new HangfireCustomBasicAuthenticationFilter
        {
            User = "Admin",
            Pass = "adminNitro"
        }
    }

});
RecurringJob.AddOrUpdate<ICacheMessage>("Notif-job", x => x.GetAllMessages(), "*/2 * * * * *");

app.Run();
