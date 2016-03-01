using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenIddict
{
    public static class OpenIddictHelpers
    {
        public static async Task<bool> IsConfidentialApplicationAsync<TUser, TApplication>(
            this OpenIddictManager<TUser, TApplication> manager, TApplication application)
            where TUser : class
            where TApplication : class
        {
            if (manager == null)
            {
                throw new ArgumentNullException(nameof(manager));
            }

            if (application == null)
            {
                throw new ArgumentNullException(nameof(application));
            }

            var type = await manager.GetApplicationTypeAsync(application);

            return string.Equals(type, OpenIddictLocalConstants.ApplicationTypes.Confidential, StringComparison.OrdinalIgnoreCase);
        }

        public static async Task<bool> IsPublicApplicationAsync<TUser, TApplication>(
            this OpenIddictManager<TUser, TApplication> manager, TApplication application)
            where TUser : class
            where TApplication : class
        {
            if (manager == null)
            {
                throw new ArgumentNullException(nameof(manager));
            }

            if (application == null)
            {
                throw new ArgumentNullException(nameof(application));
            }

            var type = await manager.GetApplicationTypeAsync(application);

            return string.Equals(type, OpenIddictLocalConstants.ApplicationTypes.Public, StringComparison.OrdinalIgnoreCase);
        }
    }
}
