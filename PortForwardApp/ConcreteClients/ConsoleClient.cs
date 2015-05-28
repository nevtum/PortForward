using System;

namespace PortForward
{
    public class ConsoleClient : Client
    {
        public override void HandleRx(object sender, EventArgs e)
        {
            byte[] bytes = (byte[])sender;
            string message = PortForward.Utilities.ByteStringConverter.GetString(bytes);
            Console.WriteLine("Message received: {0}", message);
        }
    }
}
