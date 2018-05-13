using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace IC_Core.Network
{
    public class SocketServer
    {

        public enum State
        {
            IDLE = -1,
            STOPPED = 0,
            RUNNING = 1,
        }

        private Socket socket;
        private readonly byte[] _bytes = new Byte[1024];
        private readonly IPHostEntry _ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
        private readonly IPAddress _ipAddress;
        private readonly IPEndPoint _localEndPoint;
        private readonly object _lock = new object();

        public State state { get; private set; } = State.IDLE;
        private Thread thread;

        public SocketServer(int port)
        {
            _ipAddress = _ipHostInfo.AddressList[0];
            _localEndPoint = new IPEndPoint(_ipAddress, 0);

            socket = new Socket(_ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp); // TCP?

            start();
        }

        private void safeThread()
        {

            while(true)
            {

                Socket client = socket.Accept();
                acceptSocket(client);

            }

        }

        private void acceptSocket(Socket client)
        {

            string data = string.Empty;
            byte[] bytes = _bytes.ToArray();


            lock(_lock)
            {
                while(true)
                {

                    int byteLength = client.Receive(bytes);
                    data += Encoding.ASCII.GetString(bytes, 0, byteLength);
                    if (data.IndexOf("<EOF>") > -1) break;
                }

                // parse data

            }
        }

        public void start()
        {
            if(state != State.RUNNING)
            {
                try
                {

                    socket.Bind(_localEndPoint);

                    thread = new Thread(new ThreadStart(safeThread));
                    thread.Start();

                    Console.WriteLine("[INFO] Socket Server Started");

                }
                catch (Exception e)
                {
                    Console.WriteLine("[ERROR] " + e.Message.ToString());
                }
            }
        }

    }
}
