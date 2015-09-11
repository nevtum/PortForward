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

            Bridge bridge = new Bridge();
            Client clientA = new SerialTestClient(settings, bridge.PortA);
            Client clientB = new LoggingClient(bridge.PortB);
        }

        private static void RunNetworkBridge()
        {
            Console.Write("Please enter publisher address: ");
            string address = Console.ReadLine();

            Bridge bridge = new Bridge();
            Client clientA = new ConsoleClient(bridge.PortA);
            Client clientB = new ZMQClient(address, bridge.PortB);

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
