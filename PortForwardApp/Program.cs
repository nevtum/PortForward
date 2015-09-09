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

            //RunSerialApp();
            RunNetworkBridge();

            Console.ReadLine(); // prevent application from exiting
        }

        private static void RunSerialApp()
        {
            Console.WriteLine("Please enter COM port nr: ");
            int comPort = int.Parse(Console.ReadLine());

            SerialSettings settings = new SerialSettings()
            {
                BaudRate = 9600,
                PortName = string.Format("COM{0}", comPort),
                Parity = System.IO.Ports.Parity.None,
                StopBits = System.IO.Ports.StopBits.One,
                DataBits = 8
            };

            Client clientA = new SerialTestClient(settings);
            Client clientB = new LoggingClient();

            Bridge bridge = new Bridge(clientA, clientB);
        }

        private static void RunNetworkBridge()
        {
            Console.Write("Please enter publisher address: ");
            string address = Console.ReadLine();

            Client clientA = new ConsoleClient();
            Client clientB = new ZMQClient(address);

            Bridge bridge = new Bridge(clientA, clientB);

            while (true)
            {
                string input = Console.ReadLine();

                clientA.Push(PortForward.Utilities.ByteStringConverter.GetBytes(input));
            }
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
