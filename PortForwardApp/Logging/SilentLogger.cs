using System;

namespace PortForwardApp.Logging
{
    public class SilentLogger : ILogger
    {
        public void Log(string header, DateTime datetime, string data)
        {
            // Do nothing
        }
    }
}
