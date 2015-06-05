using NetMQ;
using PortForward.Utilities;
using System;
using System.Threading.Tasks;

namespace PortForward.NetworkBridge
{
    public class ZMQClient : Client
    {
        private NetMQSocket _pubSocket;
        private NetMQSocket _subSocket;

        public override void Initialize(Port port)
        {
            base.Initialize(port);

            NetMQContext context = NetMQContext.Create();
            _pubSocket = context.CreatePublisherSocket();
            _subSocket = context.CreateSubscriberSocket();

            _pubSocket.Bind("tcp://localhost:4040");
            _subSocket.Bind("tcp://localhost:4041");

            Task.Factory.StartNew(ListenerThread);
        }

        public override void HandleResponse(object sender, EventArgs e)
        {
            byte[] bytes = (byte[])sender;
            _pubSocket.Send(bytes);
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
