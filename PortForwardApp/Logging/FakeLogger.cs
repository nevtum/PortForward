using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PortForwardApp.Logging
{
    public class FakeLogger : ILogger
    {
        public void Log(string header, DateTime datetime, string data)
        {
            Console.WriteLine("[{0}] {1}: {2}", header, datetime, data);
        }
    }
}
