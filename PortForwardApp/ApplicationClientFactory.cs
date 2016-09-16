using Distributed;
using PortForward;
using PortForward.Utilities;
using PortForward.Utilities.Decoding;
using PortForwardApp.Logging;
using System;
using System.Net;

namespace PortForwardApp
{
    public static class ApplicationClientFactory
    {
        public static Client EchoClient(Socket socket)
        {
            return new EchoClient(socket);
        }

        public static Client ConsoleClient(Socket socket)
        {
            IDecoder decoder = new RawByteDecoder();
            //IDecoder decoder = new AsciiDecoder();

            //ILogger logger = new FakeLogger();
            //ILogger logger = new SilentLogger();
            ILogger logger = new TextFileLogger("log.txt", new FakeLogger());

            return new ConsoleClient(socket, decoder, logger);
        }

        public static Client MessageQueueClient(Socket socket)
        {
            Console.WriteLine("Broadcasting on {0}", GetLocalIPAddress());
            string address = System.Configuration.ConfigurationManager.AppSettings["SubscriptionAddress"];

            if (string.IsNullOrEmpty(address))
            {
                Console.Write("Enter ip address to accept data from: ");
                address = Console.ReadLine();
            }

            return new ZMQClient(address, socket, new FakeLogger());
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

            IDecoder decoder = new RawByteDecoder();
            //IDecoder decoder = new AsciiDecoder();

            ILogger logger = new FakeLogger();
            //ILogger logger = new SilentLogger();
            //ILogger logger = new TextFileLogger("serial_log.txt", new FakeLogger());

            return new SerialClient(socket, settings, decoder, logger);
        }

        private static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("Local IP Address Not Found!");
        }
    }
}
