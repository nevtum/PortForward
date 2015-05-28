using System;

namespace PortForward
{
    public class EchoClient : Client
    {
        public override void HandleRx(object sender, EventArgs e)
        {
            byte[] message = (byte[])sender;
            Transmit(message);
        }
    }
}
