namespace PortForward.Utilities
{
    public interface ILogger
    {
        void Log(string message, params object[] args);
        void Error(string message, params object[] args);
    }
}
