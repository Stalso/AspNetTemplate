using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Template.OpenIdConnect
{
    public static class ApplicationTypes
    {
        public const string Confidential = "confidential";
        public const string Public = "public";
    }
    public static class Destinations
    {
        //public const string AccessToken = "access_token";
        public const string IdentityToken = "id_token";
        //TODO rename in rc2
        public const string AccessToken = "token";
    }
    public static class Claims
    {
        public const string Roles = "roles";
    }

    public static class Scopes
    {
        public const string Roles = "roles";
    }
    public static class Properties
    {
        public const string Audiences = "audiences";
        public const string Confidential = "confidential";
        public const string Destinations = "destinations";
        public const string Destination = "destination";
        public const string Nonce = "nonce";
        public const string Presenters = "presenters";
        public const string RedirectUri = "redirect_uri";
        public const string Resources = "resources";
        public const string Scopes = "scopes";
        public const string Usage = "usage";
    }
}
