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
        //public override bool AuthorizeHubConnection(HubDescriptor hubDescriptor, IRequest request)
        //{
        //    return base.AuthorizeHubConnection(hubDescriptor, request);
        //}
        //public override bool AuthorizeHubMethodInvocation(IHubIncomingInvokerContext hubIncomingInvokerContext, bool appliesToMethod)
        //{
        //    return base.AuthorizeHubMethodInvocation(hubIncomingInvokerContext, appliesToMethod);
        //}
        //protected override bool UserAuthorized(IPrincipal user)
        //{
        //    //var res = base.UserAuthorized(user);
        //    //if (!res)
        //    //{
        //    //    throw new HttpException("403", "Not authenticated");
        //    //}
        //    return base.UserAuthorized(user);
        //}

    }
    [HubName("msg")]
    //[JwtAuthorize]
    public class MessageHub : Hub
    {

        //public void SendFreeMessage(dynamic message)
        //{
        //    //Clients.Group("auth").authhello(message); 

        //    //this.Clients.Client("free message" + message);
        //    this.Clients.All.sendFreeMessage("free message" + message);
        //}
        private string time {
            get {
                return DateTime.Now.ToLongTimeString();
            }
        }
        public void SendFreeMessage(string message)
        {           
            this.Clients.All.sendFreeMessage(time);
        }

        [JwtAuthorize]
        public void SendProtMessage(string message)
        {
            this.Clients.All.sendProtectedMessage(time);
        }
        [JwtAuthorize(Roles = "Admin")]
        public void SendAdminMessage(string message)
        {
            this.Clients.All.sendAdminMessage(time);
        }

        public override Task OnConnected()
        {
            return base.OnConnected();
        }
        public override Task OnDisconnected(bool stopCalled)
        {
            return base.OnDisconnected(stopCalled);
        }
    }
    [HubName("prot")]
    [JwtAuthorize]
    public class ProtHub : Hub
    {
        private string time
        {
            get
            {
                return DateTime.Now.ToLongTimeString();
            }
        }
        //public void SendFreeMessage(dynamic message)
        //{
        //    //Clients.Group("auth").authhello(message); 

        //    //this.Clients.Client("free message" + message);
        //    this.Clients.All.sendFreeMessage("free message" + message);
        //}
        public void SendFreeMessage(string message)
        {
            this.Clients.All.sendFreeMessage(time);
        }

        //[JwtAuthorize]
        public void SendProtMessage(string message)
        {
            this.Clients.All.sendProtectedMessage(time);
        }
        [JwtAuthorize(Roles = "Admin")]
        public void SendAdminMessage(string message)
        {
            this.Clients.All.sendAdminMessage(time);
        }

        public override Task OnConnected()
        {
            return base.OnConnected();
        }
        public override Task OnDisconnected(bool stopCalled)
        {
            return base.OnDisconnected(stopCalled);
        }
    }
    [HubName("admin")]
    [JwtAuthorize(Roles = "Admin")]
    public class AdminHub : Hub
    {
        private string time
        {
            get
            {
                return DateTime.Now.ToLongTimeString();
            }
        }
        //public void SendFreeMessage(dynamic message)
        //{
        //    //Clients.Group("auth").authhello(message); 

        //    //this.Clients.Client("free message" + message);
        //    this.Clients.All.sendFreeMessage("free message" + message);
        //}
        public void SendFreeMessage(string message)
        {
            this.Clients.All.sendFreeMessage(time);
        }

        //[JwtAuthorize]
        public void SendProtMessage(string message)
        {
            this.Clients.All.sendProtectedMessage(time);
        }
       
        public void SendAdminMessage(string message)
        {
            this.Clients.All.sendAdminMessage(time);
        }

        public override Task OnConnected()
        {
            return base.OnConnected();
        }
        public override Task OnDisconnected(bool stopCalled)
        {
            return base.OnDisconnected(stopCalled);
        }
    }
}
