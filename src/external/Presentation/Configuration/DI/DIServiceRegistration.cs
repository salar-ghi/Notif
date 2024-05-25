using Application.Configuration;
using Infrastructure.Configuration;

namespace Presentation.Configuration.DI;

public static class DIServiceRegistration
{
    public static IServiceCollection AddServices(this IServiceCollection services, ApplicationSettingModel applicationSetting)
    {
        // Transient
        //services.AddTransient();
        services.AddTransient<IUnitOfWork, UnitOfWork>();

        services.AddTransient<Melipayamak>();
        services.AddTransient<Idehpardazan>();
        services.AddTransient<PayamSms>();

        //services.AddTransient<ISmsProvider, Idehpardazan>();
        //services.AddTransient<ISmsProvider, Melipayamak>();

        services.AddTransient<ISmsProvider, SmsService>();
        services.AddTransient<IEmailProvider, EmailService>();


        services.AddScoped<IMelipayamak, Melipayamak>();
        services.AddScoped<IIdehpardazan, Idehpardazan>();
        services.AddScoped<IPayamSms, PayamSms>();


        services.AddScoped<INotifManagementService, NotifManagementService>();

        // Scoped
        //services.AddScoped();
        services.AddScoped<ISaveNotifToStorageJob, SaveNotifToStorageJob>();
        services.AddScoped<ISendNotifJob, SendNotifJob>();

        services.AddTransient<INotifService, NotifService>();


        services.AddScoped<INotifSender, NotifSenderService>();


        services.AddScoped<INotifLogService, NotifLogService>();
        services.AddScoped<IProviderService, ProviderService>();
        

        services.AddScoped<ICacheMessage, InMemoryCacheRepository>();

        //services.AddScoped<IElasticsearchService, ElasticsearchService>();

        //services.AddScoped<ISaveNotifToStorageJob, >();
        //services.AddScoped<ICacheMessage, RedisCacheRepository>();
        //RecurringJob.AddOrUpdate<ICacheMessage>("Notif-job", x => x.GetAllMessages(), "*/2 * * * * *");
        //BackgroundJob.Schedule<ICacheMessage>(x => x.GetAllMessages(), TimeSpan.FromSeconds(2));

        // Singleton
        //services.AddSingleton();

        services.AddBaseServices(applicationSetting);
        return services;
    }
}
