using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Template.OpenIdConnect.EntityFramework
{
    public class AuthStore<TUser, TApplication, TRole, TContext, TKey> : UserStore<TUser, TRole, TContext, TKey>, IAuthStore<TUser, TApplication>
       where TUser : IdentityUser<TKey>
       where TApplication : IdentityApplication<TKey>
       where TRole : IdentityRole<TKey>
       where TContext : DbContext
       where TKey : IEquatable<TKey>
    {
        public AuthStore(TContext context)
            : base(context)
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
