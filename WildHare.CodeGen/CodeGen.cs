using System;
using Microsoft.Extensions.Options;
using WildHare.Web.Models;
using WildHare.Web;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace WildHare.CodeGen
{
    public class CodeGen
    {
        private readonly IConfiguration _config;
        private readonly IHostEnvironment _env;
        private readonly AppSettings _app;


        public CodeGen( IConfiguration configuration,
                        IHostEnvironment hostEnvironment,
                        AppSettings appSettings)
        {
            _config = configuration;
            _env    = hostEnvironment;
            _app    = appSettings;
        }

        public void Generate(string cmd)
        {
            Console.WriteLine("Begin Generating Code...");

            if (cmd.ToLower() == "cssmap")
            {
                CodeGenCssMap.Init(_app.AnalyticsWriteToPath(_env.ContentRootPath), _app.AnalyticsWriteToFile, true);
            }
            else if (cmd.ToLower() == "adapters")
            {
                // CodeGenAdapters.Init(contentRootPath);
            }
            else if (cmd.ToLower() == "fromsql")
            {
                // CodeGenFromSql.Init(contentRootPath, _config.GetConnectionString("ExampleDB"));
            }
            else if (cmd.ToLower() == "classesfromsqltables")
            {
                // CodeGenClassesFromSqlTables.Init(contentRootPath, _config.GetConnectionString("ExampleDB"));
            }
            else if (cmd.ToLower() == "sqlrowinsert")
            {
                // CodeGenSqlRowInsert.Init(contentRootPath);
            }
            else if (cmd.ToLower() == "schema")
            {
                // CodeGenSchema.Init(contentRootPath);
            }
            else
            {
                Console.WriteLine("Cmd is invalid in CodeGen.Generate.");
                return;
            }

            Console.WriteLine("Generating Code Completed.");
        }
    }
}
