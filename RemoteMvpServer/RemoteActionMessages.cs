

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

    public class RemoteActionResponse
    {
        public ResponseType Type { get; set; }

        public string? Message { get; set; }

        public RemoteActionResponse(ResponseType type, string? message)
        {
            Type = type;
            Message = message;
        }
    }
    public class RemoteExtendedActionResponse
    {
        public ResponseType Type { get; }
        public string? Message { get; }
        public string NewToken { get; }
        public bool AdminVerfied { get; }

        public RemoteExtendedActionResponse(ResponseType type, string? message, string newToken , bool adminVerfied)
        {
            Type = type;
            Message = message;
            NewToken = newToken;
            AdminVerfied = adminVerfied;
        }
    }
}
