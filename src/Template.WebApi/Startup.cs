using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using AspNet.Security.OpenIdConnect.Extensions;
using Template.WebApi.Extensions;
using NWebsec.Middleware;
using Template.WebApi.Providers;
using Template.WebApi.Models;
using Microsoft.Data.Entity;
using Microsoft.AspNet.Http;

namespace Template.WebApi
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddEntityFramework()
               .AddInMemoryDatabase()
               .AddDbContext<ApplicationContext>(options => {
                   options.UseInMemoryDatabase();
               });

            services.AddAuthentication();
            // Add framework services.
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseIISPlatformHandler();

            app.UseStaticFiles();

            // Create a new branch where the registered middleware will be executed only for API calls.
            app.UseWhen(context => context.Request.Path.StartsWithSegments(new PathString("/api")), branch => {
                branch.UseJwtBearerAuthentication(options => {
                    options.AutomaticAuthenticate = true;
                    options.AutomaticChallenge = true;
                    options.RequireHttpsMetadata = false;

                    options.Audience = "http://localhost:10450/";
                    options.Authority = "http://localhost:10450/";
                });
            });


            // Note: visit https://docs.nwebsec.com/en/4.2/nwebsec/Configuring-csp.html for more information.
            app.UseCsp(options => options.DefaultSources(configuration => configuration.Self())
                                         .ImageSources(configuration => configuration.Self().CustomSources("data:"))
                                         .ScriptSources(configuration => configuration.UnsafeInline())
                                         .StyleSources(configuration => configuration.Self().UnsafeInline()));

            app.UseXContentTypeOptions();

            app.UseXfo(options => options.Deny());

            app.UseXXssProtection(options => options.EnabledWithBlockMode());

            app.UseOpenIdConnectServer(options => {
                options.Provider = new AuthorizationProvider();

                // Note: see AuthorizationController.cs for more
                // information concerning ApplicationCanDisplayErrors.
                options.ApplicationCanDisplayErrors = true;
                options.AllowInsecureHttp = true;

                // Note: by default, tokens are signed using dynamically-generated
                // RSA keys but you can also use your own certificate:
                // options.SigningCredentials.AddCertificate(certificate);
            });

            app.UseMvc();

            app.UseWelcomePage();

            using (var database = app.ApplicationServices.GetService<ApplicationContext>())
            {
                database.Applications.Add(new Application
                {
                    ApplicationID = "myClient",
                    DisplayName = "My client application",
                    RedirectUri = "http://localhost:53507/signin-oidc",
                    LogoutRedirectUri = "http://localhost:53507/",
                    Secret = "secret_secret_secret"
                });

                database.SaveChanges();
            }
        }

        // Entry point for the application.
        public static void Main(string[] args) => Microsoft.AspNet.Hosting.WebApplication.Run<Startup>(args);
    }
}
