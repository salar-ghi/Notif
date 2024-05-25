using Elastic.Clients.Elasticsearch;
using Elasticsearch.Net;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Presentation.Jobs;

namespace Presentation;

public class Startup
{
    private readonly IConfigurationRoot _configuration;
    private ApplicationSettingExtenderModel _applicationExtenderSetting;
    private IWebHostEnvironment _environment;

    public Startup(IWebHostEnvironment env)
    {
        _environment = env;

        var builder = new ConfigurationBuilder();
        builder.Config(env,
                       out _configuration,
                       out _applicationExtenderSetting);
        _applicationExtenderSetting = new ApplicationSettingExtenderModel();
        _configuration.Bind(_applicationExtenderSetting);
    }


    public void ConfigureServices(IServiceCollection services)
    {
        services.AddExceptionHandler<GlobalErrorHandler>();
        services.AddProblemDetails();

        services.AddDbContext<MessageContext>(options =>
        {
            //options.UseSqlServer(configuration.GetConnectionString("SqlConnection"),
            options.UseSqlServer(_configuration.GetConnectionString("SqlConnection"),
            sqlServerOptionsAction: sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 3,
                maxRetryDelay: TimeSpan.FromMilliseconds(10),
                errorNumbersToAdd: null);
            });
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }, ServiceLifetime.Scoped); //, ServiceLifetime.Transient
                                    //

        //services.AddHostedService<CheckStorageBackgroundService>();
        services.AddOptions();
        services.AddHttpContextAccessor();
        services.AddHealthChecks()
                .AddCheck<ApplicationHealthCheck>(nameof(ApplicationHealthCheck));

        var autoMapperConfig = new MapperConfiguration(cfg =>
            cfg.AddMaps(
                nameof(Domain),
                nameof(Application),
                nameof(Infrastructure),
                nameof(Presentation),
                nameof(WebCore)
                ));

        var mapper = autoMapperConfig.CreateMapper();
        services.AddSingleton(mapper);

        #region ApplicationSettingModel Binding
        services.AddSingleton(typeof(ApplicationSettingExtenderModel), _applicationExtenderSetting);
        services.AddSingleton(typeof(ApplicationSettingModel), _applicationExtenderSetting);
        #endregion

        services.ConfigResponseCompression();

        services.AddMemoryCache();
        //services.AddStackExchangeRedisCache(options =>
        //{
        //    options.Configuration = "loclahost:6379";
        //    options.InstanceName = "Notification";
        //});

        services.AddServices(_applicationExtenderSetting);
        services.AddHttpClients(_applicationExtenderSetting);

        services.ConfigHangfire(_configuration, "Nitro_Notif", _environment);
        //services.AddHangfireServer(x =>
        //{
        //    x.MaxDegreeOfParallelismForSchedulers = 10;
        //    x.ServerTimeout = TimeSpan.FromMilliseconds(240);
        //});

        //services.AddFluentEmail((ConfigurationManager)_configuration);
        services.AddFluentEmail(_configuration) ;

        //var elasticsearchSettings = _configuration.GetSection("ElasticsearchSettings").Get<ElasticsearchSettings>();
        //services.AddSingleton(elasticsearchSettings);
        //var pool = new SingleNodeConnectionPool(new Uri(elasticsearchSettings.Url));
        
        
        //var settings = new ConnectionSettings(pool).
        //    .DefaultIndex(elasticsearchSettings.DefaultIndex)
        //    .BasicAuthentication(elasticsearchSettings.Username, elasticsearchSettings.Password);
        //services.AddSingleton<IElasticsearchClient>(new ElasticsearchClient(settings));



        services.AddControllers(options =>
        {
            options.Filters.Add(new AssignCorrelationId());
        })
            .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
            options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.JsonSerializerOptions.NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString;
            options.JsonSerializerOptions.AllowTrailingCommas = true;
            options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
            options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;

        });

        //services.AddFluentValidationAutoValidation()
        //    .AddFluentValidationClientsideAdapters(fl =>
        //    {
        //    });

        services.AddMvc()
            .AddFluentValidation (fv =>
            {
                fv.ImplicitlyValidateChildProperties = true;
                fv.RegisterValidatorsFromAssembly(typeof(Startup).Assembly);
            });

        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });

        services.AddCors(o => o.AddPolicy(Constants.CorsPolicyName, builder =>
        {
            builder.WithOrigins("*")
                .AllowAnyMethod()
                .AllowAnyHeader();
        }));

        services.AddSwaggerConfig<NotifController>(typeof(Startup), _applicationExtenderSetting);
    }


    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
    {
        ServiceLocator.Configure(app.ApplicationServices, _applicationExtenderSetting);
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwaggerConfig<Startup>(_applicationExtenderSetting);
        }
        else
        {
            app.UseExceptionHandler();
            app.UseHsts();
        }

        app.UseStatusCodePages(async context => await context.UseHttpStatusCodePagesAsync(app, loggerFactory, env));
        app.UseHttpsRedirection();

        app.UseRouting();


        app.UseSerilogRequestLogging();

        app.UseCors(Constants.CorsPolicyName);
        //app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();

            endpoints.MapHealthChecks(GlobalConstants.ApiHealthCheckRoute);
            endpoints.MapHangfireDashboard();
        });

        app.UseResponseCompression();

        JobScheduler.ScheduleJobs(app, _applicationExtenderSetting);



        //RecurringJob.AddOrUpdate<CheckStorageBackgroundService>("Save data to cache ", x => x.ExecuteAsync(), "*/2 * * * *");

        //RecurringJob.AddOrUpdate<SendNotifBackgroundService>("Send Notif", x => x.ExecuteAsync(), "*/2 * * * *");
    }
}
