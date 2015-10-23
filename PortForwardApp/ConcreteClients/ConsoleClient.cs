using PortForward.Utilities.Decoding;
using PortForwardApp.Logging;
using System;
using System.Threading.Tasks;

namespace PortForward
{
    public class ConsoleClient : Client
    {
        private TaskFactory _taskFactory;
        private IDecoder _decoder;
        private ILogger _logger;

        public ConsoleClient(Socket socket, IDecoder decoder, ILogger logger)
            : base(socket)
        {
            _decoder = decoder;
            _logger = logger;
            _taskFactory = new TaskFactory();
        }

        public override void Push(byte[] data)
        {
            _taskFactory.StartNew(() => base.Push(data));
            string message = _decoder.Decode(data);
            _logger.Log("Message sent: {0}", message);
        }

        protected override void HandleResponse(byte[] data)
        {
            string message = _decoder.Decode(data);
            _logger.Log("Message received: {0}", message);
        }
    }
}
