using AspNet.Security.OpenIdConnect.Extensions;
using AspNet.Security.OpenIdConnect.Server;
using Microsoft.AspNet.Authentication;
using Microsoft.AspNet.Http.Authentication;
using Microsoft.AspNet.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.OptionsModel;
using OpenIddict.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace OpenIddict
{
    public partial class OpenIddictProvider<TUser, TApplication> : OpenIdConnectServerProvider where TUser : class where TApplication : class
    {
        public override async Task ValidateClientAuthentication(ValidateClientAuthenticationContext context)
        {
            // Note: client authentication is not mandatory for non-confidential client applications like mobile apps
            // (except when using the client credentials grant type) but this authorization server uses a safer policy
            // that makes client authentication mandatory and returns an error if client_id or client_secret is missing.
            // You may consider relaxing it to support the resource owner password credentials grant type
            // with JavaScript or desktop applications, where client credentials cannot be safely stored.
            // In this case, call context.Skipped() to inform the server middleware the client is not trusted.
            if (string.IsNullOrEmpty(context.ClientId) || string.IsNullOrEmpty(context.ClientSecret))
            {
                context.Rejected(
                    error: "invalid_request",
                    description: "Missing credentials: ensure that your credentials were correctly " +
                                 "flowed in the request body or in the authorization header");

                return;
            }

            var manager = context.HttpContext.RequestServices.GetRequiredService<OpenIddictManager<TUser, TApplication>>();
           
            // Retrieve the application details corresponding to the requested client_id.
            var application = await manager.FindApplicationByIdAsync(context.ClientId);

            if (application == null)
            {
                context.Rejected(
                    error: "invalid_client",
                    description: "Application not found in the database: ensure that your client_id is correct");

                return;
            }
            
            if (!await manager.ValidateSecretAsync(application, context.ClientSecret))
            {
                context.Rejected(
                    error: "invalid_client",
                    description: "Invalid credentials: ensure that you specified a correct client_secret");

                return;
            }

            context.Validated();
        }

        public override async Task ValidateTokenRequest(ValidateTokenRequestContext context)
        {
            var manager = context.HttpContext.RequestServices.GetRequiredService<OpenIddictManager<TUser, TApplication>>();

            // Note: OpenIdConnectServerHandler supports authorization code, refresh token,
            // client credentials, resource owner password credentials and custom grants
            // but this authorization server uses a stricter policy rejecting custom grant types.
            if (!context.Request.IsAuthorizationCodeGrantType() && !context.Request.IsRefreshTokenGrantType() &&
                !context.Request.IsPasswordGrantType() && !context.Request.IsClientCredentialsGrantType())
            {
                context.Rejected(
                    error: OpenIdConnectConstants.Errors.UnsupportedGrantType,
                    description: "Only authorization code, refresh token, client credentials " +
                                 "and password grants are accepted by this authorization server.");

                return;
            }

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

            // TODO Application cast
            // Retrieve the application details corresponding to the requested client_id.
            var application = await manager.FindApplicationByIdAsync(context.ClientId);
            if (application == null)
            {
                context.Rejected(
                    error: OpenIdConnectConstants.Errors.InvalidClient,
                    description: "Application not found in the database: ensure that your client_id is correct.");

                return;
            }

            // TODO uncomment with rc2
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
            //        context.Reject(
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
            //        context.Reject(
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

            context.Validated();
        }

        public override async Task GrantClientCredentials( GrantClientCredentialsContext context)
        {
            var manager = context.HttpContext.RequestServices.GetRequiredService<OpenIddictManager<TUser, TApplication>>();

            // Retrieve the application details corresponding to the requested client_id.
            var application = await manager.FindApplicationByIdAsync(context.ClientId);
            

            var identity = new ClaimsIdentity(context.Options.AuthenticationScheme);

            // Note: the name identifier is always included in both identity and
            // access tokens, even if an explicit destination is not specified.
            identity.AddClaim(ClaimTypes.NameIdentifier, context.ClientId);

            identity.AddClaim(ClaimTypes.Name, await manager.GetDisplayNameAsync(application),
                OpenIdConnectLocalConstants.Destinations.AccessToken,
                OpenIdConnectLocalConstants.Destinations.IdentityToken);

            // Create a new authentication ticket
            // holding the application identity.
            var ticket = new AuthenticationTicket(
                new ClaimsPrincipal(identity),
                new AuthenticationProperties(),
                context.Options.AuthenticationScheme);

            ticket.SetResources(context.Request.GetResources());
            ticket.SetScopes(context.Request.GetScopes());

            context.Validated(ticket);
        }

        public override async Task GrantRefreshToken(GrantRefreshTokenContext context)
        {
            var manager = context.HttpContext.RequestServices.GetRequiredService<OpenIddictManager<TUser, TApplication>>();
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

            context.Validated(ticket);
        }

        public override async Task GrantResourceOwnerCredentials(GrantResourceOwnerCredentialsContext context)
        {
            var manager = context.HttpContext.RequestServices.GetRequiredService<OpenIddictManager<TUser, TApplication>>();

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

            // Return an error if the username corresponds to the registered
            // email address and if the "email" scope has not been requested.
            //TODO uncommecnt with rc2
            //if (context.Request.HasScope(OpenIdConnectConstants.Scopes.Profile) &&
            //   !context.Request.HasScope(OpenIdConnectConstants.Scopes.Email) &&
            //    string.Equals(await manager.GetUserNameAsync(user),
            //                  await manager.GetEmailAsync(user),
            //                  StringComparison.OrdinalIgnoreCase))
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

            var identity = await manager.CreateIdentityAsync(user, context.Request.GetScopes());
           

            // Create a new authentication ticket holding the user identity.
            var ticket = new AuthenticationTicket(
                new ClaimsPrincipal(identity),
                new AuthenticationProperties(),
                context.Options.AuthenticationScheme);

            ticket.SetResources(context.Request.GetResources());
            ticket.SetScopes(context.Request.GetScopes());

            context.Validated(ticket);
        }
    }
}
