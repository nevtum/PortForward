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
            string message = BitConverter.ToString(data);
            Console.WriteLine("Tx: {0}", message);

            using (StreamWriter w = File.AppendText("log.txt"))
            {
                string msg = string.Format("[Tx] {0}: {1}", DateTime.Now, message);
                w.WriteLine(msg);
            }

            base.Push(data);
        }

        protected override void HandleResponse(byte[] data)
        {
            string message = BitConverter.ToString(data);
            Console.WriteLine("Rx: {0}", message);

            using (StreamWriter w = File.AppendText("log.txt"))
            {
                string msg = string.Format("[Rx] {0}: {1}", DateTime.Now, message);
                w.WriteLine(msg);
            }
        }
    }
}
