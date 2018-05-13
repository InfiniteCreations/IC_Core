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

        protected IC_Core core { get; private set; }
        private Dictionary<Guid, IC_Server> servers = new Dictionary<Guid, IC_Server>();

        public IC_Master(IC_Core core)
        {
            this.core = core;
        }

        public Guid createServer(string name, Int64 owner)
        {
            IC_Server server = new IC_Server(this, name, owner);
            servers.Add(server.guid, server);
            return server.guid;
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

        public  void stopServer(Guid id)
        {
            getServer(id)?.Stop();
        }

    }
}
