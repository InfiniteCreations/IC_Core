using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IC_Core;
using System.IO;
using System.Runtime.InteropServices;

namespace InfiniteCreations
{
    class InfiniteCreations : IC_Core.IC_Core
    {

        private HTTPServer httpServer;

        static void Main(string[] args)
        {

            Console.Title = "InfiniteCreations";

            // preparations

            new InfiniteCreations();
        }

        public InfiniteCreations() : base()
        {
            handler = new ConsoleEventDelegate(ConsoleEventCallback);
            SetConsoleCtrlHandler(handler, true);
            base.output += (s, e) => Console.WriteLine(e);
            string path = Path.GetFullPath(Path.Combine(Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\")), @"Client"));

            try
            {
                httpServer = new HTTPServer(path, 8080); // web port 8080

                Console.WriteLine("Debug webserver started on port 8080");

            }catch(Exception ex)
            {
                Console.WriteLine("Failed to start webserver " + ex.Message);
                Console.ReadLine();
                Environment.Exit(0);
            }

            while (true)
            {
                cmd(Console.ReadLine().ToString().Split(' '));
            }

        }

        private bool ConsoleEventCallback(int eventType)
        {
            if (eventType == 2)
            {
                Console.WriteLine("Stopping server...");
                httpServer.Stop();
                this.master.Stop();
            }
            return false;
        }
        static ConsoleEventDelegate handler;   // Keeps it from getting garbage collected
                                               // Pinvoke
        private delegate bool ConsoleEventDelegate(int eventType);
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleCtrlHandler(ConsoleEventDelegate callback, bool add);

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
                                    base.master.Stop();
                                    break;
                                case "servers":
                                    this.master.stopServers();
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
