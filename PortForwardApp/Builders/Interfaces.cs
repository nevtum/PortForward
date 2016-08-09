using PortForward.Utilities.Decoding;
using PortForwardApp.Logging;

namespace PortForward.Builders
{
    public interface ISetSerialSettings
    {
        ISetDecoder WithSettings(SerialSettings settings);
    }

    public interface ISetDecoder
    {
        ISetLoggerOrBuildClient WithDecoder(IDecoder decoder);
    }

    public interface ISetLoggerOrBuildClient
    {
        IBuildClient WithLogger(ILogger logger);
        Client Build();
    }

    public interface IBuildClient
    {
        Client Build();
    }
}
