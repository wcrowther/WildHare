using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WildHare.Extensions;
using WildHare.Web.Models;

namespace WildHare.Web
{
    public class Startup(IConfiguration configuration)
	{
        private string _dbConnString;

		public IConfiguration Configuration { get; } = configuration;

		// ================================================================================
		public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();

            services.AddSingleton(Configuration.GetSection("App").Get<AppSettings>());

            _dbConnString = Configuration["ConnectionStrings:MachineEnglishDB"];  // TestDB
		}

        // ================================================================================

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });

			// RunCodeGen(env);
        }
    }
}




// private void RunCodeGen(IWebHostEnvironment env)
// {
//     if (env.IsDevelopment())
//     {
// 			//CodeGenValidators.Init(env.ContentRootPath, "/Validators/", true);
// 
// 			// CodeGenClassesFromSqlTables.Init(env.ContentRootPath, _dbConnString); 
// 			// CodeGenFromSql.Init(env.ContentRootPath, _dbConnString);
// 			// CodeGenAdapters.Init(env.ContentRootPath);
// 			// CodeGenCssMap.Init(env.ContentRootPath);
// 			// CodeGenSqlRowInsert.Init(env.ContentRootPath);
// 			// CodeGenSchema.Init(env.ContentRootPath);
// 	   }
// }