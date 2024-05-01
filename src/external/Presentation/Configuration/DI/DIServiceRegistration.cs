using Microsoft.Extensions.DependencyInjection;
namespace Presentation.Configuration.DI;

public static class DIServiceRegistration
{
    public static IServiceCollection AddServices(this IServiceCollection services, ApplicationSettingModel applicationSetting)
    {
        // Transient
        //services.AddTransient();


        // Scoped
        //services.AddScoped();
        services.AddScoped<INotifService, NotifService>();

        //services.AddScoped<INotifSender,>()

        services.AddScoped<INotifSender, EmailNotifSender>();
        services.AddScoped<INotifSender, SmsNotifSender>();
        services.AddScoped<INotifSender, MessageBrokerNotifSender>();

        // Singleton
        //services.AddSingleton();

        services.AddBaseServices(applicationSetting);

        return services;
    }
}
