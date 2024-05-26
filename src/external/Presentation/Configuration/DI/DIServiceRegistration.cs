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

        services.AddTransient<ISmsProvider, SmsService>();
        services.AddTransient<IEmailProvider, EmailService>();

        services.AddScoped<IMelipayamak, Melipayamak>();
        services.AddScoped<IIdehpardazan, Idehpardazan>();
        services.AddScoped<IPayamSms, PayamSms>();

        services.AddScoped<IMessageManagementService, MessageManagementService>();
        services.AddTransient<IMessageService, MessageService>();

        // Scoped
        //services.AddScoped();
        services.AddScoped<IMessageSender, MessageSenderService>();

        services.AddScoped<IMessageLogService, MessageLogService>();
        services.AddScoped<IProviderService, ProviderService>();        

        services.AddScoped<ICacheMessage, InMemoryCacheRepository>();

        //services.AddScoped<IElasticsearchService, ElasticsearchService>();


        // Singleton
        //services.AddSingleton();

        services.AddBaseServices(applicationSetting);
        return services;
    }
}
