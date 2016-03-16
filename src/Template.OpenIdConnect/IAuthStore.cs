using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using System.Threading;

namespace Template.OpenIdConnect
{
    public interface IAuthStore<TUser, TApplication> : IUserStore<TUser> where TUser : class where TApplication : class 
    {
        Task<TApplication> FindApplicationByIdAsync(string identifier, CancellationToken cancellationToken);
        Task<TApplication> FindApplicationByLogoutRedirectUri(string urln, CancellationToken cancellationToken);
        Task<string> GetApplicationTypeAsync(TApplication application, CancellationToken cancellationToken);
        Task<string> GetDisplayNameAsync(TApplication application, CancellationToken cancellationToken);
        Task<string> GetRedirectUriAsync(TApplication application, CancellationToken cancellationToken);
        Task<string> GetLogoutRedirectUriAsync(TApplication application, CancellationToken cancellationToken);
        Task<string> GetHashedSecretAsync(TApplication application, CancellationToken cancellationToken);        
    }
}
