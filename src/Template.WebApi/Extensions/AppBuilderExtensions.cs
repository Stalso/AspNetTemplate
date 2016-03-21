using System;
using Microsoft.AspNet.Http;
using System.Collections.Generic;
using Microsoft.AspNet.Builder;
using Microsoft.Owin.Builder;
using Owin;
using System.Threading.Tasks;
namespace Template.WebApi.Extensions {
    using System.Net;
    using AppFunc = Func<IDictionary<string, object>, Task>;
    public static class AppBuilderExtensions {
        public static IApplicationBuilder UseWhen(this IApplicationBuilder app,
            Func<HttpContext, bool> condition, Action<IApplicationBuilder> configuration) {
            if (app == null) {
                throw new ArgumentNullException(nameof(app));
            }

            if (condition == null) {
                throw new ArgumentNullException(nameof(condition));
            }

            if (configuration == null) {
                throw new ArgumentNullException(nameof(configuration));
            }

            var builder = app.New();
            configuration(builder);

            return app.Use(next => {
                builder.Run(next);

                var branch = builder.Build();

                return context => {
                    if (condition(context)) {
                        return branch(context);
                    }

                    return next(context);
                };
            });
        }

        public static IApplicationBuilder UseAppBuilder(
          this IApplicationBuilder app,
          Action<IAppBuilder> configure)
        {
            app.UseOwin(addToPipeline =>
            {
                addToPipeline(next =>
                {
                    var appBuilder = new AppBuilder();
                    appBuilder.Properties["builder.DefaultApp"] = next;

                    configure(appBuilder);

                    return appBuilder.Build<AppFunc>();
                });
            });

            return app;
        }

        public static void UseSignalR2(this IApplicationBuilder app)
        {
            //var listener = (HttpListener)app.Properties[typeof(HttpListener).FullName];
            //listener.AuthenticationSchemes = AuthenticationSchemes.Ntlm;
            //app.UseAppBuilder(appBuilder => appBuilder.MapSignalR());
            app.UseAppBuilder(appBuilder => appBuilder.MapSignalR(new Microsoft.AspNet.SignalR.HubConfiguration() { EnableDetailedErrors = true }));
        }
    }
}