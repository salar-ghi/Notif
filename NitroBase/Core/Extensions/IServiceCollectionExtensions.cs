using Core.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using Core.Abstractions;
using Core.Services;

namespace Core.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddBaseServices(this IServiceCollection services, ApplicationSettingModel applicationSetting)
        {
            services.AddTransient<IExcelBuilderService, ExcelBuilderService>();

            services.AddSingleton<IGuard, Guard>();
            services.AddSingleton<IReflectionService, ReflectionService>();

            return services;
        }

    }
}
