using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Newtonsoft;
using Newtonsoft.Json;

namespace IC_Core.Network
{
    

    public class SocketEvents
    {

        public class SocketMessage : IDisposable
        {

            public Guid guid;
            public dynamic data;

            public SocketMessage(Guid _guid, dynamic _data)
            {
                guid = _guid;
                data = _data;
            }

            public void Dispose()
            {
                GC.SuppressFinalize(this);
            }

        }

        public class SocketError : IDisposable
        {

            public int errorCode;
            public string data;

            public SocketError(int _errorCode, string _data)
            {
                errorCode = _errorCode;
                data = _data;
            }

            public void Dispose()
            {
                GC.SuppressFinalize(this);
            }


        }


    }

    public class RootMessage
    {

        public string guid { get; set; }

    }

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
        private readonly int _port;

        public State state { get; private set; } = State.IDLE;

        public event EventHandler<SocketEvents.SocketMessage> message;
        public event EventHandler<PlayerSocket> connect;
        public event EventHandler<SocketEvents.SocketError> error;
        public event EventHandler disconnect;

        private Thread thread;

        public SocketServer(int port)
        {
            _ipAddress = _ipHostInfo.AddressList[0];
            _localEndPoint = new IPEndPoint(_ipAddress, 0);
            _port = port;
            socket = new Socket(_ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp); // TCP?

            start();
        }

        private void safeThread()
        {


            while(true)
            {

                Socket client = socket.Accept();
                acceptSocket(socket);

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



                try
                {

                    // parse data

                    var _data = JsonConvert.DeserializeObject<RootMessage>(data);

                    if(_data.guid.Length == 32)
                    {
                        message(null, new SocketEvents.SocketMessage(Guid.Parse(_data.guid), _data));
                    }

                }
                catch (Exception ex)
                {

                }


            }
        }

        public void start()
        {
            if(state != State.RUNNING)
            {
                try
                {

                    socket.Bind(_localEndPoint);
                    socket.Listen(_port);

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

        public void stop()
        {
            if(state == State.RUNNING)
            {
                if(socket.Connected)
                {
                    socket.Disconnect(true);

                }

                disconnect(null, null);

                state = State.STOPPED;
                
            }
        }

    }
}
