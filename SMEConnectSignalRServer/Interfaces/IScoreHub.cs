using SMEConnectSignalRServer.Modals;

namespace SMEConnectSignalRServer.Interfaces
{
    public interface IScoreHub
    {
        Task ReceiveMessage(dynamic message);
    }
}
