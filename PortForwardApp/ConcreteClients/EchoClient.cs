using System;

namespace PortForward
{
    public class EchoClient : Client
    {
        public override void HandleRx(object sender, EventArgs e)
        {
            string message = (string)sender;
            Transmit(message);
        }
    }
}
