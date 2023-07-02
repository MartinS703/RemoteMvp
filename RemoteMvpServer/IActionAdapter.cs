namespace RemoteMvpLib
{
    public interface IActionAdapter
    {
        Task<IActionResponse> PerformActionAsync(IRequest request);
    }
}