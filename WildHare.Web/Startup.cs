using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WildHare.Web.Models;

namespace WildHare.Web
{
    public class Startup
    {
        private string _dbConnString = null;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();

            services.AddSingleton(Configuration.GetSection("App").Get<AppSettings>());

            _dbConnString = Configuration["ConnectionStrings:ExampleDB"];
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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

            RunCodeGen(env);
        }

        private void RunCodeGen(IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                // CodeGenCssMap.Init(env.ContentRootPath);
                CodeGenAdapters.Init(env.ContentRootPath);
                // CodeGenFromSql.Init(env.ContentRootPath, _dbConnString);
                // CodeGenClassesFromSqlTables.Init(env.ContentRootPath, _dbConnString);
                // CodeGenSqlRowInsert.Init(env.ContentRootPath);
                // CodeGenSchema.Init(env.ContentRootPath);
            }
        }
    }
}
