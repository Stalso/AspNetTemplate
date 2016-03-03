using System;
using System.Linq;
using System.Threading.Tasks;
using AspNet.Security.OpenIdConnect.Extensions;
using AspNet.Security.OpenIdConnect.Server;
using Microsoft.Data.Entity;
using Microsoft.Extensions.DependencyInjection;
using Template.WebApi.Models;
using System.Security.Claims;
using Microsoft.AspNet.Authentication;
using Microsoft.AspNet.Http.Authentication;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json.Schema;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Extensions.OptionsModel;

namespace Template.WebApi.Providers {
    public sealed class AuthorizationProvider : OpenIdConnectServerProvider {
      
        public override async Task ValidateClientAuthentication(ValidateClientAuthenticationContext context) {

            // Note: client authentication is not mandatory for non-confidential client applications like mobile apps
            // (except when using the client credentials grant type) but this authorization server uses a safer policy
            // that makes client authentication mandatory and returns an error if client_id or client_secret is missing.
            // You may consider relaxing it to support the resource owner password credentials grant type
            // with JavaScript or desktop applications, where client credentials cannot be safely stored.
            // In this case, call context.Skipped() to inform the server middleware the client is not trusted.
            if (string.IsNullOrEmpty(context.ClientId))
            {
                context.Rejected(
                    error: "invalid_request",
                    description: "No client_id");

                return;
            }

            //var database = context.HttpContext.RequestServices.GetRequiredService<ApplicationDbContext<ApplicationUser,Application,IdentityRole,string>>();

            //// Retrieve the application details corresponding to the requested client_id.
            //var application = await (from entity in database.Applications
            //                         where entity.Id == context.ClientId
            //                         select entity).SingleOrDefaultAsync(context.HttpContext.RequestAborted);

            //var database = context.HttpContext.RequestServices.GetRequiredService<IAuthStore<ApplicationUser, Application>>();
            var manager = context.HttpContext.RequestServices.GetRequiredService<AuthManager<ApplicationUser, Application>>();


            // Retrieve the application details corresponding to the requested client_id.
            //var application = await database.FindApplicationByIdAsync(context.ClientId, context.HttpContext.RequestAborted);
            var application = await manager.FindApplicationByIdAsync(context.ClientId);
            if (application == null) {
                context.Rejected(
                    error: "invalid_client",
                    description: "Application not found in the database: ensure that your client_id is correct");

                return;
            }

            // Note: client authentication is not mandatory for non-confidential client applications like mobile apps
            // (except when using the client credentials grant type) but this authorization server uses a safer policy
            // that makes client authentication mandatory and returns an error if client_id or client_secret is missing.
            // You may consider relaxing it to support the resource owner password credentials grant type
            // with JavaScript or desktop applications, where client credentials cannot be safely stored.
            // In this case, call context.Skipped() to inform the server middleware the client is not trusted.
            //if (application.Type == ApplicationTypes.Confidential && (string.IsNullOrEmpty(context.ClientSecret)))
            //{
            //    context.Rejected(
            //        error: "invalid_request",
            //        description: "Missing credentials: ensure that your credentials were correctly " +
            //                     "flowed in the request body or in the authorization header");

            //    return;
            //}
            //if (application.Type == ApplicationTypes.Confidential && (string.IsNullOrEmpty(context.ClientSecret)))
            //{
            //    context.Rejected(
            //        error: "invalid_request",
            //        description: "Missing credentials: ensure that your credentials were correctly " +
            //                     "flowed in the request body or in the authorization header");

            //    return;
            //}

            //if (application.Type == ApplicationTypes.Confidential &&  !string.Equals(context.ClientSecret, application.Secret, StringComparison.Ordinal)) {
            //    context.Rejected(
            //        error: "invalid_client",
            //        description: "Invalid credentials: ensure that you specified a correct client_secret");

            //    return;
            //}
            if (await manager.IsConfidentialApplicationAsync(application))
            {
                if(!await manager.ValidateSecretAsync(application,context.ClientSecret))
                    context.Rejected(
                        error: "invalid_client",
                        description: "Invalid credentials: ensure that you specified a correct client_secret");

            }
            context.Validated();
        } 
        //TODO       
        public override async Task ValidateClientRedirectUri(ValidateClientRedirectUriContext context) {
            //var database = context.HttpContext.RequestServices.GetRequiredService<ApplicationDbContext<ApplicationUser, Application, IdentityRole, string>>();

            //// Retrieve the application details corresponding to the requested client_id.
            //var application = await (from entity in database.Applications
            //                         where entity.Id == context.ClientId
            //                         select entity).SingleOrDefaultAsync(context.HttpContext.RequestAborted);

            //var database = context.HttpContext.RequestServices.GetRequiredService<IAuthStore<ApplicationUser, Application>>();
            var manager = context.HttpContext.RequestServices.GetRequiredService<AuthManager<ApplicationUser, Application>>();
            // Retrieve the application details corresponding to the requested client_id.
            //var application = await database.FindApplicationByIdAsync(context.ClientId, context.HttpContext.RequestAborted);
            var application = await manager.FindApplicationByIdAsync(context.ClientId);
            if (application == null) {
                context.Rejected(
                    error: "invalid_client",
                    description: "Application not found in the database: ensure that your client_id is correct");

                return;
            }

            if (!string.IsNullOrEmpty(context.RedirectUri)) {
                if (!string.Equals(context.RedirectUri, application.RedirectUri, StringComparison.Ordinal)) {
                    context.Rejected(error: "invalid_client", description: "Invalid redirect_uri");

                    return;
                }
            }

            context.Validated(application.RedirectUri);
        }
        //ToDO
        public override async Task ValidateClientLogoutRedirectUri(ValidateClientLogoutRedirectUriContext context) {
            //var database = context.HttpContext.RequestServices.GetRequiredService<ApplicationDbContext<ApplicationUser, Application, IdentityRole, string>>();

            //// Note: ValidateClientLogoutRedirectUri is not invoked when post_logout_redirect_uri is null.
            //// When provided, post_logout_redirect_uri must exactly match the address registered by the client application.
            //if (!await database.Applications.AnyAsync(application => application.LogoutRedirectUri == context.PostLogoutRedirectUri)) {
            //    context.Rejected(error: "invalid_client", description: "Invalid post_logout_redirect_uri");

            //    return;
            //}

            //var database = context.HttpContext.RequestServices.GetRequiredService<IAuthStore<ApplicationUser, Application>>();

            //// Retrieve the application details corresponding to the requested client_id.
            //var application = await database.FindApplicationByLogoutRedirectUri(context.PostLogoutRedirectUri, context.HttpContext.RequestAborted);
            //var database = context.HttpContext.RequestServices.GetRequiredService<IAuthStore<ApplicationUser, Application>>();

            var manager = context.HttpContext.RequestServices.GetRequiredService<AuthManager<ApplicationUser, Application>>();
            // Retrieve the application details corresponding to the requested client_id.
            //var application = await database.FindApplicationByIdAsync(context.ClientId, context.HttpContext.RequestAborted);
            var application = await manager.FindApplicationByLogoutRedirectUri(context.PostLogoutRedirectUri);

            if (application == null)
            {
                context.Rejected(error: "invalid_client", description: "Invalid post_logout_redirect_uri");

                return;
            }
            if (!string.IsNullOrEmpty(context.PostLogoutRedirectUri))
            {
                if (!string.Equals(context.PostLogoutRedirectUri, application.LogoutRedirectUri, StringComparison.Ordinal))
                {
                    context.Rejected(error: "invalid_client", description: "Invalid redirect_uri");

                    return;
                }
            }
            context.Validated();
        }
        public override Task MatchEndpoint(MatchEndpointContext context) {
            // Note: by default, OpenIdConnectServerHandler only handles authorization requests made to the authorization endpoint.
            // This context handler uses a more relaxed policy that allows extracting authorization requests received at
            // /connect/authorize/accept and /connect/authorize/deny (see AuthorizationController.cs for more information).
            if (context.Options.AuthorizationEndpointPath.HasValue &&
                context.Request.Path.StartsWithSegments(context.Options.AuthorizationEndpointPath)) {
                context.MatchesAuthorizationEndpoint();
            }

            return Task.FromResult<object>(null);
        }
        public override Task ProfileEndpoint(ProfileEndpointContext context) {
            // Note: by default, OpenIdConnectServerHandler automatically handles userinfo requests and directly
            // writes the JSON response to the response stream. This sample uses a custom ProfileController that
            // handles userinfo requests: context.SkipToNextMiddleware() is called to bypass the default
            // request processing executed by OpenIdConnectServerHandler.
            context.SkipToNextMiddleware();

            return Task.FromResult<object>(null);
        }
        public override async Task ValidateTokenRequest(ValidateTokenRequestContext context) {
            // Note: OpenIdConnectServerHandler supports authorization code, refresh token, client credentials
            // and resource owner password credentials grant types but this authorization server uses a safer policy
            // rejecting the last two ones. You may consider relaxing it to support the ROPC or client credentials grant types.
            if (!context.Request.IsAuthorizationCodeGrantType() && !context.Request.IsRefreshTokenGrantType() && !context.Request.IsPasswordGrantType()) {
                context.Rejected(
                    error: "unsupported_grant_type",
                    description: "Only authorization code and refresh token grant types " +
                                 "are accepted by this authorization server");
            }
            var manager = context.HttpContext.RequestServices.GetRequiredService<AuthManager<ApplicationUser, Application>>();
            // Note: though required by the OpenID Connect specification for the refresh token grant,
            // client authentication is not mandatory for non-confidential client applications in OAuth2.
            // To avoid breaking OAuth2 scenarios, OpenIddict uses a relaxed policy that allows
            // public applications to use the refresh token grant without having to authenticate.
            // See http://openid.net/specs/openid-connect-core-1_0.html#RefreshingAccessToken
            // and https://tools.ietf.org/html/rfc6749#section-6 for more information.

            // Skip client authentication if the client identifier is missing.
            // Note: ASOS will automatically ensure that the calling application
            // cannot use an authorization code or a refresh token if it's not
            // the intended audience, even if client authentication was skipped.
            if (string.IsNullOrEmpty(context.ClientId))
            {
                context.Skipped();

                return;
            }

            // Retrieve the application details corresponding to the requested client_id.
            var application = await manager.FindApplicationByIdAsync(context.ClientId);
            if (application == null)
            {
                context.Rejected(
                    error: OpenIdConnectConstants.Errors.InvalidClient,
                    description: "Application not found in the database: ensure that your client_id is correct.");

                return;
            }
            // Todo uncomment in rc2
            //// Reject tokens requests containing a client_secret if the client application is not confidential.
            //if (await manager.IsPublicApplicationAsync(application) && !string.IsNullOrEmpty(context.ClientSecret))
            //{
            //    context.Rejected(
            //        error: OpenIdConnectConstants.Errors.InvalidRequest,
            //        description: "Public clients are not allowed to send a client_secret.");

            //    return;
            //}

            //// Confidential applications MUST authenticate
            //// to protect them from impersonation attacks.
            //else if (await manager.IsConfidentialApplicationAsync(application))
            //{
            //    if (string.IsNullOrEmpty(context.ClientSecret))
            //    {
            //        context.Rejected(
            //            error: OpenIdConnectConstants.Errors.InvalidClient,
            //            description: "Missing credentials: ensure that you specified a client_secret.");

            //        return;
            //    }

            //    if (!await manager.ValidateSecretAsync(application, context.ClientSecret))
            //    {
            //        context.Reject(
            //            error: OpenIdConnectConstants.Errors.InvalidClient,
            //            description: "Invalid credentials: ensure that you specified a correct client_secret.");

            //        return;
            //    }
            //}
            //return Task.FromResult<object>(null);
        }
        public override async Task GrantResourceOwnerCredentials(GrantResourceOwnerCredentialsContext context)
        {

            #region UserChecking
            var manager = context.HttpContext.RequestServices.GetRequiredService<AuthManager<ApplicationUser, Application>>();

            var user = await manager.FindByNameAsync(context.UserName);
            if (user == null)
            {
                context.Rejected(
                    error: OpenIdConnectConstants.Errors.InvalidGrant,
                    description: "Invalid credentials.");

                return;
            }

            // Ensure the user is not already locked out.
            if (manager.SupportsUserLockout && await manager.IsLockedOutAsync(user))
            {
                context.Rejected(
                    error: OpenIdConnectConstants.Errors.InvalidGrant,
                    description: "Account locked out.");

                return;
            }

            // Ensure the password is valid.
            if (!await manager.CheckPasswordAsync(user, context.Password))
            {
                context.Rejected(
                    error: OpenIdConnectConstants.Errors.InvalidGrant,
                    description: "Invalid credentials.");

                if (manager.SupportsUserLockout)
                {
                    await manager.AccessFailedAsync(user);

                    // Ensure the user is not locked out.
                    if (await manager.IsLockedOutAsync(user))
                    {
                        context.Rejected(
                            error: OpenIdConnectConstants.Errors.InvalidGrant,
                            description: "Account locked out.");
                    }
                }

                return;
            }

            if (manager.SupportsUserLockout)
            {
                await manager.ResetAccessFailedCountAsync(user);
            }

            //Uncommebnt if I want Email
            if (context.Request.ContainsScope(OpenIdConnectConstants.Scopes.Profile) &&
               !context.Request.ContainsScope(OpenIdConnectConstants.Scopes.Email) &&
                string.Equals(await manager.GetUserNameAsync(user),
                              await manager.GetEmailAsync(user),
                              StringComparison.OrdinalIgnoreCase))
            {
                context.Rejected(
                    error: OpenIdConnectConstants.Errors.InvalidRequest,
                    description: "The 'email' scope is required.");

                return;
            }

            #endregion

            var identity = await manager.CreateIdentityAsync(user, context.Request.GetScopes());

            var ticket = new AuthenticationTicket(
               new ClaimsPrincipal(identity),
               new AuthenticationProperties(),
               context.Options.AuthenticationScheme);

            //ticket.SetResources(context.Request.GetResources());
            ticket.SetResources(new[] { "http://localhost:10450/" });
            ticket.SetScopes(context.Request.GetScopes());

            context.Validated(ticket);


        }
        public override async Task GrantRefreshToken(GrantRefreshTokenContext context)
        {
            var manager = context.HttpContext.RequestServices.GetRequiredService<AuthManager<ApplicationUser, Application>>();
            var options = context.HttpContext.RequestServices.GetRequiredService<IOptions<IdentityOptions>>();


            // TODO uncommecnt with rc2
            //var principal = context.Ticket?.Principal;
            // TODO replace
            var principal = context.AuthenticationTicket?.Principal;

            // TODO uncommecnt with rc2
            //var user = await manager.GetUserAsync(principal);
            var user = await manager.FindByNameAsync(principal.Identity.Name);
            if (user == null)
            {
                context.Rejected(
                    error: OpenIdConnectConstants.Errors.InvalidGrant,
                    description: "The refresh token is no longer valid.");

                return;
            }

            // If the user manager supports security stamps,
            // ensure that the refresh token is still valid.
            if (manager.SupportsUserSecurityStamp)
            {
                var identifier = principal.GetClaim(options.Value.ClaimsIdentity.SecurityStampClaimType);
                if (!string.IsNullOrEmpty(identifier) &&
                    !string.Equals(identifier, await manager.GetSecurityStampAsync(user), StringComparison.Ordinal))
                {
                    context.Rejected(
                        error: OpenIdConnectConstants.Errors.InvalidGrant,
                        description: "The refresh token is no longer valid.");

                    return;
                }
            }

            // Note: the "scopes" property stored in context.AuthenticationTicket is automatically
            // updated by ASOS when the client application requests a restricted scopes collection.
            // TODO uncommecnt with rc2
            //var identity = await manager.CreateIdentityAsync(user, context.Ticket.GetScopes());
            var identity = await manager.CreateIdentityAsync(user, context.AuthenticationTicket.GetScopes());



            // Create a new authentication ticket holding the user identity but
            // reuse the authentication properties stored in the refresh token.
            // TODO uncommecnt with rc2
            //var ticket = new AuthenticationTicket(
            //    new ClaimsPrincipal(identity),
            //    context.Ticket.Properties,
            //    context.Options.AuthenticationScheme);
            var ticket = new AuthenticationTicket(
               new ClaimsPrincipal(identity),
               context.AuthenticationTicket.Properties,
               context.Options.AuthenticationScheme);
            ticket.SetResources(new[] { "http://localhost:10377" });
            context.Validated(ticket);
        }


