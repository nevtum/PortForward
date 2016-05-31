using System;

namespace PortForwardApp.Logging
{
    public interface ILogger
    {
        void Log(string header, DateTime datetime, string data);
    }
}
