using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using System;
using System.IO;
using WildHare.Web.Models;

namespace WildHare.CodeGen
{
    class Program
    {
        static void Main(string[] args)
        {
            string contentRootPath = @"C:\Git\WildHare\WildHare.Web";

            var serviceProvider = ConfigureServices().BuildServiceProvider();

            serviceProvider.GetService<CodeGen>().Generate(contentRootPath);
        }

        private static IServiceCollection ConfigureServices()
        {
            IServiceCollection serviceCollection = new ServiceCollection();

            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appSettings.json", optional: false, reloadOnChange: true)
                .AddUserSecrets("bce71bbc-0b7c-4fb3-bd9f-77582fdf3b51")
                .Build();

            // add services
            serviceCollection.AddSingleton(configuration)
                                .AddSingleton(configuration.GetSection("App").Get<AppSettings>())
                                .AddSingleton<IHostEnvironment, HostingEnvironment>()
                                //.AddScoped<ICodeGenManager, CodeGenManager>()
                                //.AddScoped<IDataRepo, DataRepo>()
                                .AddScoped<CodeGen>();

            return serviceCollection;
        }
    }
}
