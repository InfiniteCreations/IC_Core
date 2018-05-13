using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Sockets;

namespace IC_Core.Network
{
    public class PlayerSocket
    {

        public Socket client;

        public PlayerSocket(Socket _client)
        {
            client = _client;
        }

    }
}
