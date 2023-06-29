﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using RemoteMvpLib;

namespace RemoteMvpClient
{
    public class ClientPresenter
    {
        private readonly ClientView _clientView;
        private readonly IActionAdapter _adapter;

        public ClientPresenter(IActionAdapter adapter)
        {
            _adapter = adapter;
            _clientView = new ClientView();
            _clientView.LoginRequested += OnLoginRequested;
            _clientView.RegisterRequested += OnRegisterRequested;
            _clientView.RegisterAdminRequested += OnRegisterAdminRequested;
        }

       

        public void OpenUI(bool isModal)
        {
            if (isModal)
            {
                _clientView.ShowDialog();
            }
            else
            {
                _clientView.Show();
            }
            
        }

        private async void OnLoginRequested(object? sender, Tuple<string, string> e)
        {
            // TODO: Change RemoteActionRequest to RemoteFirstRequest
            RemoteFirstRequest loginRequest = new RemoteFirstRequest(ActionType.Login, e.Item1, e.Item2);
            await ProcessRequest(loginRequest);
        }

        private async void OnRegisterRequested(object? sender, Tuple<string, string> e)
        {
            // TODO: Change RemoteActionRequest to RemoteFirstRequest
            RemoteFirstRequest loginRequest = new RemoteFirstRequest(ActionType.Register, e.Item1, e.Item2);
            await ProcessRequest(loginRequest);
        }
        private async void OnRegisterAdminRequested(object? sender, Tuple<string, string> e)
        {
            // TODO: Change RemoteActionRequest to RemoteFirstRequest
            RemoteFirstRequest loginRequest = new RemoteFirstRequest(ActionType.RegisterAdmin, e.Item1, e.Item2);
            await ProcessRequest(loginRequest);
        }

        // TODO: For deletion or to show user list, send the following information ->      ActionType ;   sessionToken    ;    instruction as string(null if userlist, username if action == deletion)

        // TODO: To logout, also send ActionType and sessionToken,   sessionToken than will be deleted an no longer available to get access

        /// <summary>
        /// Collect and process all UI events
        /// </summary>
        /// <param name="sender">Source of event</param>
        /// <param name="request">Property-based request</param>
        private async Task ProcessRequest(RemoteFirstRequest request)
        {
            // Execute action in actionlistener and wait for result asynchronously
            RemoteActionResponse response = await _adapter.PerformActionAsync(request);

            // Process result

            switch (response.Type)
            {
                case ResponseType.Error:
                    _clientView.ShowErrorMessage(response.Message);
                    break;
                case ResponseType.Success:
                    switch (request.Type)
                    {
                        case ActionType.Register:
                            _clientView.RegisterOk(response.Message);
                            break;
                        case ActionType.Login:
                            _clientView.LoginOk(response.Message);   
                            break;
                        case ActionType.RegisterAdmin:
                            _clientView.RegisterOk(response.Message);
                            break;
                    }
                    break;
            }
        }
    }
}
