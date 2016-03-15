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
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Authentication.JwtBearer;
using System.Security.Claims;
using Microsoft.AspNet.Authentication;
using Microsoft.AspNet.Http.Authentication;
using AspNet.Security.OpenIdConnect.Server;
using Template.Domain.Entities;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Formatters;

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

         
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                        builder =>
                        {
                            builder.AllowCredentials();
                            builder.AllowAnyOrigin();
                            builder.AllowAnyHeader();
                            builder.AllowAnyMethod();
                            builder.Build();
                        });
            });
            
            services.AddEntityFramework()
               .AddInMemoryDatabase()
               .AddDbContext<ApplicationDbContext<ApplicationUser, Application, IdentityRole, string>>(options => {
                   options.UseInMemoryDatabase();
               });
            services.AddScoped<IAuthStore<ApplicationUser,Application>, AuthStore<ApplicationUser, Application, IdentityRole, 
                ApplicationDbContext<ApplicationUser, Application, IdentityRole, string>, string>>();
            services.AddScoped<AuthManager<ApplicationUser, Application>>();
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password = new PasswordOptions()
                {
                    RequiredLength = 1,
                    RequireDigit = false,
                    RequireLowercase = false,
                    RequireUppercase = false,
                    RequireNonLetterOrDigit = false

                };
            }).AddEntityFrameworkStores<ApplicationDbContext<ApplicationUser, Application, IdentityRole, string>>().AddDefaultTokenProviders();
            services.AddAuthorization(options => options.AddPolicy("ElevatedRights", policy =>
                   policy.RequireRole("Admin", "PowerUser", "BackupAdministrator")));
            //services.AddAuthentication();

            // Add framework services.
            services.AddInstance<IConfiguration>(Configuration);
            services.AddCaching();

            //services.AddMvc().Configure<MvcOptions>(options =>
            //{
            //    var jsonOutputFormatter = new JsonOutputFormatter();
            //    jsonOutputFormatter.SerializerSettings.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Objects;

            //    var jsonInputFormatter = new JsonInputFormatter();
            //    jsonInputFormatter.SerializerSettings.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Objects;

            //     options.OutputFormatters.Insert(0, jsonOutputFormatter);
            //    options.InputFormatters.Insert(0, jsonInputFormatter);
            //}); 

            services.AddMvc(options =>
            {
                var jsonOutputFormatter = new JsonOutputFormatter();
                jsonOutputFormatter.SerializerSettings.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Objects;

                var jsonInputFormatter = new JsonInputFormatter();
                jsonInputFormatter.SerializerSettings.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Objects;

                options.OutputFormatters.Insert(0, jsonOutputFormatter);
                options.InputFormatters.Insert(0, jsonInputFormatter);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            app.UseCors("AllowAllOrigins");
            app.UseIISPlatformHandler();
            app.UseDeveloperExceptionPage();
            app.UseDefaultFiles();
            app.UseStaticFiles();

            // Create a new branch where the registered middleware will be executed only for API calls.            

            app.UseWhen(context => context.Request.Path.StartsWithSegments(new PathString("/api")), branch =>
            {
                branch.UseJwtBearerAuthentication(options =>
                {
                  
                    options.AutomaticAuthenticate = true;
                    options.AutomaticChallenge = true;
                    options.RequireHttpsMetadata = false;
                    // Thisi is test, if I uncomment this and SetResource in AuthorizationProvider everything works in postman
                    //options.Audience = "http://localhost:10450/";
                    // My Angular client
                    //options.Audience = "http://localhost:10377";
                    options.TokenValidationParameters.ValidAudiences = new[] { "http://localhost:10450/", "http://localhost:10377/" };
                    //options.
                    // My Api
                    options.Authority = "http://localhost:10450/";
                    
                    //options.TokenValidationParameters.RoleClaimType 
                  
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

            app.UseOpenIdConnectServer(options =>
            {
                
                options.Provider = new AuthorizationProvider();
                // Note: see AuthorizationController.cs for more
                // information concerning ApplicationCanDisplayErrors.
                options.ApplicationCanDisplayErrors = true;
                options.AllowInsecureHttp = true;
                options.AuthorizationEndpointPath = PathString.Empty;
                //options.AuthorizationEndpointPath = "/auth";
                //options.ProfileEndpointPath = "/prof";
                options.TokenEndpointPath = "/token";
                //options.AccessTokenLifetime = new TimeSpan(0, 0, 5);
                //options.RefreshTokenLifetime = new TimeSpan(0, 0, 10);
                //options.Iden
                // Note: by default, tokens are signed using dynamically-generated
                // RSA keys but you can also use your own certificate:
                // options.SigningCredentials.AddCertificate(certificate);
            });
         
            app.UseMvc();
            var hasher = new PasswordHasher<Application>();
            using (var database = app.ApplicationServices.GetService<ApplicationDbContext<ApplicationUser, Application, IdentityRole, string>>())
            {              
                database.Applications.Add(new Application
                {
                    Id = "myPublicClient",
                    DisplayName = "My client application",
                    Type = ApplicationTypes.Public
                });
                database.Applications.Add(new Application
                {
                    Id = "myConfidentialClient",
                    DisplayName = "My client application",
                    Secret = hasher.HashPassword(null, "secret_secret_secret"),
                    Type = ApplicationTypes.Confidential
                });

                database.SampleEntities.Add(new SampleEntity<string>() {
                    Id = "1",
                    Name = "firstEntity",
                    DbData = "firstEntity DbData",
                    Description = "firstEntity Description"
                });
                database.SampleEntities.Add(new SampleEntity<string>()
                {
                    Id = "2",
                    Name = "secondEntity",
                    DbData = "secondEntity DbData",
                    Description = "secondEntity Description"
                });

                database.SaveChanges();
                CreateUser(app).Wait();
            }        
        }
        public async Task CreateUser(IApplicationBuilder app)
        {
            try
            {
                var user = new ApplicationUser { UserName = "admin" };
                var userManager = app.ApplicationServices.GetRequiredService<UserManager<ApplicationUser>>();

                var result = await userManager.CreateAsync(user, "admin");

                var role = new IdentityRole()
                {
                    Name = "Admin",
                };
                var roleManager = app.ApplicationServices.GetRequiredService<RoleManager<IdentityRole>>();
                var roleResult = await roleManager.CreateAsync(role);
                var addToRoleResult = await userManager.AddToRoleAsync(user, "Admin");
            }
            catch (Exception ex)
            {

            }
        }
        // Entry point for the application.
        public static void Main(string[] args) => Microsoft.AspNet.Hosting.WebApplication.Run<Startup>(args);
    }
}
