using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IC_Core;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace InfiniteCreations
{
    class InfiniteCreations : IC_Core.IC_Core
    {

        private HTTPServer httpServer;

        static bool isAdministrator()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        static void Main(string[] args)
        {

            Console.Title = "InfiniteCreations";

            if(!isAdministrator())
            {
                Console.WriteLine("You must be an administrator to run this program");
                Console.ReadLine();
                Environment.Exit(0);
            }

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
                httpServer = new HTTPServer(path, 5050); // web port 8080
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
                base.master.Stop();
                httpServer.Stop();
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
                    case "quit":
                    case "exit":
                    case "abort":
                        base.master.Stop();
                        httpServer.Stop();
                        Console.WriteLine("Services stopped. Press ENTER to exit");
                        Console.ReadLine();
                        Environment.Exit(0);
                        break;
                    case "help": Console.WriteLine("InfiniteCreations");
                        break;
                    case "servers": Console.WriteLine("Running servers : " + base.master.getServerCount());
                        break;
                    case "start":
                        httpServer.Start();
                        this.master.Start();
                        break;
                    case "stop":
                        if(str.Length == 2)
                        {
                            switch(str[1].ToLower())
                            {
                                case "all":
                                    base.master.Stop();
                                    httpServer.Stop();
                                    break;
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
