using System;
using Microsoft.AspNet.SignalR;
using Server.Identity.Hubs;

namespace Server.Identity.Helper
{
    public static class B2BSignalrEventHandlers
    {
        public static B2BSignalrEvents B2BSignalrEvents { get; set; }
        static B2BSignalrEventHandlers()
        {
                B2BSignalrEvents=new B2BSignalrEvents();
            B2BSignalrEvents.ChatReceived += B2BSignalrEvents_ChatReceived;

        }

        private static void B2BSignalrEvents_ChatReceived(object sender, EventArgs e)
        {
            ChatEventArgs chatArgs = (ChatEventArgs) e;
            IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<MyHub>();
            //var connections = ConnectionHandler.Connections.GetConnections(chatArgs.ReceiverUsername).ToList();

            //hubContext.Clients.Clients(connections).chatReceived(chatArgs.SenderUsername,chatArgs.Message);
        }    

    }
}