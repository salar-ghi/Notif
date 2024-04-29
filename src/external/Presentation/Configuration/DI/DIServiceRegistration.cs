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


        // Singleton
        //services.AddSingleton();

        services.AddBaseServices(applicationSetting);
        return services;
    }
}
