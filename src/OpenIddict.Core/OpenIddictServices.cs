using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenIddict.Core
{
    public class OpenIddictServices
    {
        public OpenIddictServices(IServiceCollection services)
        {
            Services = services;
        }

        /// <summary>
        /// Gets or sets the type corresponding to the Application entity.
        /// </summary>
        public Type ApplicationType { get; set; }

        /// <summary>
        /// Gets or sets the type corresponding to the Role entity.
        /// </summary>
        public Type RoleType { get; set; }

        /// <summary>
        /// Gets or sets the type corresponding to the User entity.
        /// </summary>
        public Type UserType { get; set; }

        /// <summary>
        /// Gets the services used by OpenIddict.
        /// </summary>
        public IServiceCollection Services { get; }
    }
}
