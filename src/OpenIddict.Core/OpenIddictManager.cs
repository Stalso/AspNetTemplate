using AspNet.Security.OpenIdConnect.Extensions;
using AspNet.Security.OpenIdConnect.Server;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.OptionsModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace OpenIddict
{
    public class OpenIddictManager<TUser, TApplication> : UserManager<TUser> where TUser : class where TApplication : class
    {
        public OpenIddictManager(
            IOpenIddictStore<TUser, TApplication> store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<TUser> passwordHasher,
            IEnumerable<IUserValidator<TUser>> userValidators,
            IEnumerable<IPasswordValidator<TUser>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<UserManager<TUser>> logger,
            IHttpContextAccessor contextAccessor)
            : base(store, optionsAccessor,
                   passwordHasher, userValidators,
                   passwordValidators, keyNormalizer,
                   errors, services, logger, contextAccessor)
        {
            //Context = services.GetService<IHttpContextAccessor>()?.HttpContext;
            Options = optionsAccessor.Value;
            Context = contextAccessor?.HttpContext;
        }

        /// <summary>
        /// Gets the HTTP context associated with the current manager.
        /// </summary>
        public virtual HttpContext Context { get; }

        /// <summary>
        /// Gets the cancellation token used to abort async operations.
        /// </summary>
        public virtual CancellationToken CancellationToken => Context?.RequestAborted ?? CancellationToken.None;

        /// <summary>
        /// Gets the Identity options associated with the current manager.
        /// </summary>
        public virtual IdentityOptions Options { get; }

        /// <summary>
        /// Gets the store associated with the current manager.
        /// </summary>
        public virtual new IOpenIddictStore<TUser, TApplication> Store
        {
            get { return base.Store as IOpenIddictStore<TUser, TApplication>; }
        }

        public virtual async Task<ClaimsIdentity> CreateIdentityAsync(TUser user, IEnumerable<string> scopes)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (scopes == null)
            {
                throw new ArgumentNullException(nameof(scopes));
            }

            var identity = new ClaimsIdentity(
                OpenIdConnectServerDefaults.AuthenticationScheme,
                Options.ClaimsIdentity.UserNameClaimType,
                Options.ClaimsIdentity.RoleClaimType);

            // Note: the name identifier is always included in both identity and
            // access tokens, even if an explicit destination is not specified.
            identity.AddClaim(ClaimTypes.NameIdentifier, await GetUserIdAsync(user));

            // Resolve the username and the email address associated with the user.
            var username = await GetUserNameAsync(user);
            var email = await GetEmailAsync(user);

            // Only add the name claim if the "profile" scope was granted.
            if (scopes.Contains(OpenIdConnectConstants.Scopes.Profile))
            {
                // Throw an exception if the username corresponds to the registered
                // email address and if the "email" scope has not been requested.
                if (!scopes.Contains(OpenIdConnectConstants.Scopes.Email) &&
                     string.Equals(username, email, StringComparison.OrdinalIgnoreCase))
                {
                    throw new InvalidOperationException("The 'email' scope is required.");
                }

                identity.AddClaim(ClaimTypes.Name, username,
                    OpenIdConnectLocalConstants.Destinations.AccessToken,
                    OpenIdConnectLocalConstants.Destinations.IdentityToken);
                //OpenIdConnectLocalExtensions.AddClaim()
            }

            // Only add the email address if the "email" scope was granted.
            if (scopes.Contains(OpenIdConnectConstants.Scopes.Email))
            {
                identity.AddClaim(ClaimTypes.Email, email,
                    OpenIdConnectLocalConstants.Destinations.AccessToken,
                    OpenIdConnectLocalConstants.Destinations.IdentityToken);
            }

            if (SupportsUserRole && scopes.Contains(OpenIddictConstants.Scopes.Roles))
            {
                foreach (var role in await GetRolesAsync(user))
                {
                    identity.AddClaim(identity.RoleClaimType, role,
                    OpenIdConnectLocalConstants.Destinations.AccessToken,
                    OpenIdConnectLocalConstants.Destinations.IdentityToken);
                }
            }

            if (SupportsUserSecurityStamp)
            {
                var identifier = await GetSecurityStampAsync(user);

                if (!string.IsNullOrEmpty(identifier))
                {
                    identity.AddClaim(Options.ClaimsIdentity.SecurityStampClaimType, identifier,
                        OpenIdConnectLocalConstants.Destinations.AccessToken,
                        OpenIdConnectLocalConstants.Destinations.IdentityToken);
                }
            }

            return identity;
        }
    }
}
