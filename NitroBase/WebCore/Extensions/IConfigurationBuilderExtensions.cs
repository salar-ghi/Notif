using Core.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebCore.Extensions
{
    public static class ConfigurationBuilderExtensions
    {
        /// <summary>
        /// Configures appsetting files
        /// </summary>
        /// <param name="env"></param>
        /// <param name="_configuration"></param>
        /// <param name="_applicationSetting"></param>
        public static void Config<T>(this ConfigurationBuilder builder,
            IWebHostEnvironment env,
            out IConfigurationRoot _configuration,
            out T _applicationSetting) where T : ApplicationSettingModel   , new()
        {
            builder.SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName.ToLower()}.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.MachineName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            _configuration = builder.Build();
            _applicationSetting = new T();
            _configuration.Bind(_applicationSetting);

        }

    }
}
