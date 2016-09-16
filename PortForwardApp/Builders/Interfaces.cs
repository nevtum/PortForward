using PortForward.Utilities;
using PortForward.Utilities.Decoding;
using System;

namespace PortForward.Builders
{
    public interface ISetSerialSettings
    {
        ISetDecoder WithSettings(Func<SerialSettings> settingsFactory);
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
