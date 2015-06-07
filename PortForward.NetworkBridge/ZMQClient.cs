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

        public override void Initialize(Port port)
        {
            base.Initialize(port);

            _context = NetMQContext.Create();
            _pubSocket = _context.CreatePublisherSocket();
            _subSocket = _context.CreateSubscriberSocket();

            _pubSocket.Bind("tcp://*:4040");
            _subSocket.Bind("tcp://*:4041");

            Task.Factory.StartNew(ListenerThread);
        }

        public override void HandleResponse(object sender, EventArgs e)
        {
            byte[] bytes = (byte[])sender;
            _pubSocket.SendMore(_topic).Send(bytes);
        }

        private void ListenerThread()
        {
            while (true)
            {
                string response = _subSocket.ReceiveString();
                byte[] bytes = ByteStringConverter.GetBytes(response);

                Push(bytes);
            }
        }
    }
}
