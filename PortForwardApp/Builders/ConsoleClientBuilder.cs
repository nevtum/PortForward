using PortForward.Utilities;
using PortForward.Utilities.Decoding;
using PortForwardApp.Logging;

namespace PortForward.Builders
{
    public class ConsoleClientBuilder
    {
        private Socket _socket;
        private IDecoder _decoder;
        private ILogger _logger;

        public ConsoleClientBuilder(Socket socket)
        {
            _socket = socket;
        }

        public ConsoleClientBuilder WithDecoder(IDecoder decoder)
        {
            _decoder = decoder;
            return this;
        }

        public ConsoleClientBuilder WithLogger(ILogger logger)
        {
            _logger = logger;
            return this;
        }

        public ConsoleClient Build()
        {
            return new ConsoleClient(_socket, _decoder, _logger);
        }
    }
}