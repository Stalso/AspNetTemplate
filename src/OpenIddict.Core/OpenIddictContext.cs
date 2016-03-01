using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using OpenIddict.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenIddict.Core
{
    public class OpenIddictContext<TUser, TApplication, TRole, TKey> : IdentityDbContext<TUser, TRole, TKey>
        where TUser : IdentityUser<TKey>
        where TApplication : Application
        where TRole : IdentityRole<TKey>
        where TKey : IEquatable<TKey>
    {
        public OpenIddictContext() { }

        public OpenIddictContext(DbContextOptions options)
            : base(options)
        { }

        public OpenIddictContext(IServiceProvider services)
            : base(services)
        { }

        public OpenIddictContext(IServiceProvider services, DbContextOptions options)
            : base(services, options)
        { }

        public DbSet<TApplication> Applications { get; set; }
    }
}
