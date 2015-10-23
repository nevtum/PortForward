﻿using PortForward.Utilities.Decoding;
using PortForwardApp.Logging;

namespace PortForward
{
    public class ConsoleClient : Client
    {
        private IDecoder _decoder;
        private ILogger _logger;

        public ConsoleClient(Socket socket, IDecoder decoder, ILogger logger)
            : base(socket)
        {
            _decoder = decoder;
            _logger = logger;
        }

        public override void Push(byte[] data)
        {
            base.Push(data);
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
