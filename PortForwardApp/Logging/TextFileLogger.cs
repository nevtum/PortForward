using System;
﻿using PortForward.Utilities;
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

        public void Log(string message, params object[] args)
        {
            lock (_lockObject)
            {
                using (StreamWriter w = File.AppendText(_filename))
                {
                    w.WriteLine(message, args);
                }
            }

            if (_inner != null)
                _inner.Log(message, args);
        }

        public void Error(string message, params object[] args)
        {
            using (StreamWriter w = File.AppendText(_filename))
            {
                w.WriteLine("** ERROR **");
                w.WriteLine(message, args);
            }

            if (_inner != null)
                _inner.Log(message, args);
        }
    }
}
