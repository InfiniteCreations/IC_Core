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

        public event EventHandler<CloseEventArgs> close;
        public event EventHandler<MessageEventArgs> message;
        public event EventHandler<ErrorEventArgs> error;
        public event EventHandler<IWebSocketSession> open;

        protected override void OnClose(CloseEventArgs e)
        {
            close(this, e);
            base.OnClose(e);
        }

        protected override void OnError(ErrorEventArgs e)
        {
            error(this, e);
            base.OnError(e);
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            message(this, e);
            base.OnMessage(e);
        }

        protected override void OnOpen()
        {
            base.OnOpen();
            if (Sessions[ID] != null)
            {
                open(this, Sessions[ID]);
            }
        }

    }
}