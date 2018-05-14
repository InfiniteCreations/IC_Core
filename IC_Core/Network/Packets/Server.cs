using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IC_Core.Network.Packets
{
    public class Server : Interface.Network.Packets.Packet
    {

        public Guid guid { get; set; } = Guid.Empty;

    }
}
