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
            bool showMenu = true;
            while (showMenu)
            {
                showMenu = MainMenu();
            }  
        }

        private static bool MainMenu()
        {
            Console.Clear();
            Console.WriteLine("Choose an option:");
            Console.WriteLine("1) Generate Css ");
            Console.WriteLine("2) Exit");
            Console.Write("\r\nSelect an option: ");

            var serviceProvider = ConfigureServices().BuildServiceProvider();

            string input = Console.ReadLine();

            return serviceProvider.GetService<CodeGen>().Generate(input);
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
