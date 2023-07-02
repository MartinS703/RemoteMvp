using CsvHelper.Configuration.Attributes;
using RemoteMvpLib;
using System.Collections.Concurrent;
using System.Text;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace RemoteMvpApp
{
    internal class ApplicationController
    {
        // concurrent = gleichzeitig, concurrent access from different threads
        private static ConcurrentDictionary<string, string> sessions = new ConcurrentDictionary<string, string>();

        // Model 
        private readonly Userlist _users;

        // ActionEndpoint (to be called by the view)
        private readonly IActionEndpoint _actionEndpoint;       //-> handles requests and responses

        private string _filePath;

        public ApplicationController(IActionEndpoint actionEndpoint)
        {
            _filePath = "myFilepath.csv";           // Change if custom filepath from user

            // Create new Model
            _users = new Userlist(_filePath);

            // Link ActionEndpoint to local method
            _actionEndpoint = actionEndpoint;
            _actionEndpoint.OnFirstActionPerformed += EndpointOnActionPerformed;
            _actionEndpoint.OnActionPerformed += EndpointOnExtendedActionPerformed;
        }


        public void RunActionEndPoint() => _actionEndpoint.RunActionEndpoint();

        public Task RunActionEndPointAsync()
        {
            var task = new Task(_actionEndpoint.RunActionEndpoint);
            task.Start();
            return task;
        }

        /// <summary>
        /// Handles Login and Register requests
        /// </summary>
        /// <param name="sender">sender object that should be a <see cref="RemoteActionEndpoint"/></param>
        /// <param name="request">Login or register request</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private void EndpointOnActionPerformed(object? sender, RemoteFirstRequest request)
        {
            if (sender is not RemoteActionEndpoint) return;

            var handler = (RemoteActionEndpoint)sender;
            switch (request.Type)
            {
                case ActionType.Login:
                    Process_Login(handler, request.UserName, request.Password);
                    break;
                case ActionType.Register:
                    Process_Register(handler, request.UserName, request.Password);
                    break;
                case ActionType.RegisterAdmin:
                    Process_Register(handler, request.UserName, request.Password, true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("Request not supported");
            }
        }

        /// <summary>
        /// Handles various requests
        /// </summary>
        /// <param name="sender">sender object that should be a <see cref="RemoteActionEndpoint"/></param>
        /// <param name="request">Client request</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private void EndpointOnExtendedActionPerformed(object? sender, RemoteActionRequest actionRequest)
        {
            if (sender is not RemoteActionEndpoint) return;
            var handler = (RemoteActionEndpoint)sender;

            bool isAdmin = _users.GetByUsername(sessions[actionRequest.SessionToken])?.admin ?? false;
            switch (actionRequest.Type)
            {
                case ActionType.DeleteUser:
                    // TODO: Implement action
                    if (sessions.ContainsKey(actionRequest.SessionToken) && isAdmin)
                    {
                        Process_DeleteUser(handler, actionRequest.Instruction);
                    }
                    break;
                case ActionType.SendUsers:
                    // TODO: Implement action
                    if (sessions.ContainsKey(actionRequest.SessionToken) && isAdmin)
                    {
                        Process_SendUsers(handler);
                    }
                    break;
                case ActionType.Logout:
                    if(sessions.ContainsKey(actionRequest.SessionToken) && isAdmin)
                    {
                        sessions.TryRemove(actionRequest.SessionToken, out _);
                    }
                    break;
                // TODO: More ActionTypes
                default:
                    throw new ArgumentOutOfRangeException("Request not supported");
            }
        }

        /// <summary>
        /// Delete user from databank
        /// </summary>
        /// <param name="usernameDelete">Username of user who should get deleted</param>
        private void Process_DeleteUser(RemoteActionEndpoint handler, string usernameDelete)
        {
            // TODO: ResponseType.Error if username didn't exist
            _users.Delete(usernameDelete);
            DeleteUsersToken(usernameDelete);
            handler.PerformActionResponse(handler.Handler, new RemoteActionResponse(ResponseType.Success, $"Successfully deleted {usernameDelete}"));
        }

        /// <summary>
        /// Sends userlist to 
        /// </summary>
        /// <param name="handler"></param>
        private void Process_SendUsers(RemoteActionEndpoint handler)
        {
            StringBuilder usersString = new StringBuilder();
            foreach(var user in _users._users)
            {
                usersString.Append(user.UserName + "*");
            }
            handler.PerformActionResponse(handler.Handler, new RemoteActionResponse(ResponseType.Success, usersString.ToString()));
        }
        private void Process_Login(RemoteActionEndpoint handler, string username, string password)
        {
            switch (_users.LoginUser(username, password))
            {
                case UserListActionResult.AccessGranted:
                    string token = GenerateSessionToken(username);
                    handler.PerformExtendedActionResponse(handler.Handler, new RemoteExtendedActionResponse(ResponseType.Success, $"Access granted for {username}.", token, false));
                    break;
                case UserListActionResult.AccessGrantedAsAdmin:
                    string adminToken = GenerateSessionToken(username);
                    handler.PerformExtendedActionResponse(handler.Handler, new RemoteExtendedActionResponse(ResponseType.Success, $"Access granted for {username}.", adminToken, true));
                    break;
                case UserListActionResult.UserOkPasswordWrong:
                    handler.PerformActionResponse(handler.Handler, new RemoteActionResponse(ResponseType.Error, "Wrong password.")); 
                    break;
                case UserListActionResult.UserNotExisting:
                    handler.PerformActionResponse(handler.Handler, new RemoteActionResponse(ResponseType.Error, $"User {username} not existing."));
                    break;
                default:
                    handler.PerformActionResponse(handler.Handler, new RemoteActionResponse(ResponseType.Error, "Unsupported action."));
                    break;
            }
        }

        private void Process_Register(RemoteActionEndpoint handler, string username, string password, bool admin = false)
        {
            switch (_users.RegisterUser(username, password, admin))
            {
                case UserListActionResult.UserAlreadyExists:
                    Console.WriteLine("Error registering: User already existing.");
                    handler.PerformActionResponse(handler.Handler, new RemoteActionResponse(ResponseType.Error, $"Error! User {username} is already existing."));
                    break;
                case UserListActionResult.RegistrationOk:
                    Console.WriteLine("User registration OK.");
                    handler.PerformActionResponse(handler.Handler, new RemoteActionResponse(ResponseType.Success, $"Registration successful for {username}. You can now login."));
                    //
                    
                    //
                    break;
                default:
                    Console.WriteLine("Unknown action.");
                    handler.PerformActionResponse(handler.Handler, new RemoteActionResponse(ResponseType.Error, "Unsupported operation."));
                    break;
            }
        }

        /// <summary>
        /// Helper method to parse semicolon-separated key=value pairs
        /// </summary>
        /// <param name="cmd">A string semicolon-separated key=value pairs</param>
        /// <returns>A dictionary with key value pairs</returns>
        private Dictionary<string, string> ProcessCmd(string cmd)
        {
            cmd = cmd.TrimEnd(';');

            string[] parts = cmd.Split(new char[] { ';' });

            Dictionary<string, string> keyValuePairs = cmd.Split(';')
                .Select(value => value.Split('='))
                .ToDictionary(pair => pair[0], pair => pair[1]);

            return keyValuePairs;
        }

        /// <summary>
        /// Generates and adds session token, new session token if user already has one
        /// </summary>
        /// <param name="username">Username to log in</param>
        /// <returns>usernames generated session token</returns>
        private string GenerateSessionToken(string username)
        {
            // Create new session token
            string sessionToken = Guid.NewGuid().ToString();      

            // Store the session token
            sessions.TryAdd(sessionToken, username);

            return sessionToken;
        }

        private void DeleteUsersToken(string username)
        {
            foreach(var session in sessions)
            {
                if(session.Value == username)
                {
                    sessions.TryRemove(session);
                }
            }
        }
    }
}
