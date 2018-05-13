using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IC_Core.Master;

namespace IC_Core
{

    public class IC_Core {


        protected IC_Master master { get; private set; }

        protected event EventHandler<string> output;

        public IC_Core()
        {
            this.master = new IC_Master(this);
        }

        public void log(object a)
        {
            output(null, a.ToString());
        }


    }

}
