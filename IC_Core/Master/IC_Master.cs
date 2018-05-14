using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IC_Core.Server;
using IC_Core.Network;
using WebSocketSharp.Server;

namespace IC_Core.Master
{
    public class IC_Master
    {
        public Network.SocketServer server { get; private set; }
        private IC_Core core;
        private Dictionary<Guid, IC_Server> servers = new Dictionary<Guid, IC_Server>();

        List<IWebSocketSession> clients = new List<IWebSocketSession>();

        public IC_Master(IC_Core core)
        {
            Console.WriteLine("[INFO] Master server starting!.");
            this.core = core;
            this.server = new Network.SocketServer(443);

            server.AddWebSocketService<Network.Behaviors.Master>("/", () => new Network.Behaviors.Master(MasterSocket_message, MasterSocket_open, MasterSocket_close, MasterSocket_error));

            Start();

            Console.WriteLine("[INFO] Master server is ready");

        }

        private void MasterSocket_open(Object sender, WebSocketSharp.Server.IWebSocketSession e)
        {
            clients.Add(e);
            Console.WriteLine("Socket connected " + e.ID);
        }

        private void MasterSocket_message(Object sender, WebSocketSharp.MessageEventArgs e)
        {
            Console.WriteLine("Socket message " + e.Data);
        }

        private void MasterSocket_error(Object sender, WebSocketSharp.ErrorEventArgs e)
        {
            Console.WriteLine("Socket error " + e.Message);
        }

        private void MasterSocket_close(Object sender, WebSocketSharp.CloseEventArgs e)
        {
            int i = clients.FindIndex(x => x.ID == (string) sender);
            if(i > -1) clients.RemoveAt(i);
            Console.WriteLine( i + " Socket closed " + e.Code + " : " + sender);
        }

        public Guid createServer(string name, Int64 owner)
        {
            IC_Server server = new IC_Server(this, name, owner);

            // bind socket behavior
            servers.Add(server.guid, server);

            // register protocol on socket
            this.server.AddWebSocketService<Network.Behaviors.Server>("/" + server.namehash, () => new Network.Behaviors.Server(server));

            Console.WriteLine("[INFO] Server created " + server.namehash);
            return server.guid;
        }

        private void sendServerMessage(Guid id, dynamic data)
        {
            getServer(id)?.parse(data);
        }

        public int getServerCount()
        {
            return servers.Count;
        }

        public void Start()
        {
            this.server.Start();

        }

        public void Stop()
        {
            this.server.Stop(1000, "closed master server");
        }

        public IC_Server getServer(Guid server)
        {
            if (servers.ContainsKey(server))
            {
                return servers[server];
            }
            return null;
        }

        public void startServer(Guid id)
        {
            getServer(id)?.Start(new Configuration()) ;
        }

        public void stopServer(Guid id)
        {
            getServer(id)?.Stop();
        }

        public void stopServers()
        {
            foreach(KeyValuePair<Guid, IC_Server> server in servers)
            {
                if(server.Value.state == IC_Server.ServerState.RUNNING)
                {
                    server.Value.Stop();
                }
            }
        }

    }
}
