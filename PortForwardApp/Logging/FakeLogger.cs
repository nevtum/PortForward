using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PortForwardApp.Logging
{
    public class FakeLogger : ILogger
    {
        public void Log(string message, params object[] args)
        {
            Console.WriteLine(message, args);
        }
    }
}
