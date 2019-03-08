using System;
using System.Globalization;
using Common.Monitoring;
using Common.Resources;
using Common.WebApi.Authorization;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureADB2C.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Common.WebApi
{
    public static class ServiceColllectionExtensions
    {
        /// <summary>
        /// Ajoute les services nécessaires à la mise en oeuvre des Web API Action Logement Services.
        /// </summary>
        /// <param name="services">La <see cref="IServiceCollection"></see> à laquelle ajouter ce service.</param>
        /// <param name="configuration">La collection des clés de configuration.</param>
        /// <returns>Une référence <see cref="IMvcBuilder" /> qui peut être utilisée pour ajouter d'autres services.</returns>
        public static IServiceCollection AddWebApiServices(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddSingleton<IEnsureConfiguration, EnsureConfiguration>()
                .AddSingleton(InitMonitoring(configuration))

                .AddAndConfigureCors()
                .AddAndConfigureAuthentication(configuration)

                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddFluentValidation(fv =>
                {
                    fv.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
                });

            IServiceProvider provider = services.BuildServiceProvider();
            IEnsureConfiguration ensuredConfiguration = provider.GetRequiredService<IEnsureConfiguration>();
            var authorizationService = new ResourceAuthorizationService(ensuredConfiguration);
            services.AddSingleton<IResourceAuthorizationService>(authorizationService);
            services.AddSingleton<IResourceAuthorizationCacheService>(authorizationService);

            return services.AddApiVersioning();
        }


        private static IServiceCollection AddAndConfigureCors(this IServiceCollection services)
        {
            return services
                .AddCors(options =>
                {
                    options.AddPolicy(Constants.PolicyName.CorsSameDomain, builder =>
                    {
                        builder
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials()
                            .SetIsOriginAllowedToAllowWildcardSubdomains()
                            .WithOrigins("https://*.lvh.me");
                    });
                });
        }

        private static IServiceCollection AddAndConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddAuthentication(AzureADB2CDefaults.BearerAuthenticationScheme)
                .AddAzureADB2CBearer(options => configuration.Bind("AzureAdB2C", options));
            return services;
        }

        /// <summary>
        ///     Initialise le monitoring de la Web API.
        /// </summary>
        /// <param name="configuration">La collection des clés de configuration.</param>
        private static IFrameworkTracer InitMonitoring(IConfiguration configuration)
        {
            MonitoringOptions options = configuration.GetSection(nameof(MonitoringOptions)).Get<MonitoringOptions>();
            if (string.IsNullOrEmpty(options.TraceSourceName)) throw new ApplicationConfigurationException(ErrorCodes.Application.InvalidConfiguration, string.Format(CultureInfo.CurrentCulture, InternalMessages.ApplicationConfigurationMissingSetting, nameof(options.TraceSourceName)));
            var tracer = DotNetTracerFactory.Instance.CreateTracer(TraceSourceType.Web, options.TraceSourceName);
            // Interception de l'ensemble des exceptions 
            tracer.SetExceptionHandling(options.LogFirstChanceExceptions);
            return tracer;
        }

    }
}
