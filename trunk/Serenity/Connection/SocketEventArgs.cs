using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Serenity.Connection
{
    public class SocketEventArgs : EventArgs
    {
        public Socket Socket { get; private set; }

        public SocketEventArgs(Socket socket)
        {
            Socket = socket;
        }
    }
}
