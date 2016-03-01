﻿using AspNet.Security.OpenIdConnect.Extensions;
using AspNet.Security.OpenIdConnect.Server;
using Microsoft.AspNet.Authentication;
using Microsoft.AspNet.Http.Authentication;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OpenIddict
{
    public partial class OpenIddictProvider<TUser, TApplication> : OpenIdConnectServerProvider where TUser : class where TApplication : class
    {
        public override async Task ValidateAuthorizationRequest(ValidateAuthorizationRequestContext context)
        {
            var manager = context.HttpContext.RequestServices.GetRequiredService<OpenIddictManager<TUser, TApplication>>();

            // TODO uncomment this in rc2
            //// Note: redirect_uri is not required for pure OAuth2 requests
            //// but this provider uses a stricter policy making it mandatory,
            //// as required by the OpenID Connect core specification.
            //// See http://openid.net/specs/openid-connect-core-1_0.html#AuthRequest.
            //if (string.IsNullOrEmpty(context.RedirectUri))
            //{
            //    context.Rejected(
            //        error: OpenIdConnectConstants.Errors.InvalidRequest,
            //        description: "The required redirect_uri parameter was missing.");

            //    return;
            //}

            // Retrieve the application details corresponding to the requested client_id.
            var application = await manager.FindApplicationByIdAsync(context.ClientId);
            if (application == null)
            {
                context.Rejected(
                    error: OpenIdConnectConstants.Errors.InvalidClient,
                    description: "Application not found in the database: ensure that your client_id is correct.");

                return;
            }

            // TODO uncomment this in rc2
            //if (!await manager.ValidateRedirectUriAsync(application, context.RedirectUri))
            //{
            //    context.Rejecet(
            //        error: OpenIdConnectConstants.Errors.InvalidClient,
            //        description: "Invalid redirect_uri.");

            //    return;
            //}

            // To prevent downgrade attacks, ensure that authorization requests using the hybrid/implicit
            // flow are rejected if the client identifier corresponds to a confidential application.
            // Note: when using the authorization code grant, ValidateClientAuthentication is responsible of
            // rejecting the token request if the client_id corresponds to an unauthenticated confidential client.
            if (await manager.IsConfidentialApplicationAsync(application) && !context.Request.IsAuthorizationCodeFlow())
            {
                context.Rejected(
                    error: OpenIdConnectConstants.Errors.InvalidRequest,
                    description: "Confidential clients can only use response_type=code.");

                return;
            }

            // If the user is connected, ensure that a corresponding profile exists and that
            // the appropriate set of scopes is requested to prevent personal data leakage.
            if (context.HttpContext.User.Identities.Any(identity => identity.IsAuthenticated))
            {
                // Ensure the user profile still exists in the database.
                // TODO uncomment this in rc2
                //var user = await manager.GetUserAsync(context.HttpContext.User);
                // TODO replace
                var user = await manager.FindByNameAsync(context.HttpContext.User.Identity.Name);

                if (user == null)
                {
                    context.Rejected(
                        error: OpenIdConnectConstants.Errors.ServerError,
                        description: "An internal error has occurred.");

                    return;
                }

                // Return an error if the username corresponds to the registered
                // email address and if the "email" scope has not been requested.
                // TODO uncomment this in rc2
                //if (context.Request.HasScope(OpenIdConnectConstants.Scopes.Profile) &&
                //   !context.Request.HasScope(OpenIdConnectConstants.Scopes.Email) &&
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
            }

            // Run additional checks for prompt=none requests.
            if (string.Equals(context.Request.Prompt, "none", StringComparison.Ordinal))
            {
                // If the user is not authenticated, return an error to the client application.
                // See http://openid.net/specs/openid-connect-core-1_0.html#Authenticates
                if (!context.HttpContext.User.Identities.Any(identity => identity.IsAuthenticated))
                {
                    context.Rejected(
                        error: OpenIdConnectConstants.Errors.LoginRequired,
                        description: "The user must be authenticated.");

                    return;
                }

                // Extract the principal contained in the id_token_hint parameter.
                // If no principal can be extracted, an error is returned to the client application.
                var principal = await context.HttpContext.Authentication.AuthenticateAsync(context.Options.AuthenticationScheme);
                if (principal == null)
                {
                    context.Rejected(
                        error: OpenIdConnectConstants.Errors.InvalidRequest,
                        description: "The required id_token_hint parameter is missing.");

                    return;
                }

                // Ensure the client application is listed as a valid audience in the identity token
                // and that the identity token corresponds to the authenticated user.
                if (!principal.HasClaim(JwtRegisteredClaimNames.Aud, context.Request.ClientId) ||
                    !principal.HasClaim(ClaimTypes.NameIdentifier, context.HttpContext.User.GetClaim(ClaimTypes.NameIdentifier)))
                {
                    context.Rejected(
                        error: OpenIdConnectConstants.Errors.InvalidRequest,
                        description: "The id_token_hint parameter is invalid.");

                    return;
                }
            }

            context.Validated();
        }

        public override async Task AuthorizationEndpoint(AuthorizationEndpointContext context)
        {
            // Only handle prompt=none requests at this stage.
            if (!string.Equals(context.Request.Prompt, "none", StringComparison.Ordinal))
            {
                return;
            }

            var manager = context.HttpContext.RequestServices.GetRequiredService<OpenIddictManager<TUser, TApplication>>();

            // Note: principal is guaranteed to be non-null since ValidateAuthorizationRequest
            // rejects prompt=none requests missing or having an invalid id_token_hint.
            var principal = await context.HttpContext.Authentication.AuthenticateAsync(context.Options.AuthenticationScheme);


            // Note: user may be null if the user was removed after
            // the initial check made by ValidateAuthorizationRequest.
            // In this case, ignore the prompt=none request and
            // continue to the next middleware in the pipeline.
            // TODO uncomment this in rc2
            //var user = await manager.GetUserAsync(context.HttpContext.User);
            // TODO replace
            var user = await manager.FindByNameAsync(context.HttpContext.User.Identity.Name);
            if (user == null)
            {
                return;
            }

            // Note: filtering the username is not needed at this stage as OpenIddictController.Accept
            // and OpenIddictProvider.GrantResourceOwnerCredentials are expected to reject requests that
            // don't include the "email" scope if the username corresponds to the registed email address.
            var identity = await manager.CreateIdentityAsync(user, context.Request.GetScopes());


            // Create a new authentication ticket holding the user identity.
            var ticket = new AuthenticationTicket(
                new ClaimsPrincipal(identity),
                new AuthenticationProperties(),
                context.Options.AuthenticationScheme);

            ticket.SetResources(context.Request.GetResources());
            ticket.SetScopes(context.Request.GetScopes());

            // Call SignInAsync to create and return a new OpenID Connect response containing the serialized code/tokens.
            await context.HttpContext.Authentication.SignInAsync(ticket.AuthenticationScheme, ticket.Principal, ticket.Properties);

            // Mark the response as handled
            // to skip the rest of the pipeline.
            context.HandleResponse();
        }
    }
}
