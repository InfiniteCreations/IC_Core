using IC_Core.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebSocketSharp;
using WebSocketSharp.Server;

namespace IC_Core.Network.Behaviors
{
    public class Server : WebSocketBehavior
    {

        public Server(IC_Server server)
        {

        }

        protected override void OnClose(CloseEventArgs e)
        {
     
            base.OnClose(e);
        }

        protected override void OnError(ErrorEventArgs e)
        {

            base.OnError(e);
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            base.OnMessage(e);
        }

        protected override void OnOpen()
        {
            base.OnOpen();
            if (Sessions[ID] != null)
            {

            }
        }

    }
}