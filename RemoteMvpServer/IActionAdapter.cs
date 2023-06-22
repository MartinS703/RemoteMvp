namespace RemoteMvpLib
{
    public interface IActionAdapter
    {
        Task<RemoteActionResponse> PerformActionAsync(RemoteFirstRequest request);
    }
}