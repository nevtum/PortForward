using System;
using System.IO;

namespace PortForward
{
    public class LoggingClient : Client
    {
        public override void Push(byte[] message)
        {
            string messageString = BitConverter.ToString(message);
            Console.WriteLine("Tx: {0}", messageString);

            using (StreamWriter w = File.AppendText("log.txt"))
            {
                string msg = string.Format("[Tx] {0}: {1}", DateTime.Now, messageString);
                w.WriteLine(msg);
            }

            base.Push(message);
        }

        public override void HandleResponse(object sender, EventArgs e)
        {
            byte[] bytes = (byte[])sender;
            string message = BitConverter.ToString(bytes);
            Console.WriteLine("Rx: {0}", message);

            using (StreamWriter w = File.AppendText("log.txt"))
            {
                string msg = string.Format("[Rx] {0}: {1}", DateTime.Now, message);
                w.WriteLine(msg);
            }
        }
    }
}
