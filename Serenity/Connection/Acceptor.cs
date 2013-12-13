using Serenity;
using Serenity.Servers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Serenity.Connection
{
    public class Acceptor : IDisposable
    {
        private Socket _socket;
        private bool _continue;

        public void Start(int port, bool con)
        {
            _continue = con;
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _socket.Bind(new IPEndPoint(IPAddress.Any, port));
            _socket.Listen(100);

            StartAccepting();
        }

        private void StartAccepting()
        {
            try
            {
                _socket.BeginAccept((asyncResult) =>
                {
                    try
                    {
                        Socket clientSocket = _socket.EndAccept(asyncResult);
                        if (OnSocketAccept != null)
                            OnSocketAccept(this, new SocketEventArgs(clientSocket));

                        if (_continue)
                            StartAccepting();
                    }
                    catch { }
                }, null);
            }
            catch { }
        }

        public void Dispose()
        {
            if (_socket != null)
            {
                //_socket.Dispose(true);
                _socket = null;
            }
        }

        public event EventHandler<SocketEventArgs> OnSocketAccept;
    }
}
