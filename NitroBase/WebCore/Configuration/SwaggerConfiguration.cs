using Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Mvc.Controllers;
using Pluralize.NET;
using Core.Helpers;
using System.Text;
using System.Reflection;
using System.Threading;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.IO;
using WebCore.Filters.Swagger;

namespace WebCore.Configuration
{
    public static class SwaggerConfiguration
    {
        public static IServiceCollection AddSwaggerConfig<TController>(this IServiceCollection services, Type startupClass, ApplicationSettingModel applicationSetting)
        {

            services.AddSwaggerGen(c =>
            {
                c.OperationFilter<ApplySummariesOperationFilter>();
                c.EnableAnnotations();
                c.IgnoreObsoleteActions();
                c.IgnoreObsoleteProperties();
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                c.OrderActionsBy((apiDesc) => $"{apiDesc.ActionDescriptor.RouteValues["controller"]}_{apiDesc.HttpMethod}");
                //c.IncludeXmlComments($@"{System.AppDomain.CurrentDomain.BaseDirectory}/{startupClass.Assembly.GetName().Name}.xml");
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Nitro Documentation",
                    Version = $"v1.0",
                    Contact = new OpenApiContact
                    {
                        Name = "Support",
                        Email = "info@nitrogenco.com",
                        Url = new System.Uri("https://nitrogenco.com")
                    },
                    Description = "This is some description",
                });
                //c.SwaggerDoc("v2", new OpenApiInfo
                //{
                //    Title = applicationSetting.ExposedAPI.Swagger.Title,
                //    Version = $"v{CONSTANTS.API_V2}",
                //    Contact = new OpenApiContact
                //    {
                //        Name = applicationSetting.ExposedAPI.Swagger.Name,
                //        Email = applicationSetting.ExposedAPI.Swagger.Email,
                //        Url = new System.Uri(applicationSetting.ExposedAPI.Swagger.Url)
                //    },
                //    Description = applicationSetting.ExposedAPI.Swagger.Description,
                //});


                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });

                c.CustomSchemaIds(s => s.FullName);

