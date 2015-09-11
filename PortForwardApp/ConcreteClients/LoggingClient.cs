using PortForward.Utilities;
using System;
using System.IO;

namespace PortForward
{
    public class LoggingClient : Client
    {
        public LoggingClient(Socket socket)
            : base(socket)
        {
        }

        public override void Push(byte[] data)
        {
            LogData(data, isTransmitting: true);
            base.Push(data);
        }

        protected override void HandleResponse(byte[] data)
        {
            LogData(data, isTransmitting: false);
        }

        private void LogData(byte[] data, bool isTransmitting)
        {
            string direction = isTransmitting ? "Tx" : "Rx";

            string message = ByteStringConverter.GetString(data);
            Console.WriteLine("{0}: {1}", direction, message);

            using (StreamWriter w = File.AppendText("log.txt"))
            {
                string msg = string.Format("[{0}] {1}: {2}", direction, DateTime.Now, message);
                w.WriteLine(msg);
            }
        }
    }
}
