using CodeGen.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using System.IO;
using WildHare.Extensions;

namespace CodeGen;

public class Program
{
	static bool _showMenu = true;

	// ====================================================================================

	static void Main(string[] args)
    {
		if (args.Length > 0)
		{
			RunWithoutMenu(args);
			return;
		}
		
		while (_showMenu)
        {
            _showMenu = Menu();
        }
    }

	// ====================================================================================

	private static bool Menu()
    {
		return GetCodeGen().GenerateMenu();
    }

	private static void RunWithoutMenu(string[] inputs)
	{
		GetCodeGen().RunCodeGen(inputs);
	}

	private static CodeGen GetCodeGen()
	{
		var serviceProvider = ConfigureServices()
								.BuildServiceProvider();

		return serviceProvider.GetService<CodeGen>();
	}

	private static ServiceCollection ConfigureServices()
    {
        // =================================================================================
        // NOTE: Console app will use appsettings.json if included and Properties
        // marked as 'TransformFiles if Newer', (?) otherwise will use the one in the Web proj .
        // =================================================================================

        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddUserSecrets("bce71bbc-0b7c-4fb3-bd9f-77582fdf3b51")
            .Build();

        var serviceCollection = new ServiceCollection();

        serviceCollection.AddSingleton(configuration)
                         .AddSingleton(configuration.GetSection("AppSettings").Get<AppSettings>())
                         .AddSingleton<IHostEnvironment, HostingEnvironment>()
                          //LIKE .AddScoped<IDataRepo, DataRepo>()
                         .AddScoped<CodeGen>();

        return serviceCollection;
    }

	// =====================================================================================
}
