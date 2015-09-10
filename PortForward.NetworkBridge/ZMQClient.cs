using NetMQ;
using PortForward.Utilities;
using System;
using System.Threading.Tasks;

namespace PortForward.NetworkBridge
{
    public class ZMQClient : Client
    {
        private NetMQContext _context;
        private NetMQSocket _pubSocket;
        private NetMQSocket _subSocket;
        private string _topic = "raw-stream";
        private string _remote;

        public ZMQClient(string publisherAddress)
        {
            _remote = string.Format("tcp://{0}:4040", publisherAddress);
            
            _context = NetMQContext.Create();
            _pubSocket = _context.CreatePublisherSocket();
            _subSocket = _context.CreateSubscriberSocket();

            _pubSocket.Bind("tcp://*:4040");
            Task.Factory.StartNew(ListenerThread);
        }

        public override void HandleResponse(object sender, EventArgs e)
        {
            byte[] bytes = (byte[])sender;
            _pubSocket.SendMore(_topic).Send(bytes, bytes.Length, dontWait: true);
        }

        private void ListenerThread()
        {
            int minReconnectDelay = 1;
            int reconnectDelay = minReconnectDelay;
            int maxReconnectDelay = 60;

            while (true)
            {
                try
                {
                    _subSocket.Connect(_remote);
                    _subSocket.Subscribe(_topic);
                    reconnectDelay = minReconnectDelay;

                    while (true)
                    {
                        string response = _subSocket.ReceiveString();
                        byte[] bytes = ByteStringConverter.GetBytes(response);

                        Push(bytes);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Could not connect to remote publisher!");
                    Console.WriteLine("Trying to re-connect in {0}s", reconnectDelay);
                    System.Threading.Thread.Sleep(reconnectDelay * 1000);

                    reconnectDelay *= 2;

                    if (reconnectDelay > maxReconnectDelay)
                        reconnectDelay = maxReconnectDelay;
                }
            }
        }
    }
}