        #region OldGrants
        public async Task GrantResourceOwnerCredentialsOld1(GrantResourceOwnerCredentialsContext context)
        {
            // Validate the credentials here (e.g using ASP.NET Identity).
            // You can call Rejected() with an error code/description to reject
            // the request and return a message to the caller.
            //UserManager<ApplicationUser> userManager = context.HttpContext.RequestServices.GetRequiredService<UserManager<ApplicationUser>>();
            var userManager = context.HttpContext.RequestServices.GetRequiredService<AuthManager<ApplicationUser, Application>>();
            ApplicationUser user = await userManager.FindByNameAsync(context.UserName);

            var database = context.HttpContext.RequestServices.GetRequiredService<IAuthStore<ApplicationUser, Application>>();
            var application = await database.FindApplicationByIdAsync(context.ClientId, context.HttpContext.RequestAborted);

            if (!(await userManager.CheckPasswordAsync(user, context.Password)))
            {
                context.Rejected(
                     error: "invalid_grant",
                    description: "The user name or password is incorrect."
                    );
                return;
            }

            //var identity = new ClaimsIdentity(OpenIdConnectServerDefaults.AuthenticationScheme);
            var identity = new ClaimsIdentity(context.Options.AuthenticationScheme);

            // Note: the name identifier is always included in both identity and
            // access tokens, even if an explicit destination is not specified.
            identity.AddClaim(ClaimTypes.NameIdentifier, context.ClientId);

            //identity.AddClaim(ClaimTypes.Name, await manager.GetDisplayNameAsync(application),
            //    Destinations.AccessToken,
            //    Destinations.IdentityToken);
            identity.AddClaim(ClaimTypes.Name, application.Id,
                Destinations.AccessToken,
                Destinations.IdentityToken);

            // Create a new authentication ticket
            // holding the application identity.
            var ticket = new AuthenticationTicket(
                new ClaimsPrincipal(identity),
                new AuthenticationProperties(),
                context.Options.AuthenticationScheme);

            ticket.SetResources(context.Request.GetResources());
            ticket.SetScopes(context.Request.GetScopes());

            context.Validated(ticket);
            //OpenIdConnectConstants.Scopes
        }
        public  async Task GrantResourceOwnerCredentials12(GrantResourceOwnerCredentialsContext context)
        {
            // Validate the credentials here (e.g using ASP.NET Identity).
            // You can call Rejected() with an error code/description to reject
            // the request and return a message to the caller.
            UserManager<ApplicationUser> userManager = context.HttpContext.RequestServices.GetRequiredService<UserManager<ApplicationUser>>();
            ApplicationUser user = await userManager.FindByNameAsync(context.UserName);
            if (!(await userManager.CheckPasswordAsync(user, context.Password)))
            {
                context.Rejected(
                     error: "invalid_grant",
                    description: "The user name or password is incorrect."
                    );
                return;
            }



            var identity = new ClaimsIdentity(OpenIdConnectServerDefaults.AuthenticationScheme);
            identity.AddClaim(ClaimTypes.NameIdentifier, "todo");

            // By default, claims are not serialized in the access and identity tokens.
            // Use the overload taking a "destination" to make sure your claims
            // are correctly inserted in the appropriate tokens.
            identity.AddClaim("urn:customclaim", "value", "token id_token");
            identity.AddClaim("urn:customclaim1", "value1", "token access_token");

            var ticket = new AuthenticationTicket(
                new ClaimsPrincipal(identity),
                new AuthenticationProperties(),
                context.Options.AuthenticationScheme);

            // Call SetResources with the list of resource servers
            // the access token should be issued for.
            ticket.SetResources(new[] { "http://localhost:10450/" });

            // Call SetScopes with the list of scopes you want to grant
            // (specify offline_access to issue a refresh token).
            ticket.SetScopes(new[] { "profile", "offline_access" });

            context.Validated(ticket);

            // return null;
        } 
        #endregion
    }
}