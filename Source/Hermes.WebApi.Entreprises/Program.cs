using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Hermes.WebApi.Entreprises
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args)
                .Build()
                .Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .CaptureStartupErrors(true)
                .UseHttpSys()
                .UseStartup<Startup>();

    }
}
