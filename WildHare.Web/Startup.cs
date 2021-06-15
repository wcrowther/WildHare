using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WildHare.Web.Models;

namespace WildHare.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddSingleton(Configuration.GetSection("App").Get<AppSettings>());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseMvc();

            RunCodeGen(env);
        }

        private void RunCodeGen(IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                CodeGenCssMap.Init(env);

                //CodeGenAdapters.Init();
                //CodeGenFromSql.Init();
                //CodeGenNewtonFromSql.Init();
                //CodeGenSqlRowInsert.Init();
                //CodeGenSchema.Init();
            }
        }
    }
}
