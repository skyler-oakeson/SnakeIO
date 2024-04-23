using System;
using System.Net;
using System.Net.Sockets;
using Microsoft.Xna.Framework;

namespace Server
{
    class Server
    {
        public static int Main(string[] args)
        {
            Console.WriteLine("Server starting...");
            if (readArgs(args, out ushort port))
            {
                if (MessageQueueServer.instance.initialize(port))
                {
                    Console.WriteLine("Server waiting for incomming connections on port {0}", port);
                    startServer();
                    MessageQueueServer.instance.shutdown();
                }
                else
                {
                    Console.WriteLine("Failure to initialize the MessageQueueServer");
                }
            }
            else
            {
                Console.WriteLine("Invalid port parameter.");
                Console.WriteLine("Example program usage: server --port 3000");
            }

            return 0;
        }


        /// <summary>
        /// Verify the command line port parameter is correct and if so, return the specified port
        /// </summary>
        private static bool readArgs(string[] args, out ushort port)
        {
            Predicate<string> PortParam = (arg => arg == "-p" || arg == "--port" || arg == "-port");

            port = 0;
            bool valid = true;
            if (args.Length != 2)
            {
                valid = false;
            }
            else if (!PortParam(args[0].ToLower()))
            {
                valid = false;
            }
            else
            {
                if (!ushort.TryParse(args[1], out port))
                {
                    valid = false;
                }
            }

            return valid;
        }

        private static void startServer()
        {
            TimeSpan SIMULATION_UPDATE_RATE_MS = TimeSpan.FromMilliseconds(33);

            GameModel model = new GameModel();
            bool running = model.Initialize();

            DateTime previousTime = DateTime.Now;
            while (running)
            {
                TimeSpan elapsedTime = DateTime.Now - previousTime;
                while (elapsedTime < SIMULATION_UPDATE_RATE_MS)
                {
                    elapsedTime = DateTime.Now - previousTime;
                }
                previousTime = DateTime.Now;

                model.Update(elapsedTime);
            }
            model.shutdown();
        }
    }
}

