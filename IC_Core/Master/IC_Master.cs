using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IC_Core.Server;
using IC_Core.Network;

namespace IC_Core.Master
{
    public class IC_Master
    {
        public Network.SocketServer server { get; private set; }
        private IC_Core core;
        private Dictionary<Guid, IC_Server> servers = new Dictionary<Guid, IC_Server>();

        private Network.Behaviors.Master masterSocket;

        public IC_Master(IC_Core core)
        {
            Console.WriteLine("[INFO] Master server starting!.");
            this.core = core;
            this.server = new Network.SocketServer(443);

            masterSocket = new Network.Behaviors.Master();

            masterSocket.close += MasterSocket_close;
            masterSocket.error += MasterSocket_error;
            masterSocket.message += MasterSocket_message;
            masterSocket.open += MasterSocket_open;

            server.AddWebSocketService<Network.Behaviors.Master>("/", () => masterSocket);

            Start();

            Console.WriteLine("[INFO] Master server is ready");

        }

        private void MasterSocket_open(object sender, WebSocketSharp.Server.IWebSocketSession e)
        {
            throw new NotImplementedException();
        }

        private void MasterSocket_message(object sender, WebSocketSharp.MessageEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void MasterSocket_error(object sender, WebSocketSharp.ErrorEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void MasterSocket_close(object sender, WebSocketSharp.CloseEventArgs e)
        {
            throw new NotImplementedException();
        }

        public Guid createServer(string name, Int64 owner)
        {
            IC_Server server = new IC_Server(this, name, owner);

            // bind socket behavior
            Network.Behaviors.Server behavior = new Network.Behaviors.Server();
            server.registerBehavior(behavior);
            servers.Add(server.guid, server);

            // register protocol on socket
            this.server.AddWebSocketService<Network.Behaviors.Server>("/" + server.namehash, () => behavior);

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
