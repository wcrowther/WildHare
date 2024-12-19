using CodeGen.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using System;
using System.IO;
using WildHare.Extensions;
using static System.Environment;

namespace CodeGen
{
    public class Program
    {
		static bool _showMenu = true;

		static void Main(string[] args)
        {
            while (_showMenu)
            {
                _showMenu = MainMenu();
            }
        }

        private static bool MainMenu()
        {
			var serviceProvider = ConfigureServices().BuildServiceProvider();

			bool result	= serviceProvider
							.GetService<CodeGen>()
							.GenerateMenu();
            return result;
        }

        private static ServiceCollection ConfigureServices()
        {
            // =========================================================================
            // NOTE: Console app will use appsettings.json if included and Properties
            // marked as 'Copy if Newer', otherwise will use the one in the Web proj.
            // =========================================================================

            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddUserSecrets("bce71bbc-0b7c-4fb3-bd9f-77582fdf3b51")
                .Build();

            var serviceCollection = new ServiceCollection();

            serviceCollection.AddSingleton(configuration)
                             .AddSingleton(configuration.GetSection("App").Get<App>())
                             .AddSingleton<IHostEnvironment, HostingEnvironment>()
                              //.AddScoped<ICodeGenManager, CodeGenManager>()
                              //.AddScoped<IDataRepo, DataRepo>()
                             .AddScoped<CodeGen>();

            return serviceCollection;
        }
    }
}
