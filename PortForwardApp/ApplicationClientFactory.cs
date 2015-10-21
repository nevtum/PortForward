using Distributed;
using PortForward;
using PortForwardApp.Decoding;
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
            return new ConsoleClient(socket, decoder);
        }

        public static Client LoggingClient(Socket socket)
        {
            IDecoder decoder = new RawByteDecoder();
            //IDecoder decoder = new AsciiDecoder();
            return new LoggingClient(socket, decoder);
        }

        public static Client MessageQueueClient(Socket socket)
        {
            Console.WriteLine("Listening on {0}", GetLocalIPAddress());
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
