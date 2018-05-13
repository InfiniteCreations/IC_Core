using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IC_Core.Master;

namespace IC_Core.Server
{



    public class IC_Server
    {

        public enum ServerState
        {
            INACTIVE = -1,
            STOPPED = 0,
            RUNNING = 1,
        }

        public ServerState state = ServerState.INACTIVE;

        private IC_Master master;

        public readonly string name;
        public readonly Int64 owner;
        public readonly Guid guid;


        public IC_Server(IC_Master master, string name, Int64 owner)
        {
            this.master = master;
            this.name = name;
            this.owner = owner;
            this.guid = Guid.NewGuid();
        }


        public bool Start(Configuration config)
        {
            if(state != ServerState.RUNNING)
            {
                return true;
            }

            return false;
        }

        public bool Stop()
        {
            if(state == ServerState.RUNNING)
            {
                return true;
            }

            return false;
        }

    }
}
