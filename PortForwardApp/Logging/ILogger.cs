﻿namespace PortForwardApp.Logging
{
    public interface ILogger
    {
        void Log(string message, params object[] args);
    }
}
