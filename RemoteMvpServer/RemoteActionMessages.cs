

using System.Security;

namespace RemoteMvpLib
{
    public enum ActionType
    {
        Register,
        RegisterAdmin,
        Login,
        Logout,
        DeleteUser,
        SendUsers,
    }

    public class RemoteFirstRequest
    {
        public ActionType Type { get; }

        public string UserName { get; }

        public string Password { get; }
        public RemoteFirstRequest(ActionType type, string username, string password)
        {
            Type = type;
            UserName = username;
            Password = password;
        }
    }

    public class RemoteActionRequest
    {
        public ActionType Type { get; }

        public string SessionToken { get; }

        public string Instruction { get; }

        public RemoteActionRequest(ActionType type, string sessionToken, string instruction)
        {
            Type = type;
            SessionToken = sessionToken;
            Instruction = instruction;
        }
    }

    public enum ResponseType
    {
        Success,
        Error
    }

    public class RemoteActionResponse : IActionResponse
    {
        public ResponseType Type { get; set; }

        public string? Message { get; set; }

        /// <summary>
        /// Do not use, is null!
        /// </summary>
        public string NewToken { get; }

        /// <summary>
        /// Do not use, no information!
        /// </summary>
        public bool AdminVerified { get; }

        public RemoteActionResponse(ResponseType type, string? message)
        {
            Type = type;
            Message = message;
            NewToken = null;
            AdminVerified = false;
        }
    }
    public class RemoteExtendedActionResponse : IActionResponse
    {
        public ResponseType Type { get; }
        public string? Message { get; }
        public string NewToken { get; }
        public bool AdminVerified { get; }

        public RemoteExtendedActionResponse(ResponseType type, string? message, string newToken , bool adminVerfied)
        {
            Type = type;
            Message = message;
            NewToken = newToken;
            AdminVerified = adminVerfied;
        }
    }
    public interface IActionResponse
    {
        ResponseType Type { get; }
        string? Message { get; }
        string NewToken { get; }
        bool AdminVerified { get; }
    }
}
