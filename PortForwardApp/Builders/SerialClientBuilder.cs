using PortForward.Utilities.Decoding;
using PortForwardApp.Logging;
using System;

namespace PortForward.Builders
{
    public class SerialClientBuilder : ISetSerialSettings, ISetLoggerOrBuildClient, IBuildClient, ISetDecoder
    {
        private Func<SerialSettings> _settingsFactory;
        private ILogger _logger = new SilentLogger();
        private IDecoder _decoder;
        private Socket _socket;

        public SerialClientBuilder(Socket socket)
        {
            _socket = socket;
        }

        public ISetDecoder WithSettings(Func<SerialSettings> settingsFactory)
        {
            _settingsFactory = settingsFactory;
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
            return new SerialClient(_socket, _settingsFactory(), _decoder, _logger);
        }
    }
}
