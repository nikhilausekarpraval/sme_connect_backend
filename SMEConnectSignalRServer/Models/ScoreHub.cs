using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SMEConnectSignalRServer.Interfaces;
using SMEConnectSignalRServer.Modals;

namespace SMEConnectSignalRServer.Models
{
        public class ScoreHub : Hub<IScoreHub>
        {
            public async Task SendMessage(dynamic message)
                => await Clients.All.ReceiveMessage(message);

            public async Task SendMessageToCaller(dynamic message)
                => await Clients.Caller.ReceiveMessage(message);

            public async Task SendMessageToGroup(dynamic message)
                => await Clients.Group("SignalR Users").ReceiveMessage(message);
        }
    
}
