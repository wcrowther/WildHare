using System;
using Microsoft.Extensions.Options;
using WildHare.Web.Models;
using WildHare.Web;
using Microsoft.Extensions.Configuration;
using static System.Environment;

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

        public bool Generate(string input)
        {
            bool remainOpen = true;

            switch (input.ToLower())
            {
                case "1":
                    CodeGenClassTagList.Init(_appSettings.ClassTagListSourceFolderRootPath,
                                             _appSettings.ClassTagListWriteToFilePath,
                                             _appSettings.ClassTagList_Overwrite);
                    break;
                case "2":
                    CodeGenFromSql.Init(@"c:\Temp\Models", "TestNamespace", _config.GetConnectionString("MachineEnglishDB"), true);
                    break;
                case "exit": case "x":
                    Console.WriteLine($"{NewLine}--> Exiting console...");
                    remainOpen = false;
                    break;
                default:
                    Console.WriteLine("That input is not valid.");
                    break;
            }
            return remainOpen;
        }
    }
}
