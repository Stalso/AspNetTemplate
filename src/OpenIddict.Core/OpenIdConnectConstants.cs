using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenIddict
{
    public class OpenIdConnectLocalConstants
    {
        public static class Destinations
        {
            public const string AccessToken = "access_token";
            public const string IdentityToken = "id_token";
        }
        public static class Properties
        {
            public const string Audiences = "audiences";
            public const string Confidential = "confidential";
            public const string Destinations = "destinations";
            public const string Nonce = "nonce";
            public const string Presenters = "presenters";
            public const string RedirectUri = "redirect_uri";
            public const string Resources = "resources";
            public const string Scopes = "scopes";
            public const string Usage = "usage";
        }
    }
}
