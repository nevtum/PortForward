using System;
using System.IO;

namespace PortForwardApp.Logging
{
    public class TextFileLogger : ILogger
    {
        private string _filename;
        private ILogger _inner;
        private Object _lockObject;

        public TextFileLogger(string filename, ILogger inner = null)
        {
            _filename = filename;
            _inner = inner;
            _lockObject = new Object();
        }

        public void Log(string header, DateTime datetime, string data)
        {
            lock (_lockObject)
            {
                using (StreamWriter w = File.AppendText(_filename))
                {
                    w.WriteLine("[{0}] {1}: {2}", header, datetime, data);
                }
            }

            if (_inner != null)
                _inner.Log(header, datetime, data);
        }
    }
}
