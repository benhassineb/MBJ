using System;
using System.IO;
using Common.Resources;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Common.WebApi
{
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Construit le pipeline ASP.NET Core avec les services nécessaires à la mise en oeuvre des Web API Action Logement Services.
        /// </summary>
        /// <param name="app">L'instance <see cref="IApplicationBuilder" />.</param>
        /// <param name="env">L'environnement <see cref="IHostingEnvironment" /> d'exécution de l'application .</param>
        /// <returns>Une référence <see cref="IApplicationBuilder" /> qui peut être utilisée pour chaîner l'utilisation d'autres services.</returns>
        public static IApplicationBuilder UseWebApiServices(this IApplicationBuilder app, IHostingEnvironment env)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));
            if (env == null) throw new ArgumentNullException(nameof(env));
            app.UseMiddleware<MonitoringMiddleware>();
            if (!env.IsDevelopment())
            {
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseCors(Constants.PolicyName.CorsSameDomain);
            app.UseAuthentication();
            app.UseMvc();

            return app;
        }



        /// <summary>
        /// Construit le pipeline ASP.NET Core avec les services nécessaires à la mise en oeuvre des frontends Action Logement Services.
        /// </summary>
        /// <param name="app">L'instance <see cref="IApplicationBuilder" />.</param>
        /// <param name="env">L'environnement <see cref="IHostingEnvironment" /> d'exécution de l'application .</param>
        /// <returns>Une référence <see cref="IApplicationBuilder" /> qui peut être utilisée pour chaîner l'utilisation d'autres services.</returns>
        public static IApplicationBuilder UseFrontEnd(this IApplicationBuilder app, IHostingEnvironment env)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));
            if (env == null) throw new ArgumentNullException(nameof(env));
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.Run(async (context) =>
            {
                context.Response.ContentType = "text/html";
                await context.Response.SendFileAsync(Path.Combine(env.WebRootPath, "index.html"));
            });
            return app;
        }

    }

}
