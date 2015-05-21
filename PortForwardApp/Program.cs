using PortForward;
using System;

namespace PortForwardApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Client clientA = new ConsoleClient();
            Client clientB = new DecoratorClient(new EchoClient());

            Bridge bridge = new Bridge(clientA, clientB);

            while (true)
            {
                Console.WriteLine("write some text!");
                string input = Console.ReadLine();

                clientA.Transmit(input);
            }
        }
    }
}
