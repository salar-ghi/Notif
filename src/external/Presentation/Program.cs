var builder = WebApplication.CreateBuilder(args);

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

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddApiVersioning();
builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "v1";
    config.Title = "My API";
    config.Version = "v1";
});
builder.Services.AddControllers();

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

app.Run();
