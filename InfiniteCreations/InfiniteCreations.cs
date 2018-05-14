using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IC_Core;

namespace InfiniteCreations
{
    class InfiniteCreations : IC_Core.IC_Core
    {

        static void Main(string[] args)
        {
            Console.Title = "InfiniteCreations";

            // preparations

            new InfiniteCreations();
        }

        public InfiniteCreations() : base()
        {

            base.output += (s, e) => Console.WriteLine(e);

            while (true)
            {
                cmd(Console.ReadLine().ToString().Split(' '));
            }

        }

        private void cmd(string[] str)
        {
            if(str.Length > 0)
            {
                switch(str[0].ToLower())
                {
                    case "help": Console.WriteLine("InfiniteCreations");
                        break;
                    case "servers": Console.WriteLine("Running servers : " + base.master.getServerCount());
                        break;
                    case "stop":
                        if(str.Length == 2)
                        {
                            switch(str[1].ToLower())
                            {
                                case "server":
                                    base.master.server.Stop(0, "closed server connection");
                                    break;
                                case "servers":
                                    break;
                                case "master":
                                    base.master.Stop();
                                    break;
                            }
                        }else
                        {
                            Console.WriteLine("Stop what? [server | servers]");
                        }
                        break;
                    default: Console.WriteLine("try `help`");
                        break;
                }
            }
        }

    }
}
