using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity;

namespace Template.OpenIdConnect.EntityFramework
{
    /// <summary>
    /// Native repository
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    /// <typeparam name="TApplication"></typeparam>
    /// <typeparam name="TRole"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public class OpenIdConnectDbContext<TUser, TApplication, TRole, TKey> : IdentityDbContext<TUser, TRole, TKey>
        where TUser : IdentityUser<TKey>
        //where TApplication : Application
        where TApplication : class
        where TRole : IdentityRole<TKey>
        where TKey : IEquatable<TKey>
    {
        public OpenIdConnectDbContext() { }

        public OpenIdConnectDbContext(DbContextOptions options)
            : base(options)
        { }

        public OpenIdConnectDbContext(IServiceProvider services)
            : base(services)
        { }

        public OpenIdConnectDbContext(IServiceProvider services, DbContextOptions options)
            : base(services, options)
        { }

        public DbSet<TApplication> Applications { get; set; }
        //TODO remove testData
        //public DbSet<SampleEntity<string>> SampleEntities { get; set; }
    }
}
