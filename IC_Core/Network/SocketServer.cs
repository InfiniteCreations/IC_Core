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

using WebSocketSharp;
using WebSocketSharp.Server;

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

        private readonly byte[] _bytes = new Byte[1024];
        private readonly object _lock = new object();
        private readonly int _port;

        private WebSocketServer WSS;

        public State state { get; private set; } = State.IDLE;



        public SocketServer(int port)
        {

            _port = port;

            try
            {
                WSS = new WebSocketServer(port, false);
            }catch(Exception ex)
            {
                Console.WriteLine("[ERROR] " + ex.Message);
            }

            /* Future use of SSL
             * 
            WSS.SslConfiguration.EnabledSslProtocols = System.Security.Authentication.SslProtocols.Ssl2;
            WSS.SslConfiguration.ClientCertificateRequired = true;
            WSS.SslConfiguration.CheckCertificateRevocation = true;
            WSS.SslConfiguration.ServerCertificate = new System.Security.Cryptography.X509Certificates.X509Certificate2();
            */
        }


        public void Start()
        {
            if(state != State.RUNNING)
            {
                try
                {
                    WSS.Start();
                    state = State.RUNNING;
                }
                catch (Exception e)
                {
                    Console.WriteLine("[ERROR] " + e.Message.ToString());
                }

                Console.WriteLine("[INFO] Socket Server Started");
            }
        }

        public void AddWebSocketService<TBehavior>(string path, Func<TBehavior> initializer)  where TBehavior : WebSocketBehavior
        {
            WSS.AddWebSocketService<TBehavior>(path, initializer);
        }

        public void Stop(ushort code, string reason)
        {
            if(state == State.RUNNING)
            {

                if(WSS.IsListening)
                {
                    WSS.Stop(code, reason);
                }

                state = State.STOPPED;
                Console.WriteLine("[INFO] Socket Server Stopped");

            }
        }

    }
}
