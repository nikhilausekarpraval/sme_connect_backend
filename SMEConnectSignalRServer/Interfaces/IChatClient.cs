using SMEConnectSignalRServer.Modals;

namespace SMEConnectSignalRServer.Interfaces
{
    public interface IChatClient
    {
        Task ReceiveMessage(Message message);
    }
}
