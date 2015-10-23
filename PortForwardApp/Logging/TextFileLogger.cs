using System.IO;

namespace PortForwardApp.Logging
{
    public class TextFileLogger : ILogger
    {
        private string _filename;
        private ILogger _inner;

        public TextFileLogger(string filename, ILogger inner = null)
        {
            _filename = filename;
            _inner = inner;
        }

        public void Log(string message, params object[] args)
        {
            using (StreamWriter w = File.AppendText(_filename))
            {
                w.WriteLine(message, args);
            }

            if (_inner != null)
                _inner.Log(message, args);
        }
    }
}
