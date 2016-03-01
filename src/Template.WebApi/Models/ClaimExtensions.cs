using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Template.WebApi.Models
{
    public static class ClaimExtensions
    {
        /// <summary>
        /// Adds specific destinations to a claim.
        /// </summary>
        /// <param name="claim">The <see cref="Claim"/> instance.</param>
        /// <param name="destinations">The destinations.</param>
        public static Claim SetDestinations(this Claim claim, IEnumerable<string> destinations)
        {
            if (claim == null)
            {
                throw new ArgumentNullException(nameof(claim));
            }

            if (destinations == null || !destinations.Any())
            {
                claim.Properties.Remove(Properties.Destinations);

                return claim;
            }

            if (destinations.Any(destination => destination.Contains(" ")))
            {
                throw new ArgumentException("Destinations cannot contain spaces.", nameof(destinations));
            }

            claim.Properties[Properties.Destinations] =
                string.Join(" ", destinations.Distinct(StringComparer.Ordinal));

            return claim;
        }

        /// <summary>
        /// Adds specific destinations to a claim.
        /// </summary>
        /// <param name="claim">The <see cref="Claim"/> instance.</param>
        /// <param name="destinations">The destinations.</param>
        public static Claim SetDestinations(this Claim claim, params string[] destinations)
        {
            // Note: guarding the destinations parameter against null values
            // is not necessary as AsEnumerable() doesn't throw on null values.
            return claim.SetDestinations(destinations.AsEnumerable());
        }
        /// <summary>
        /// Adds a claim to a given identity and specify one or more destinations.
        /// </summary>
        /// <param name="identity">The identity.</param>
        /// <param name="type">The type associated with the claim.</param>
        /// <param name="value">The value associated with the claim.</param>
        /// <param name="destinations">The destinations associated with the claim.</param>
        public static ClaimsIdentity AddClaim(this ClaimsIdentity identity,
            string type, string value, params string[] destinations)
        {
            // Note: guarding the destinations parameter against null values
            // is not necessary as AsEnumerable() doesn't throw on null values.
            return identity.AddClaim(type, value, destinations.AsEnumerable());
        }

        /// <summary>
        /// Adds a claim to a given identity and specify one or more destinations.
        /// </summary>
        /// <param name="identity">The identity.</param>
        /// <param name="type">The type associated with the claim.</param>
        /// <param name="value">The value associated with the claim.</param>
        /// <param name="destinations">The destinations associated with the claim.</param>
        public static ClaimsIdentity AddClaim(this ClaimsIdentity identity,
            string type, string value, IEnumerable<string> destinations)
        {
            if (identity == null)
            {
                throw new ArgumentNullException(nameof(identity));
            }

            if (destinations == null)
            {
                throw new ArgumentNullException(nameof(destinations));
            }

            identity.AddClaim(new Claim(type, value).SetDestinations(destinations));
            return identity;
        }
    }
}


