using RemoteMvpLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteMvpClient
{
    public class AdminPresenter
    {
        private string _sessionToken;
        private readonly IActionAdapter _adapter;
        AdminClient _adminClient;

        public AdminPresenter(string sessionToken, IActionAdapter adapter)
        {
            _adminClient = new AdminClient();
            _sessionToken = sessionToken;
            _adapter = adapter;

            SetupLinks();
        }

        public void Show()
        {
            _adminClient.ShowDialog();
        }
        public void SetupLinks()
        {
            _adminClient.DeleteUserRequested += OnDeleteUserRequested;
            _adminClient.ShowUserListRequested += OnShowUserListRequested;
            _adminClient.LogoutRequested += OnLogoutRequested;
        }

        private async void OnLogoutRequested(object? sender, EventArgs e)
        {
            RemoteActionRequest logoutRequested = new RemoteActionRequest(ActionType.Logout, _sessionToken, "");
            await ProcessRequest(logoutRequested);
        }

        private async void OnShowUserListRequested(object? sender, EventArgs e)
        {
            RemoteActionRequest showUserListRequest = new RemoteActionRequest(ActionType.SendUsers, _sessionToken, "");
            await ProcessRequest(showUserListRequest);
        }

        private async void OnDeleteUserRequested(object? sender, string e)
        {
            RemoteActionRequest deleteUserRequested = new RemoteActionRequest(ActionType.DeleteUser, _sessionToken, e);
            await ProcessRequest(deleteUserRequested);
        }



        /// <summary>
        /// Collect and process all UI events
        /// </summary>
        /// <param name="sender">Source of event</param>
        /// <param name="request">Property-based request</param>
        private async Task ProcessRequest(IRequest request)
        {
            IActionResponse response = await _adapter.PerformActionAsync(request);

            if (response is RemoteActionResponse)
            {
                // Process result

                switch (response.Type)
                {
                    case ResponseType.Error:
                        // TODO: Implement Error message if response was unexpected

                        //_adminClient.ShowErrorMessage(response.Message);
                        break;
                    case ResponseType.Success:
                        switch (request.Type)
                        {
                            case ActionType.SendUsers:
                                string[] split = response.Message.Split('*');
                                _adminClient.ShowUsers(split.ToList());
                                break;
                            case ActionType.DeleteUser:
                                // TODO: Implement reaction to deletion response
                                break;
                            case ActionType.Logout:
                                // TODO: Implement reaction to logout (Delete session Token and maybe reopen login screen)
                                _sessionToken = "";
                                _adminClient.Close();

                                break;
                        }
                        break;
                }
            }
            else
            {
                throw new ArgumentException("Response type is not defined");
            }
        }
    }
}
