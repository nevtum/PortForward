using PortForward.Utilities.Decoding;
using PortForwardApp.Logging;

namespace PortForward.Builders
{
    public class SerialClientBuilder : ISetSerialSettings, ISetLoggerOrBuildClient, IBuildClient, ISetDecoder
    {
        private SerialSettings _settings;
        private ILogger _logger = new SilentLogger();
        private IDecoder _decoder;
        private Socket _socket;

        public SerialClientBuilder(Socket socket)
        {
            _socket = socket;
        }

        public ISetDecoder WithSettings(SerialSettings settings)
        {
            _settings = settings;
            return this;
        }

        public IBuildClient WithLogger(ILogger logger)
        {
            _logger = logger;
            return this;
        }

        public ISetLoggerOrBuildClient WithDecoder(IDecoder decoder)
        {
            _decoder = decoder;
            return this;
        }

        public Client Build()
        {
            return new SerialClient(_socket, _settings, _decoder, _logger);
        }
    }
}
