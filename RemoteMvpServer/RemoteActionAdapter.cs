using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

namespace RemoteMvpLib
{
    public class RemoteActionAdapter : IActionAdapter
    {

        private string _host = "localhost";
        private int _port;

        public RemoteActionAdapter(string host, int port)
        {
            _host = host;
            _port = port;
        }

        public async Task<IActionResponse> PerformActionAsync(IRequest request)
        {
            var buffer = new byte[1024];

            IActionResponse response = null;

            RemoteActionRequest rar = null;
            RemoteFirstRequest rfr = null;

            // TODO: check which type IRequest is
            if(request is RemoteActionRequest)
            {
                 rar = (RemoteActionRequest)request;
            }
            else if(request is RemoteFirstRequest)
            {
                 rfr = (RemoteFirstRequest)request;
            }
            else
            {
                throw new ArgumentException("Given request is not defined");
            }


            // Connect the socket to the remote endpoint. Catch any errors.
            try
            {
                string message;
                if(rar != null)
                {
                    message = ActionSerialize(rar);
                }
                else if(rfr != null)
                {
                    message = Serialize(rfr);
                }
                else
                {
                    // do not send anything
                    message = "";
                }
                Console.WriteLine("Performing remote action: " + message);
                // Connect to a Remote server
                // Get Host IP Address that is used to establish a connection
                // In this case, we get one IP address of localhost that is IP : 127.0.0.1
                // If a host has multiple addresses, you will get a list of addresses
                var host = Dns.GetHostEntry(_host);
                var ipAddress = host.AddressList[1];
                var remoteEP = new IPEndPoint(ipAddress, _port);

                // Create a TCP/IP  socket.
                var sender = new Socket(ipAddress.AddressFamily,
                    SocketType.Stream, ProtocolType.Tcp);

                // Connect to Remote EndPoint
                sender.Connect(remoteEP);

                Console.WriteLine("Client connected to {0}", sender.RemoteEndPoint.ToString());

                // Encode the data string into a byte array.
                var msg = Encoding.ASCII.GetBytes(message);

                // Send the data through the socket asynchronously.
                var bytesSent = await sender.SendAsync(msg, SocketFlags.None);
                Console.WriteLine($"{bytesSent} bytes sent to server. Waiting for response ...");

                // Receive the response from the remote device asynchronously
                
                
                var bytesRec = await sender.ReceiveAsync(buffer,SocketFlags.None);
                var responseString = Encoding.ASCII.GetString(buffer, 0, bytesRec);
                Console.WriteLine($"Received {bytesRec} bytes: {responseString}");

                // TODO: Check which form response has

                if (Regex.Match(responseString, @"^[^;]+;[^;]+;[A-Za-z0-9]{8}-[A-Za-z0-9]{4}-[A-Za-z0-9]{4}-[A-Za-z0-9]{4}-[A-Za-z0-9]{12};(True|False)$").Success)
                {
                    response = ActionDeserialize(responseString);
                }

                // TODO: Other Regex!!! Otherwise an '!' character will lead to error
                else if (Regex.Match(responseString, @"[A-Za-z]+;[^;]*").Success)
                {
                    response = Deserialize(responseString);
                }
                else
                {
                    throw new FormatException("Response is in unknown format");
                }

                // Release the socket.
                sender.Shutdown(SocketShutdown.Both);
                sender.Close();
            }
            catch (ArgumentNullException aex)
            {
                Console.WriteLine("ArgumentNullException : {0}", aex.ToString());
                throw;
            }
            catch (SocketException sex)     // ?
            {
                Console.WriteLine("SocketException : {0}", sex.ToString());
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unexpected exception : {0}", ex.ToString());
                throw;
            }

            return response;
        }


        // ############# Protocol layer #############

        private static string Serialize(RemoteFirstRequest request)
        {
            return string.Format("{0};{1};{2}", request.Type.ToString(), request.UserName, request.Password);
        }
        private static string ActionSerialize(RemoteActionRequest request)
        {
            return string.Format("{0};{1};{2}", request.Type.ToString(), request.SessionToken, request.Instruction);
        }

        private static RemoteActionResponse Deserialize(string response)
        {
            string[] parts = response.Split(';');
            RemoteActionResponse res = new RemoteActionResponse(Enum.Parse<ResponseType>(parts[0]), parts[1]);
            return res;
        }
        private static RemoteExtendedActionResponse ActionDeserialize(string response)
        {
            string[] parts = response.Split(';');

            // TODO: Exception handling
            bool adminBool = bool.Parse(parts[3]);
            RemoteExtendedActionResponse res = new RemoteExtendedActionResponse(Enum.Parse<ResponseType>(parts[0]), parts[1], parts[2], adminBool);
            return res;
        }
    }
}
