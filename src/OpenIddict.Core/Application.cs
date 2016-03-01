using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenIddict.Models
{
    public class Application
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string RedirectUri { get; set; }
        public string LogoutRedirectUri { get; set; }
        public string Secret { get; set; }
        public string Type { get; set; }
    }
}
