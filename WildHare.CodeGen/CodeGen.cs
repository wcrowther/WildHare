using System;
using Microsoft.Extensions.Options;
using WildHare.Web.Models;
using WildHare.Web;
using Microsoft.Extensions.Configuration;

namespace WildHare.CodeGen
{
    public class CodeGen
    {
        private readonly AppSettings _appSettings;
        private readonly IConfiguration _config;

        public CodeGen(AppSettings appSettings, IConfiguration configuration)
        {
            _appSettings = appSettings;
            _config = configuration;
        }

        public void Generate(string contentRootPath)
        {
            Console.WriteLine("Type y to Generate Code:  ");

            if (Console.ReadLine() == "y")
            {
                Console.WriteLine("Begin Generating Code...");

                // CodeGenCssMap.Init(contentRootPath);
                // CodeGenAdapters.Init(contentRootPath);
                // CodeGenFromSql.Init(contentRootPath, _config.GetConnectionString("ExampleDB"));
                // CodeGenClassesFromSqlTables.Init(contentRootPath, _config.GetConnectionString("ExampleDB"));
                // CodeGenSqlRowInsert.Init(contentRootPath);
                // CodeGenSchema.Init(contentRootPath);

                Console.WriteLine("Generating Code Completed.");
            }
            else
            {
                Console.WriteLine("Exiting...");

                Environment.Exit(0);
            }
        }
    }
}
