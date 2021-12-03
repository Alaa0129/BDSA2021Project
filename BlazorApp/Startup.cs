using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CurrieTechnologies.Razor.SweetAlert2;
using Syncfusion.Blazor;

namespace BlazorApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            Uri Api = new Uri("http://localhost:3000");
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddHttpClient("default_client", client => client.BaseAddress = Api);
            services.AddScoped<IProjectRemote, ProjectRemote>();
            services.AddScoped<ITagRemote, TagRemote>();
            services.AddScoped<IRequestRemote, RequestRemote>();
            services.AddSweetAlert2();
            services.AddSyncfusionBlazor();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NTQyOTk1QDMxMzkyZTMzMmUzMFovejRzYVM1M0svUEMwZ1JqckJtKzlrc0hINjJ4cTEwaHJLM0J4M0UrUE09");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
