using Microsoft.AspNetCore.SignalR;
using SMEConnectSignalRServer.Interfaces;

namespace SMEConnectSignalRServer.Models
{
    public class ChatHub : Hub<IChatClient>
    {
        public async Task SendMessage(string user, string message)
            => await Clients.All.ReceiveMessage(user, message);

        public async Task SendMessageToCaller(string user, string message)
            => await Clients.Caller.ReceiveMessage(user, message);

        public async Task SendMessageToGroup(string user, string message)
            => await Clients.Group("SignalR Users").ReceiveMessage(user, message);
    }
}
