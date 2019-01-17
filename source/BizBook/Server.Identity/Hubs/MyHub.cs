using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Serilog;

namespace Server.Identity.Hubs
{
    using System;
    using System.Linq;
    using System.Security.Claims;
    using System.Security.Principal;

    public static class ConnectionHandler
    {
        // public static HashSet<string> ConnectedIds = new HashSet<string>();
        public static Dictionary<string, string> ConnectedUsers = new Dictionary<string, string>();

        //public static ConnectionMapping<string, ConnectionModel> Connections = new ConnectionMapping<string, ConnectionModel>();
        public static Dictionary<string, ConnectionModel> Connections = new Dictionary<string, ConnectionModel>();

    }

    // [Authorize]
    public class MyHub : Hub
    {
        public static ILogger logger = Log.ForContext<MyHub>();

        public override Task OnConnected()
        {
            Clients.Caller.connected(Context.ConnectionId);

            if (Context.Request.User == null)
            {
                logger.Debug("User is null.");
                return base.OnConnected();
            }

            string identityName = Context.Request.User.Identity.Name;
            string newConnectionEstablished = "New connection established " + Context.ConnectionId + ". Username: " + identityName;
            logger.Debug(newConnectionEstablished);

            using (HttpClient client = new HttpClient())
            {
                string s = Context.Request.Environment["server.RemoteIpAddress"].ToString();
                IPAddress ip;
                bool validIp = IPAddress.TryParse(s, out ip);

                if (validIp)
                {
                    //string req = "http://freegeoip.net/json/" + s;
                    //string result = client.GetStringAsync(req).Result;
                    //string data = "{'Username':" + identityName + " 'ConnectionId': " + Context.ConnectionId + ", 'Location': }" + result;
                    //logger.Debug(data);
                }
            }

            IPrincipal user = this.Context.Request.User;
            ClaimsIdentity claimsIdentity = user.Identity as ClaimsIdentity;
            List<Claim> claims = claimsIdentity.Claims.ToList();

            string nameidentifier = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
            var model = new ConnectionModel()
                            {
                                ConnectionId = Context.ConnectionId,
                                ShopId = claims.First(x => x.Type == "ShopId").Value,
                                UserId = claims.First(x => x.Type == nameidentifier).Value,
                                SessionStarted = DateTime.Now
                            };

            ConnectionHandler.Connections.Add(Context.ConnectionId, model);
            ConnectionHandler.ConnectedUsers.Add(Context.ConnectionId, identityName);
            string msg = $"Total connection " + ConnectionHandler.ConnectedUsers.Keys.Count;
            logger.Debug(msg);
            this.UpdateRedis();
            return base.OnConnected();
        }

        public override Task OnReconnected()
        {
            string reconnected = "Reconnected {0}" + Context.ConnectionId;
            logger.Debug(reconnected);
            string totalConnected = "Total connected {0}" + ConnectionHandler.ConnectedUsers.Count;
            logger.Debug(totalConnected);
            Clients.Caller.connected(Context.ConnectionId);
            return base.OnReconnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            bool containsKey = ConnectionHandler.ConnectedUsers.ContainsKey(Context.ConnectionId);
            if (containsKey)
            {
                string name = ConnectionHandler.ConnectedUsers[Context.ConnectionId];
                ConnectionHandler.ConnectedUsers.Remove(Context.ConnectionId);
                string disconnected = string.Format("Disconnected {0} {1}", name, Context.ConnectionId);
                logger.Debug(disconnected);
            }

            containsKey = ConnectionHandler.Connections.ContainsKey(Context.ConnectionId);
            if (containsKey)
            {
                bool removed = ConnectionHandler.Connections.Remove(this.Context.ConnectionId);
                this.UpdateRedis();
            }

            int userCount = ConnectionHandler.ConnectedUsers.Count;
            string userConnection = string.Format("Total connected user {0}, connection {1}", userCount, ConnectionHandler.ConnectedUsers.Count);
            logger.Debug(userConnection);
            return base.OnDisconnected(stopCalled);
        }

        private void UpdateRedis()
        {
            RedisService.UpdateRedis("Connections", ConnectionHandler.Connections);
        }

    }
}
