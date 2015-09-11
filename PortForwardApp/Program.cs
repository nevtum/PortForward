using PortForward;
using PortForward.NetworkBridge;
using PortForward.Utilities;
using System;

namespace PortForwardApp
{
    class Program
    {
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            RunClientWithInput();
            //RunClientWithoutInput();

            Console.ReadLine(); // prevent application from exiting
        }

        private static void RunClientWithoutInput()
        {
            Bridge bridge = new Bridge();
            Client clientA = ClientA(bridge.SocketA);
            Client clientB = ClientB(bridge.SocketB);
        }

        private static void RunClientWithInput()
        {
            Bridge bridge = new Bridge();
            Client activeClient = ClientA(bridge.SocketA);
            Client hiddenClient = ClientB(bridge.SocketB);

            while (true)
            {
                string input = Console.ReadLine();
                activeClient.Push(PortForward.Utilities.ByteStringConverter.GetBytes(input));
            }
        }

        private static Client ClientA(Socket socket)
        {
            //return ApplicationClientFactory.ConsoleClient(socket);
            return ApplicationClientFactory.LoggingClient(socket);
        }

        private static Client ClientB(Socket socket)
        {
            //return ApplicationClientFactory.MessageQueueClient(socket);
            //return ApplicationClientFactory.LoggingClient(socket);
            return ApplicationClientFactory.ConsoleClient(socket);
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Console.WriteLine(e.ExceptionObject.ToString());
            Console.WriteLine("Press any key to exit!");
            Console.ReadLine();
            Environment.Exit(1);
        }
    }
}
