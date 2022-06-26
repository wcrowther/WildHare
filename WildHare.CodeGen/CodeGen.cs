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

        public bool Generate(string input)
        {
            bool result = false;
            
            switch (input.ToLower())
            {
                case "1":
                    Console.WriteLine("Run Choice 1");
                    // CodeGenCssMap.Init(_app.AnalyticsWriteToPath(_env.ContentRootPath), _app.AnalyticsWriteToFile, true);
                    break;
                case "2":
                case "exit":
                    result = false;
                    break;
                default:
                    Console.WriteLine("Input is invalid in CodeGen.Generate.");
                    break;
            }
            return result;
        }
    }
}
