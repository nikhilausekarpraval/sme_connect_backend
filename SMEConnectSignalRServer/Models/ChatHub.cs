using Microsoft.AspNetCore.SignalR;
using SMEConnectSignalRServer.Interfaces;
using SMEConnectSignalRServer.Modals;

namespace SMEConnectSignalRServer.Models
{
    public class ChatHub : Hub<IChatClient>
    {
        public async Task SendMessage(Message message)
            => await Clients.All.ReceiveMessage(message);

        public async Task SendMessageToCaller(Message message)
            => await Clients.Caller.ReceiveMessage(message);

        public async Task SendMessageToGroup(Message message)
            => await Clients.Group("SignalR Users").ReceiveMessage(message);
    }
}
