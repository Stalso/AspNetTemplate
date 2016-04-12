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
    /// Base class for the Entity Framework database context used for identity.
    /// </summary>
    public class OpenIdConnectDbContext : OpenIdConnectDbContext<IdentityUser, IdentityApplication ,IdentityRole, string> { }



    /// <summary>
    /// Base class for the Entity Framework database context used for identity.
    /// </summary>
    /// <typeparam name="TUser">The type of the user objects.</typeparam>
    public class OpenIdConnectDbContext<TUser> : OpenIdConnectDbContext<TUser, IdentityApplication, IdentityRole, string> where TUser : IdentityUser
    { }

    /// <summary>
    /// Base class for the Entity Framework database context used for identity.
    /// </summary>
    /// <typeparam name="TUser">The type of the user objects.</typeparam>
    public class OpenIdConnectDbContext<TUser, TApplication> : OpenIdConnectDbContext<TUser, TApplication, IdentityRole, string> where TUser : IdentityUser where TApplication : IdentityApplication
    { }

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
        where TApplication : IdentityApplication<TKey>
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

        public DbSet<IdentityApplication<TKey>> Applications { get; set; }
       
    }
}
