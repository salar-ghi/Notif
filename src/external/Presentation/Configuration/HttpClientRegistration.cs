namespace Presentation.Configuration;

public static class HttpClientRegistration
{
    public static IServiceCollection AddHttpClients(this IServiceCollection services, ApplicationSettingExtenderModel applicationSetting)
    {
        services.AddTransient<LoggingDelegateHandler>();
        services.AddHttpClient<IPayamSmsClientService, PayamSmsClientService>(client =>
        {
            client.BaseAddress = new Uri(applicationSetting.Provider.Sms.PayamSms.Url);
        })
            .AddHttpMessageHandler<LoggingDelegateHandler>()
            .AddResilienceHandler("default", GetResiliencePipeline);

        return services;
    }


    private static void GetResiliencePipeline(ResiliencePipelineBuilder<HttpResponseMessage> builder) =>
             new ResiliencePipelineBuilder()
                .AddRetry(new RetryStrategyOptions()
                {
                    MaxRetryAttempts = 3,
                    Delay = TimeSpan.FromSeconds(1),
                    BackoffType = DelayBackoffType.Constant,
                    ShouldHandle = new PredicateBuilder().Handle<HttpRequestException>((e) => (int)e.StatusCode is 227 or (int)HttpStatusCode.RequestTimeout),
                    OnRetry = (options) =>
                    {
                        var logger = ServiceLocator.GetService<ILogger<ResiliencePipelineBuilder>>();

                        logger.LogWarning(options.Outcome.Exception, $"Retry #{0}: {1}", options.AttemptNumber, options.Outcome.Result?.ToJsonString());

                        return default;
                    }
                })
               .AddTimeout(TimeSpan.FromSeconds(3))
               .Build();

}
