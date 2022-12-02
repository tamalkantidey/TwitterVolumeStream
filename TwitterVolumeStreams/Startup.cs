using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TwitterVolumeStreams.Data;
using TwitterVolumeStreams.Service.Implementation;
using TwitterVolumeStreams.Service.Interface;

namespace TwitterVolumeStreams
{
    public class Startup
    {
        private string _contentRootPath = "";
        public Startup(IConfiguration configuration, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            Configuration = configuration;
            _contentRootPath = env.ContentRootPath;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddTransient<ITwitterManager, TwitterManager>();
            services.AddTransient<ITwitterDB, TwitterDB>();



            var config = new ConfigurationBuilder()
            .SetBasePath(_contentRootPath)
            .AddJsonFile("appsettings.json")
            .Build()
            .Get<Config>();

            string conn = config.ConnectionStrings.ConnectionString;
            //if (conn.Contains("%CONTENTROOTPATH%"))
            //{
            //    config.ConnectionStrings.ConnectionString = conn.Replace("%CONTENTROOTPATH%", _contentRootPath);
            //}

            //var jsonWriteOptions = new JsonSerializerOptions()
            //{
            //    WriteIndented = true
            //};
            //jsonWriteOptions.Converters.Add(new JsonStringEnumConverter());

            //var newJson = JsonSerializer.Serialize(config, jsonWriteOptions);

            //var appSettingsPath = Path.Combine(_contentRootPath, "appsettings.json");
            //File.WriteAllText(appSettingsPath, newJson);
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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
