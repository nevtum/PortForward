using PortForward;
using PortForward.Utilities;
using System;

namespace PortForwardApp
{
    class Program
    {
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

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
            Client clientB = new ConsoleClient();

            Bridge bridge = new Bridge(clientA, clientB);

            Console.ReadLine(); // prevent application from exiting
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
