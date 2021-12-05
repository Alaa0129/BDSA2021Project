using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using System.IdentityModel.Tokens.Jwt;
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
            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

            // services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
            //     .AddMicrosoftIdentityWebApp(Configuration)
            //         .EnableTokenAcquisitionToCallDownstreamApi(new string[] { Configuration["API:APIScope"] })
            //             .AddInMemoryTokenCaches(); ;

            services.AddMicrosoftIdentityWebAppAuthentication(Configuration)
                    .EnableTokenAcquisitionToCallDownstreamApi(new string[] { Configuration["API:APIScope"] })
                        .AddInMemoryTokenCaches();

            services.Configure<OpenIdConnectOptions>(OpenIdConnectDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters.RoleClaimType = "roles";
            });

            services.AddProjectRemote(Configuration);
            services.AddStudentRemote(Configuration);
            services.AddSupervisorRemote(Configuration);


            services.AddControllersWithViews().
                AddMicrosoftIdentityUI();

            services.AddRazorPages();

            services.AddServerSideBlazor()
                .AddMicrosoftIdentityConsentHandler();

            services.AddSweetAlert2();
            services.AddSyncfusionBlazor();
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
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();


            // Redirects, upon logout, to the fron page
            app.UseRewriter(
                new RewriteOptions().Add(
                    context =>
                    {
                        if (context.HttpContext.Request.Path == "/MicrosoftIdentity/Account/SignedOut")
                        {
                            context.HttpContext.Response.Redirect("/");
                        }
                    }));

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
