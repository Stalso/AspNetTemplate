using AspNet.Security.OpenIdConnect.Extensions;
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
        public override async Task ValidateClientRedirectUri(ValidateClientRedirectUriContext context)
        {
            var manager = context.HttpContext.RequestServices.GetRequiredService<OpenIddictManager<TUser, TApplication>>();

            if (string.IsNullOrEmpty(context.RedirectUri))
            {
                context.Skipped();

                return;
            }

            // Retrieve the application details corresponding to the requested client_id.
            var application = await manager.FindApplicationByIdAsync(context.ClientId);

            if (application == null)
            {
                context.Rejected(
                    error: "invalid_client",
                    description: "Application not found in the database: ensure that your client_id is correct");

                return;
            }

            if (!string.IsNullOrEmpty(context.RedirectUri))
            {
               
                if (!await manager.ValidateRedirectUriAsync(application,context.RedirectUri))
                {
                    context.Rejected(error: "invalid_client", description: "Invalid redirect_uri");

                    return;
                }
            }

            context.Validated(context.RedirectUri);
        }

        public override async Task ValidateClientLogoutRedirectUri(ValidateClientLogoutRedirectUriContext context)
        {
            //var database = context.HttpContext.RequestServices.GetRequiredService<ApplicationDbContext>();

            //// Note: ValidateClientLogoutRedirectUri is not invoked when post_logout_redirect_uri is null.
            //// When provided, post_logout_redirect_uri must exactly match the address registered by the client application.
            //if (!await database.Applications.AnyAsync(application => application.LogoutRedirectUri == context.PostLogoutRedirectUri))
            //{
            //    context.Rejected(error: "invalid_client", description: "Invalid post_logout_redirect_uri");

            //    return;
            //}

            //context.Validated();

            var manager = context.HttpContext.RequestServices.GetRequiredService<OpenIddictManager<TUser, TApplication>>();

            // Skip validation if the optional post_logout_redirect_uri
            // parameter was missing from the logout request.
            if (string.IsNullOrEmpty(context.PostLogoutRedirectUri))
            {
                context.Skipped();

                return;
            }

            var application = await manager.FindApplicationByLogoutRedirectUri(context.PostLogoutRedirectUri);
            if (application == null)
            {
                context.Rejected(
                    error: OpenIdConnectConstants.Errors.InvalidClient,
                    description: "Invalid post_logout_redirect_uri.");

                return;
            }

            context.Validated();
        }
    }
}
