using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Template.OpenIdConnect.EntityFramework
{
    /// <summary>
    /// Creates a new instance of a persistence store for users, using the default implementation
    /// of <see cref="IdentityUser{TKey}"/> with a string as a primary key.
    /// </summary>
    public class AuthStore : AuthStore<IdentityUser<string>>
    {
        public AuthStore(DbContext context, IdentityErrorDescriber describer = null) : base(context, describer) { }
    }

    /// <summary>
    /// Creates a new instance of a persistence store for the specified user type.
    /// </summary>
    /// <typeparam name="TUser">The type representing a user.</typeparam>
    public class AuthStore<TUser> : AuthStore<TUser,IdentityApplication, IdentityRole, DbContext>
        where TUser : IdentityUser<string>, new()
    {
        public AuthStore(DbContext context, IdentityErrorDescriber describer = null) : base(context, describer) { }
    }

    /// <summary>
    /// Creates a new instance of a persistence store for the specified user type.
    /// </summary>
    /// <typeparam name="TUser">The type representing a user.</typeparam>
    public class AuthStore<TUser, TApplication> : AuthStore<TUser, TApplication, IdentityRole, DbContext>
        where TUser : IdentityUser<string>, new()
        where TApplication : IdentityApplication<string>, new()
    {
        public AuthStore(DbContext context, IdentityErrorDescriber describer = null) : base(context, describer) { }
    }

    /// <summary>
    /// Creates a new instance of a persistence store for the specified user and role types.
    /// </summary>
    /// <typeparam name="TUser">The type representing a user.</typeparam>
    /// <typeparam name="TRole">The type representing a role.</typeparam>
    /// <typeparam name="TContext">The type of the data context class used to access the store.</typeparam>
    public class AuthStore<TUser, TApplication, TRole, TContext> : AuthStore<TUser, TApplication, TRole, TContext, string>
        where TUser : IdentityUser<string>, new()
        where TApplication : IdentityApplication<string>
        where TRole : IdentityRole<string>, new()
        where TContext : DbContext
    {
        public AuthStore(TContext context, IdentityErrorDescriber describer = null) : base(context, describer) { }
    }

    public class AuthStore<TUser, TApplication, TRole, TContext, TKey> : UserStore<TUser, TRole, TContext, TKey>, IAuthStore<TUser, TApplication>
       where TUser : IdentityUser<TKey>
       where TApplication : IdentityApplication<TKey>
       where TRole : IdentityRole<TKey>
       where TContext : DbContext
       where TKey : IEquatable<TKey>
    {
        public AuthStore(TContext context, IdentityErrorDescriber describer = null)
            : base(context, describer)
        {
        }

        public DbSet<TApplication> Applications
        {
            get { return Context.Set<TApplication>(); }
        }

        public virtual Task<TApplication> FindApplicationByIdAsync(string identifier, CancellationToken cancellationToken)
        {
            // TODO Id problem
            return Applications.SingleOrDefaultAsync(application => application.Id.ToString() == identifier, cancellationToken);
            //return Applications.SingleOrDefaultAsync(application => EqualityComparer<TKey>.Default.Equals(application.Id,identifier), cancellationToken);
        }

        public virtual Task<TApplication> FindApplicationByLogoutRedirectUri(string url, CancellationToken cancellationToken)
        {
            return Applications.SingleOrDefaultAsync(application => application.LogoutRedirectUri == url, cancellationToken);
        }

        public virtual Task<string> GetApplicationTypeAsync(TApplication application, CancellationToken cancellationToken)
        {
            if (application == null)
            {
                throw new ArgumentNullException(nameof(application));
            }

            return Task.FromResult(application.Type);
        }

        public virtual Task<string> GetDisplayNameAsync(TApplication application, CancellationToken cancellationToken)
        {
            if (application == null)
            {
                throw new ArgumentNullException(nameof(application));
            }

            return Task.FromResult(application.DisplayName);
        }

        public virtual Task<string> GetRedirectUriAsync(TApplication application, CancellationToken cancellationToken)
        {
            if (application == null)
            {
                throw new ArgumentNullException(nameof(application));
            }

            return Task.FromResult(application.RedirectUri);
        }
        public virtual Task<string> GetLogoutRedirectUriAsync(TApplication application, CancellationToken cancellationToken)
        {
            if (application == null)
            {
                throw new ArgumentNullException(nameof(application));
            }

            return Task.FromResult(application.LogoutRedirectUri);
        }
        public virtual Task<string> GetHashedSecretAsync(TApplication application, CancellationToken cancellationToken)
        {
            if (application == null)
            {
                throw new ArgumentNullException(nameof(application));
            }

            return Task.FromResult(application.Secret);
        }



    }
}
