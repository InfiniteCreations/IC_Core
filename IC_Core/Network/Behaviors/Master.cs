using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebSocketSharp;
using WebSocketSharp.Server;

namespace IC_Core.Network.Behaviors
{



    public class Master : WebSocketBehavior
    {

        private Action<Object, CloseEventArgs> _close;
        private Action<Object, MessageEventArgs> _message;
        private Action<Object, ErrorEventArgs> _error;
        private Action<Object, IWebSocketSession> _open;

        public Master(Action<Object, MessageEventArgs> message, Action<Object, IWebSocketSession> open, Action<Object, CloseEventArgs> close, Action<Object, ErrorEventArgs> error)
        {
            _close = close;
            _message = message;
            _error = error;
            _open = open;
        }

        protected override void OnClose(CloseEventArgs e)
        {
            _close(ID, e);
            base.OnClose(e);
        }

        protected override void OnError(ErrorEventArgs e)
        {
            _error(this, e);
            base.OnError(e);
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            _message(this, e);
            base.OnMessage(e);
        }

        protected override void OnOpen()
        {
            base.OnOpen();
            if(Sessions[ID] != null)
            {
                _open(this, Sessions[ID]);
            }
        }       

    }
}
