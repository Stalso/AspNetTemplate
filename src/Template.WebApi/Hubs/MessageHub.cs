using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Principal;

namespace Template.WebApi.Hubs
{
    public class JwtAuthorizeAttribute : AuthorizeAttribute
    {
        public override bool AuthorizeHubConnection(HubDescriptor hubDescriptor, IRequest request)
        {
            return base.AuthorizeHubConnection(hubDescriptor, request);
        }
        public override bool AuthorizeHubMethodInvocation(IHubIncomingInvokerContext hubIncomingInvokerContext, bool appliesToMethod)
        {
            return base.AuthorizeHubMethodInvocation(hubIncomingInvokerContext, appliesToMethod);
        }
        protected override bool UserAuthorized(IPrincipal user)
        {
            //var res = base.UserAuthorized(user);
            //if (!res)
            //{
            //    throw new HttpException("403", "Not authenticated");
            //}
            return base.UserAuthorized(user);
        }

    }
    [HubName("msg")]
    public class MessageHub : Hub
    {

        //public void SendFreeMessage(dynamic message)
        //{
        //    //Clients.Group("auth").authhello(message); 

        //    //this.Clients.Client("free message" + message);
        //    this.Clients.All.sendFreeMessage("free message" + message);
        //}
        public void SendFreeMessage(string message)
        {           
            this.Clients.All.sendFreeMessage("free server message " + message);
        }

        [JwtAuthorize]
        public void SendProtMessage(string message)
        {
            this.Clients.All.sendProtectedMessage("prot server message " + message);
        }

        public override Task OnConnected()
        {
            return base.OnConnected();
        }
        public override Task OnDisconnected(bool stopCalled)
        {
            return base.OnDisconnected(stopCalled);
        }
        //protected override async Task OnConnected(HttpRequest request, string connectionId)
        //{
        //    var identity = request.HttpContext.User.Identity;
        //    var status = identity.IsAuthenticated ? "Authenticated" : "Unauthenticated";

        //    Logger.LogInformation($"{status} connection {connectionId} has just connected.");

        //    await Connection.Send(connectionId, $"Connection is {status}");

        //    if (identity.IsAuthenticated)
        //    {
        //        await Connection.Send(connectionId, $"Authenticated username: {identity.Name}");
        //    }
        //}

        //protected override async Task OnReceived(HttpRequest request, string connectionId, string data)
        //{
        //    var identity = request.HttpContext.User.Identity;
        //    var status = identity.IsAuthenticated ? "authenticated" : "unauthenticated";
        //    var name = identity.IsAuthenticated ? identity.Name : "client";

        //    await Connection.Send(connectionId, $"Received an {status} message from {name}: {data}");
        //}
    }
}
