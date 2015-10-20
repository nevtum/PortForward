using PortForwardApp.Decoding;
using System;
using System.Threading.Tasks;

namespace PortForward
{
    public class ConsoleClient : Client
    {
        private TaskFactory _taskFactory;
        private IDecoder _decoder;

        public ConsoleClient(Socket socket, IDecoder decoder)
            : base(socket)
        {
            _decoder = decoder;
            _taskFactory = new TaskFactory();
        }

        public override void Push(byte[] data)
        {
            _taskFactory.StartNew(() => base.Push(data));
            string message = _decoder.Decode(data);
            Console.WriteLine("Message sent: {0}", message);
        }

        protected override void HandleResponse(byte[] data)
        {
            string message = _decoder.Decode(data);
            Console.WriteLine("Message received: {0}", message);
        }
    }
}
