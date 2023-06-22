using System.Net.Sockets;

namespace RemoteMvpLib
{
    public interface IActionEndpoint
    {
        event EventHandler<RemoteFirstRequest> OnFirstActionPerformed;
        event EventHandler<RemoteActionRequest> OnActionPerformed;
        event EventHandler<string> OnBadRequest;

        void RunActionEndpoint();
        void PerformActionResponse(Socket handler, RemoteActionResponse response);
    }
}