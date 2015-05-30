using System;

namespace PortForward
{
    public class EchoClient : Client
    {
        public override void HandleResponse(object sender, EventArgs e)
        {
            byte[] message = (byte[])sender;
            Push(message);
        }
    }
}
