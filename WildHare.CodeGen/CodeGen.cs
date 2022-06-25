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
            Console.WriteLine("Press any key to Generate Code. Press x to exit.");

            if (Console.ReadLine() == "x")
            {
                Environment.Exit(0);
            }

            Console.WriteLine("Begin Code Generating Code...");

            CodeGenClassTagList.Init(_appSettings.ClassTagListSourceFolderRootPath,
                                     _appSettings.ClassTagListWriteToFilePath,
                                     _appSettings.ClassTagList_Overwrite);

            // CodeGenAdapters.Init(contentRootPath);
            // CodeGenFromSql.Init(contentRootPath, _config.GetConnectionString("ExampleDB"));
            // CodeGenClassesFromSqlTables.Init(contentRootPath, _config.GetConnectionString("ExampleDB"));
            // CodeGenSqlRowInsert.Init(contentRootPath);
            // CodeGenSchema.Init(contentRootPath);

            Console.WriteLine("Code Generation Completed.");

        }
    }
}
