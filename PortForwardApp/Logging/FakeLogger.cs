using PortForward.Utilities;
using System;

namespace PortForwardApp.Logging
{
    public class FakeLogger : ILogger
    {
        public void Log(string message, params object[] args)
        {
            Console.WriteLine(message, args);
        }

        public void Error(string message, params object[] args)
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine(message, args);
            Console.ResetColor();
        }
    }
}