                c.OperationFilter<RemoveVersionParameterFilter>();
                c.DocumentFilter<ReplaceVersionWithExactValueInPathFilter>();
            });

            var v1 = new ApiVersion(1, 0);
            //var v2 = new ApiVersion(2, 0);

            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ApiVersionReader = new Microsoft.AspNetCore.Mvc.Versioning.UrlSegmentApiVersionReader();
                options.ReportApiVersions = true;
                options.RegisterMiddleware = true;
                options.Conventions.Controller<TController>().HasApiVersion(v1);
                //.Action(c => c.Get("")).HasApiVersion(v1)
                //.Action(c => c.Post(new Areas.B2B.V1_0.Models.OrderModel())).HasApiVersion(v1);
                //options.Conventions.Controller<Presentation.ExposedApis.PaymentController>().HasApiVersion(v2);
                //.Action(c => c.Get("")).HasApiVersion(v2)
                //.Action(c => c.Post(new Areas.B2B.V2_0.Models.ShipmentModel())).HasApiVersion(v2);

            }); // just add this

            services.AddVersionedApiExplorer(
                options =>
                {
                    // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
                    // note: the specified format code will format the version as "'v'major[.minor][-status]"
                    options.GroupNameFormat = "'v'VVV";
                    options.DefaultApiVersion = new ApiVersion(1, 0);
                    options.AssumeDefaultVersionWhenUnspecified = true;

                    // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                    // can also be used to control the format of the API version in route templates
                    options.SubstituteApiVersionInUrl = true;
                })
                .AddMvc(
                options =>
                {
                    options.RespectBrowserAcceptHeader = true;
                });

            return services;
        }

        public static IApplicationBuilder UseSwaggerConfig<TStartup>(this IApplicationBuilder app, ApplicationSettingModel applicationSetting, string swaggerPath = "")
        {

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            var containingAssembly = typeof(TStartup).Assembly;
            app.UseSwagger(c =>
            {
                c.RouteTemplate = $"{StringHelper.UrlCombine(swaggerPath, "/swagger/{documentName}/api-swagger.json")}";
            });

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.DocumentTitle = "Nitro Documentation";
                c.InjectStylesheet($@"/swagger-ui/site.css");
                c.EnableDeepLinking();
                c.SwaggerEndpoint($"/{StringHelper.UrlCombine(swaggerPath, "/swagger/v1/api-swagger.json")}", "API V1");
                c.RoutePrefix = $"{StringHelper.UrlCombine(swaggerPath, "swagger")}";
                c.DocExpansion(DocExpansion.List);
            });

            return app;
        }

        public class ApplySummariesOperationFilter : IOperationFilter
        {
            public void Apply(OpenApiOperation operation, OperationFilterContext context)
            {
                var controllerActionDescriptor = context.ApiDescription.ActionDescriptor as ControllerActionDescriptor;
                if (controllerActionDescriptor == null) return;
                var pluralizer = new Pluralizer();
                var actionName = controllerActionDescriptor.ActionName;
                var singularizeName = pluralizer.Singularize(controllerActionDescriptor.ControllerName);
                var pluralizeName = pluralizer.Pluralize(singularizeName);
                var descriptionBuilder = new StringBuilder();

                var parameterCount = operation.Parameters.Where(p => p.Name != "version" && p.Name != "api-version").Count();

                if (controllerActionDescriptor.Parameters.Count > 0)
                {
                    descriptionBuilder.AppendLine($" ## Action Model");

                    foreach (var parameter in controllerActionDescriptor.Parameters)
                    {
                        List<PropertyInfo> properties = null;

                        if (parameter.ParameterType == typeof(CancellationToken)) // || parameter.ParameterType.IsValueType)
                            descriptionBuilder.AppendLine(GetAttributesText(parameter.ParameterType));
                        else
                        {
                            properties = parameter.ParameterType.GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(p => p.GetGetMethod(false) != null).ToList();
                            foreach (var property in properties)
                                descriptionBuilder.AppendLine(GetAttributesText(property));
                        }
                    }
                    descriptionBuilder.AppendLine("___");
                }

                var OkResultType = controllerActionDescriptor.FilterDescriptors.Where(e => e.Filter is ProducesResponseTypeAttribute && (((ProducesResponseTypeAttribute)e.Filter).StatusCode >= 200 || ((ProducesResponseTypeAttribute)e.Filter).StatusCode < 300)).OrderByDescending(d => d.Scope).FirstOrDefault();
                var okResponseType = OkResultType.Filter as ProducesResponseTypeAttribute;
                if (okResponseType.Type.GenericTypeArguments.Any())
                {
                    descriptionBuilder.AppendLine($" ## RESPONSE Result Model");
                    descriptionBuilder.AppendLine($" List of ");
                    var type = okResponseType.Type.GenericTypeArguments.First();

                    descriptionBuilder.AppendLine(GetAttributesText(type));

                    var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(p => p.GetGetMethod(false) != null).ToList();
                    foreach (var property in properties)
                        descriptionBuilder.AppendLine(GetAttributesText(property));
                    //descriptionBuilder.AppendLine("___");
                }

                operation.Description = descriptionBuilder.ToString();

                if (IsGetAllAction())
                {
                    if (string.IsNullOrEmpty(operation.Summary))
                        operation.Summary = $"Returns all {pluralizeName}";
                }
                else if (IsActionName("Post", "Create", "Add"))
                {
                    if (string.IsNullOrEmpty(operation.Summary))
                        operation.Summary = $"Creates a {singularizeName}";

                    if (operation.Parameters.Count > 0 && string.IsNullOrEmpty(operation.Parameters[0].Description))
                        operation.Parameters[0].Description = $"A {singularizeName} representation";
                }
                else if (IsActionName("Read", "Get"))
                {
                    if (string.IsNullOrEmpty(operation.Summary))
                        operation.Summary = $"Retrieves a {singularizeName} by unique id";

                    if (operation.Parameters.Count > 0 && string.IsNullOrEmpty(operation.Parameters[0].Description))
                        operation.Parameters[0].Description = $"a unique id for the {singularizeName}";
                }
                else if (IsActionName("Put", "Edit", "Update"))
                {
                    if (string.IsNullOrEmpty(operation.Summary))
                        operation.Summary = $"Updates a {singularizeName} by unique id";

                    //if (!operation.Parameters[0].Description.HasValue())
                    //    operation.Parameters[0].Description = $"A unique id for the {singularizeName}";

                    if (operation.Parameters.Count > 0 && string.IsNullOrEmpty(operation.Parameters[0].Description))
                        operation.Parameters[0].Description = $"A {singularizeName} representation";
                }
                else if (IsActionName("Delete", "Remove"))
                {
                    if (string.IsNullOrEmpty(operation.Summary))
                        operation.Summary = $"Deletes a {singularizeName} by unique id";

                    if (operation.Parameters.Count > 0 && string.IsNullOrEmpty(operation.Parameters[0].Description))
                        operation.Parameters[0].Description = $"A unique id for the {singularizeName}";
                }

                #region Local Functions

                bool IsGetAllAction()
                {
                    foreach (var name in new[] { "Get", "Read", "Select" })
                    {
                        if ((actionName.Equals(name, StringComparison.OrdinalIgnoreCase) && parameterCount == 0) ||
                            actionName.Equals($"{name}All", StringComparison.OrdinalIgnoreCase) ||
                            actionName.Equals($"{name}{pluralizeName}", StringComparison.OrdinalIgnoreCase) ||
                            actionName.Equals($"{name}All{singularizeName}", StringComparison.OrdinalIgnoreCase) ||
                            actionName.Equals($"{name}All{pluralizeName}", StringComparison.OrdinalIgnoreCase))
                        {
                            return true;
                        }
                    }
                    return false;
                }

                bool IsActionName(params string[] names)
                {
                    foreach (var name in names)
                    {
                        if (actionName.Equals(name, StringComparison.OrdinalIgnoreCase) ||
                            actionName.Equals($"{name}ById", StringComparison.OrdinalIgnoreCase) ||
                            actionName.Equals($"{name}{singularizeName}", StringComparison.OrdinalIgnoreCase) ||
                            actionName.Equals($"{name}{singularizeName}ById", StringComparison.OrdinalIgnoreCase))
                        {
                            return true;
                        }
                    }
                    return false;
                }
                #endregion
            }
        }


        static bool IsListType(Type type)
        {
            var enumeratorMethod = type.GetMethod(nameof(List<object>.GetEnumerator));
            return (enumeratorMethod != null);
        }

        static string GetAttributesText(PropertyInfo property)
        {
            var description = "";
            var minLength = 0;
            var maxLength = 0;
            var isRequired = false;
            Tuple<string, string> range = null;
            var regularExpression = "";

            var descriptionAttr = GetCustomAttribute<DescriptionAttribute>(property);
            if (descriptionAttr != null)
                description = descriptionAttr.Description;

            var minLengthAttr = GetCustomAttribute<MinLengthAttribute>(property);
            if (minLengthAttr != null)
                minLength = minLengthAttr.Length;

            var maxLengthAttr = GetCustomAttribute<MaxLengthAttribute>(property);
            if (maxLengthAttr != null)
                maxLength = maxLengthAttr.Length;

            var requiredAttr = GetCustomAttribute<RequiredAttribute>(property);
            isRequired = (requiredAttr != null);

            var rangeAttr = GetCustomAttribute<RangeAttribute>(property);
            if (rangeAttr != null)
                range = new Tuple<string, string>(rangeAttr.Minimum.ToString(), rangeAttr.Maximum.ToString());

            var regularExpressionAttr = GetCustomAttribute<RegularExpressionAttribute>(property);
            if (regularExpressionAttr != null)
                regularExpression = regularExpressionAttr.Pattern;

            return FormatText(property.Name, property.PropertyType.Name, isList: false, isOptional: !isRequired,
                              description: description, minLength: minLength, maxLength: maxLength, range: range, regularExpressionPattern: regularExpression);

        }

        static string GetAttributesText(Type type)
        {
            var description = "";
            var minLength = 0;
            var maxLength = 0;
            var isRequired = false;
            Tuple<string, string> range = null;
            var regularExpression = "";

            var isList = IsListType(type);
            if (isList)
                type = type.GenericTypeArguments.First();

            var descriptionAttr = GetCustomAttribute<DescriptionAttribute>(type);
            if (descriptionAttr != null)
                description = descriptionAttr.Description;

            var minLengthAttr = GetCustomAttribute<MinLengthAttribute>(type);
            if (minLengthAttr != null)
                minLength = minLengthAttr.Length;

            var maxLengthAttr = GetCustomAttribute<MaxLengthAttribute>(type);
            if (maxLengthAttr != null)
                maxLength = maxLengthAttr.Length;

            var requiredAttr = GetCustomAttribute<RequiredAttribute>(type);
            isRequired = (requiredAttr != null);

            var rangeAttr = GetCustomAttribute<RangeAttribute>(type);
            if (rangeAttr != null)
                range = new Tuple<string, string>((string)rangeAttr.Minimum, (string)rangeAttr.Maximum);

            var regularExpressionAttr = GetCustomAttribute<RegularExpressionAttribute>(type);
            if (regularExpressionAttr != null)
                regularExpression = regularExpressionAttr.Pattern;

            return FormatText(type.Name, type.UnderlyingSystemType.Name, isList: isList, isOptional: !isRequired,
                              description: description, minLength: minLength, maxLength: maxLength, range: range, regularExpressionPattern: regularExpression);
        }

        private static T GetCustomAttribute<T>(PropertyInfo property) where T : class
        {
            var attribs = property.GetCustomAttributes(typeof(T), true);
            return attribs.LastOrDefault() as T;
        }

        private static T GetCustomAttribute<T>(Type type) where T : class
        {
            var attribs = type.GetCustomAttributes(typeof(T), true);
            return attribs.LastOrDefault() as T;
        }

        private static string FormatText(string name, string type, bool isList = false, string defaultValue = "", bool isOptional = true,
                                         string description = "", int minLength = 0, int maxLength = 0, Tuple<string, string> range = null, string regularExpressionPattern = "")
        {
            var outputBuilder = new StringBuilder();
            var listOfText = (isList) ? "List of " : "";
            outputBuilder.AppendLine($"{listOfText } **{name}** ```{type}```");
            if (defaultValue == null)
                outputBuilder.AppendLine(@$"null ");
            else if (!string.IsNullOrWhiteSpace(defaultValue))
                outputBuilder.AppendLine(@$"[{defaultValue}] ");
            if (isOptional)
                outputBuilder.AppendLine(@$"<sub>[*optional*]</sub> ");
            else
                outputBuilder.AppendLine(@$"<sub>[*required*]</sub> ");
            if (minLength > 0)
                outputBuilder.AppendLine(@$"<sub>[*min: {minLength}*]</sub> ");
            if (maxLength > 0)
                outputBuilder.AppendLine(@$"<sub>[*max: {maxLength}*]</sub> ");
            if (range != null)
                outputBuilder.AppendLine(@$"<sub>[*range: from: {range.Item1} to: {range.Item2}*]</sub> ");
            if (!string.IsNullOrWhiteSpace(regularExpressionPattern))
                outputBuilder.AppendLine(@$"<sub>[*Pattern: {regularExpressionPattern}*]</sub> ");
            if (!string.IsNullOrWhiteSpace(description))
                outputBuilder.AppendLine(@$"<br style=""padding: 5px;"" /> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; *{description}* ");

            outputBuilder.AppendLine(@"  <br /><br /> ");
            return outputBuilder.ToString();
        }

    }
}
