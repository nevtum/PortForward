using System;
using System.IO;

namespace PortForward
{
    class LoggingClient : Client
    {
        public override void HandleResponse(object sender, EventArgs e)
        {
            byte[] bytes = (byte[])sender;
            string message = BitConverter.ToString(bytes);
            Console.WriteLine("Rx: {0}", message);

            using (StreamWriter w = File.AppendText("log.txt"))
            {
                string msg = string.Format("{0}: {1}", DateTime.Now, message);
                w.WriteLine(msg);
            }
        }
    }
}
