using Common.WebApi;
using Hermes.DataAccess.Commun;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: ApiController]
namespace Hermes.WebApi.Entreprises
{
    /// <summary>
    /// Implémente le code de démarrage de l'application ASP.NET Core.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Crée une instance de <see cref="Startup"/> en spécifiant la configuration.
        /// </summary>
        /// <param name="configuration">La configuration de l'application ASP.NET Core.</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// La configuration de l'application ASP.NET Core.
        /// </summary>
        private IConfiguration Configuration { get; }

        /// <summary>
        /// Ajoute les services communs et ceux spécifiques aux contrôleurs de ce projet dans le conteneur ASP.NET Core.
        /// </summary>
        /// <param name="services">La collection des services.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddWebApiServices(Configuration);
            services.AddTransient<ICommunRepository, CommunRepository>();
        }

        /// <summary>
        /// Configure le pipeline de requêtes HTTP ASP.NET Core.
        /// </summary>
        /// <param name="app">Le <see cref="IApplicationBuilder"/> qui permet de créer l'application ASP.NET Core.</param>
        /// <param name="env">L'environnement Web dans lequel l'application ASP.NET Core s'exécute.</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseWebApiServices(env);
        }
    }
}
