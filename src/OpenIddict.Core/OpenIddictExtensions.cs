using AspNet.Security.OpenIdConnect.Server;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OpenIddict.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace OpenIddict.Core
{
    public static class OpenIddictExtensions
    {
        public static IdentityBuilder AddOpenIddict(this IdentityBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            return builder.AddOpenIddictCore<Application>(configuration =>
            {
                // Use the EF adapter by default.
                configuration.UseEntityFramework();
            });
        }
        public static IdentityBuilder AddOpenIddictCore<TApplication>(
           this IdentityBuilder builder,
           Action<OpenIddictServices> configuration)
          where TApplication : class
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            builder.Services.AddAuthentication();
            builder.Services.AddCaching();

            // add provider (Grant resources for example)
            builder.Services.TryAddSingleton(
                typeof(IOpenIdConnectServerProvider),
                typeof(OpenIddictProvider<,>).MakeGenericType(
                    builder.UserType, typeof(TApplication)));

            builder.Services.TryAddScoped(
                typeof(OpenIddictManager<,>).MakeGenericType(
                    builder.UserType, typeof(TApplication)));

            var services = new OpenIddictServices(builder.Services)
            {
                ApplicationType = typeof(TApplication),
                RoleType = builder.RoleType,
                UserType = builder.UserType
            };
            //TODO 
            builder.Services.AddInstance(services);

            configuration(services);

            return builder;
        }


        public static OpenIddictServices UseEntityFramework(this OpenIddictServices services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.Services.AddScoped(
                typeof(IOpenIddictStore<,>).MakeGenericType(services.UserType, services.ApplicationType),
                typeof(OpenIddictStore<,,,,>).MakeGenericType(
                    /* TUser: */ services.UserType,
                    /* TApplication: */ services.ApplicationType,
                    /* TRole: */ services.RoleType,
                    /* TContext: */ ResolveContextType(services),
                    /* TKey: */ ResolveKeyType(services)));

            return services;
        }

        private static Type ResolveContextType(OpenIddictServices services)
        {
            var service = (from registration in services.Services
                           where registration.ServiceType.IsConstructedGenericType
                           let definition = registration.ServiceType.GetGenericTypeDefinition()
                           where definition == typeof(IUserStore<>)
                           select registration.ImplementationType).SingleOrDefault();

            if (service == null)
            {
                throw new InvalidOperationException(
                    "The type of the database context cannot be automatically inferred. " +
                    "Make sure 'AddOpenIddict()' is the last chained call when configuring the services.");
            }

            TypeInfo type;
            for (type = service.GetTypeInfo(); type != null; type = type.BaseType?.GetTypeInfo())
            {
                if (!type.IsGenericType)
                {
                    continue;
                }

                var definition = type.GetGenericTypeDefinition();
                if (definition == null)
                {
                    continue;
                }

                if (definition != typeof(UserStore<,,,>))
                {
                    continue;
                }

                return (from argument in type.AsType().GetGenericArguments()
                        where typeof(DbContext).IsAssignableFrom(argument)
                        select argument).Single();
            }

            throw new InvalidOperationException("The type of the database context cannot be automatically inferred.");
        }

        private static Type ResolveKeyType(OpenIddictServices services)
        {
            TypeInfo type;
            for (type = services.UserType.GetTypeInfo(); type != null; type = type.BaseType?.GetTypeInfo())
            {
                if (!type.IsGenericType)
                {
                    continue;
                }

                var definition = type.GetGenericTypeDefinition();
                if (definition == null)
                {
                    continue;
                }

                if (definition != typeof(IdentityUser<>))
                {
                    continue;
                }

                return type.AsType().GetGenericArguments().Single();
            }

            throw new InvalidOperationException(
                "The type of the key identifier used by the user " +
               $"entity '{services.UserType}' cannot be automatically inferred.");
        }

       
    }
}
