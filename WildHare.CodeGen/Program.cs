using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using System;
using System.IO;
using WildHare.Extensions;
using WildHare.Web.Interfaces;
using WildHare.Web.Models;
using static System.Environment;

namespace WildHare.CodeGen
{
    class Program
    {
        static string line = "=".Repeat(60);
        
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
            // Console.Clear();
            Console.WriteLine(line);
            Console.WriteLine("Choose an option:");
            Console.WriteLine(line);
            Console.WriteLine("1) Generate Css ");
            Console.WriteLine("2) Generate Models for each table in SQL DB");
            Console.WriteLine("x) Exit");
            Console.Write("\r\nSelect an option: ");

            var serviceProvider = ConfigureServices().BuildServiceProvider();

            string input = Console.ReadLine();
            bool result = serviceProvider.GetService<CodeGen>().Generate(input);

            if (result)
            { 
                Console.WriteLine(  $"{NewLine}Code Generation suceeded. Hit any key to proceed.");
                // Console.ReadLine();
            }

            return result;
        }

        private static IServiceCollection ConfigureServices()
        {
            // NOTE: Console app will use appsettings.json if included and Properties
            // marked as 'Copy if Newer', otherwise will use the one in the Web proj.
            
            IServiceCollection serviceCollection = new ServiceCollection();

            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
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
