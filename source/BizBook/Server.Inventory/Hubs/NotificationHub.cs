using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.SignalR;

namespace Server.Inventory.Hubs
{
    using System.Security.Claims;
    using System.Security.Principal;
    using System.Threading.Tasks;

    using Newtonsoft.Json;

    using Server.Inventory.Helpers;

    public class NotificationHub : Hub
    {

        public static Dictionary<string, ConnectionModel> Connections = new Dictionary<string, ConnectionModel>();

        public void Hello()
        {
            Clients.All.hello();
        }

        public override Task OnConnected()
        {
            Clients.Caller.connected(Context.ConnectionId);
            if (Context.Request.User == null)
            {
                return base.OnConnected();
            }

            string identityName = Context.Request.User.Identity.Name;
            IPrincipal user = this.Context.Request.User;
            ClaimsIdentity claimsIdentity = user.Identity as ClaimsIdentity;
            List<Claim> claims = claimsIdentity.Claims.ToList();

            string nameidentifier = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
            var model = new ConnectionModel();
            model.ConnectionId = this.Context.ConnectionId;
            model.ShopId = claims.FirstOrDefault(x => x.Type == "ShopId") == null ? string.Empty : claims.First(x => x.Type == "ShopId").Value;
            model.UserId = claims.FirstOrDefault(x => x.Type == nameidentifier) == null ? string.Empty : claims.First(x => x.Type == nameidentifier).Value;
            model.UserName = identityName;
            model.SessionStarted = DateTime.Now;

            Connections.Add(Context.ConnectionId, model);
            RedisService.UpdateRedis(RedisKeys.Connections, Connections);
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var containsKey = Connections.ContainsKey(Context.ConnectionId);
            if (containsKey)
            {
                bool removed = Connections.Remove(this.Context.ConnectionId);
                RedisService.UpdateRedis(RedisKeys.Connections, Connections);
            }

            return base.OnDisconnected(stopCalled);
        }

        private void UpdateConnections()
        {
            string value = RedisService.GetValue(RedisKeys.Connections);
            if (string.IsNullOrWhiteSpace(value))
            {
                return;
            }

            var dictionary = JsonConvert.DeserializeObject<Dictionary<string, ConnectionModel>>(value);
            List<string> list = dictionary.Keys.Select(x => x).ToList();
            foreach (var k in list)
            {
                if (!Connections.ContainsKey(k))
                {
                    bool removed = dictionary.Remove(k);
                }
            }

            RedisService.UpdateRedis(RedisKeys.Connections, Connections);
        }
    }
}