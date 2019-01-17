using System;

namespace Server.Inventory.Helpers
{
    public class ConnectionModel
    {
        public string ConnectionId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string ShopId { get; set; }
        public DateTime SessionStarted { get; set; }
    }
}