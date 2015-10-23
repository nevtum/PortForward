namespace PortForwardApp.Logging
{
    public class SilentLogger : ILogger
    {
        public void Log(string message, params object[] args)
        {
            // Do nothing
        }
    }
}
