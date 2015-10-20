using Distributed;
using PortForward;
using System;

namespace PortForwardApp
{
    public static class ApplicationClientFactory
    {
        public static Client ConsoleClient(Socket socket)
        {
            return new ConsoleClient(socket);
        }

        public static Client LoggingClient(Socket socket)
        {
            return new LoggingClient(socket);
        }

        public static Client MessageQueueClient(Socket socket)
        {
            Console.Write("Please enter publisher address: ");
            string address = Console.ReadLine();

            return new ZMQClient(address, socket);
        }

        public static Client SerialClient(Socket socket)
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

            return new SerialTestClient(settings, socket);
        }
    }
}
