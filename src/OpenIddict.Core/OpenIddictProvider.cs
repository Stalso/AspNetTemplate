﻿using AspNet.Security.OpenIdConnect.Extensions;
using AspNet.Security.OpenIdConnect.Server;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OpenIddict.Core
{
    public partial class OpenIddictProvider<TUser, TApplication> : OpenIdConnectServerProvider where TUser : class where TApplication : class
    {
        public override Task MatchEndpoint(MatchEndpointContext context)
        {
            // Note: by default, OpenIdConnectServerHandler only handles authorization requests made to AuthorizationEndpointPath.
            // This context handler uses a more relaxed policy that allows extracting authorization requests received at
            // /connect/authorize/accept and /connect/authorize/deny (see OpenIddictController.cs for more information).
            if (context.Options.AuthorizationEndpointPath.HasValue &&
                context.Request.Path.StartsWithSegments(context.Options.AuthorizationEndpointPath))
            {
                context.MatchesAuthorizationEndpoint();
            }

            return Task.FromResult<object>(null);
        }

        public override async Task ProfileEndpoint(ProfileEndpointContext context)
        {
            var manager = context.HttpContext.RequestServices.GetRequiredService<OpenIddictManager<TUser, TApplication>>();
            //TODO rc2
            var principal = context.AuthenticationTicket?.Principal;


            // Note: user may be null if the user has been removed.
            // In this case, return a 400 response.
            // TODO rc2
            //var user = await manager.GetUserAsync(principal);
            var user = await manager.FindByNameAsync(principal.Identity.Name);
            if (user == null)
            {
                context.Response.StatusCode = 400;
                context.HandleResponse();

                return;
            }

            // Note: "sub" is a mandatory claim.
            // See http://openid.net/specs/openid-connect-core-1_0.html#UserInfoResponse
            context.Subject = await manager.GetUserIdAsync(user);

            // Only add the "preferred_username" claim if the "profile" scope was present in the access token.
            // Note: filtering the username is not needed at this stage as OpenIddictController.Accept
            // and OpenIddictProvider.GrantResourceOwnerCredentials are expected to reject requests that
            // don't include the "email" scope if the username corresponds to the registed email address.
            // TODO rc2
            //if (context.Ticket.HasScope(OpenIdConnectConstants.Scopes.Profile))
            if (context.AuthenticationTicket.ContainsScope(OpenIdConnectConstants.Scopes.Profile))
            {
                context.PreferredUsername = await manager.GetUserNameAsync(user);

                if (manager.SupportsUserClaim)
                {
                    context.FamilyName = await manager.FindClaimAsync(user, ClaimTypes.Surname);
                    context.GivenName = await manager.FindClaimAsync(user, ClaimTypes.GivenName);
                    context.BirthDate = await manager.FindClaimAsync(user, ClaimTypes.DateOfBirth);
                }
            }

            // Only add the email address details if the "email" scope was present in the access token.
            // TODO rc2
            //if (context.Ticket.HasScope(OpenIdConnectConstants.Scopes.Email))
            if (context.AuthenticationTicket.ContainsScope(OpenIdConnectConstants.Scopes.Email))
            {
                context.Email = await manager.GetEmailAsync(user);

                // Only add the "email_verified" claim
                // if the email address is non-null.
                if (!string.IsNullOrEmpty(context.Email))
                {
                    context.EmailVerified = await manager.IsEmailConfirmedAsync(user);
                }
            };

            // Only add the phone number details if the "phone" scope was present in the access token.
            // TODO rc2
            //if (context.Ticket.HasScope(OpenIdConnectConstants.Scopes.Phone))
            if (context.AuthenticationTicket.ContainsScope(OpenIdConnectConstants.Scopes.Phone))
            {
                context.PhoneNumber = await manager.GetPhoneNumberAsync(user);

                // Only add the "phone_number_verified"
                // claim if the phone number is non-null.
                if (!string.IsNullOrEmpty(context.PhoneNumber))
                {
                    context.PhoneNumberVerified = await manager.IsPhoneNumberConfirmedAsync(user);
                }
            }
            // TODO rc2
            // Only add the roles list if the "roles" scope was present in the access token.
            //if (manager.SupportsUserRole && context.Ticket.HasScope(OpenIddictConstants.Scopes.Roles))
            if (manager.SupportsUserRole && context.AuthenticationTicket.ContainsScope(OpenIddictLocalConstants.Scopes.Roles))
            {
                var roles = await manager.GetRolesAsync(user);
                if (roles.Count != 0)
                {
                    context.Claims[OpenIddictLocalConstants.Claims.Roles] = JArray.FromObject(roles);
                }
            }
        }
    }
}
