namespace Presentation.Configuration.DI;

public static class DIServiceRegistration
{
    public static IServiceCollection AddServices(this IServiceCollection services, ApplicationSettingModel applicationSetting)
    {
        // Transient
        //services.AddTransient();
        services.AddTransient<IUnitOfWork, UnitOfWork>();
       
        // Scoped
        //services.AddScoped();
        services.AddScoped<INotifService, NotifService>();

        services.AddScoped<INotifSender, SmsNotifSender>();
        services.AddScoped<ISmsProvider, SmsNotifSender >();

        //services.AddScoped<INotifSender, EmailNotifSender>();
        //services.AddScoped<INotifSender, MessageBrokerNotifSender>();

        services.AddScoped<ICacheMessage, InMemoryCacheRepository>();
        //services.AddScoped<ICacheMessage, RedisCacheRepository>();
        //RecurringJob.AddOrUpdate<ICacheMessage>("Notif-job", x => x.GetAllMessages(), "*/2 * * * * *");
        //BackgroundJob.Schedule<ICacheMessage>(x => x.GetAllMessages(), TimeSpan.FromSeconds(2));


        // Singleton
        //services.AddSingleton();

        services.AddBaseServices(applicationSetting);
        return services;
    }
}
