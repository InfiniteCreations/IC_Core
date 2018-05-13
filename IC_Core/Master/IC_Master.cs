using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IC_Core.Server;

namespace IC_Core.Master
{
    public class IC_Master
    {
        public Network.SocketServer server { get; private set; }
        private IC_Core core;
        private Dictionary<Guid, IC_Server> servers = new Dictionary<Guid, IC_Server>();

        public IC_Master(IC_Core core)
        {
            this.core = core;
            this.server = new Network.SocketServer(443);

            this.server.message += (s, e ) => sendServerMessage(e.guid, e);

            this.server.disconnect += (s, e) => stopServers();
        }

        public Guid createServer(string name, Int64 owner)
        {
            IC_Server server = new IC_Server(this, name, owner);
            servers.Add(server.guid, server);
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
