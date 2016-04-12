using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Template.OpenIdConnect
{
    public class IdentityApplication : IdentityApplication<string>
    {

    }
    public class IdentityApplication<TKey> where TKey : IEquatable<TKey>
    {
        public virtual TKey Id { get; set; }
        public string DisplayName { get; set; }
        public string RedirectUri { get; set; }
        public string LogoutRedirectUri { get; set; }
        public string Secret { get; set; }
        public string Type { get; set; }
    }
}
