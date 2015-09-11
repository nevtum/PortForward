using System;

namespace PortForward
{
    public class EchoClient : Client
    {
        public override void HandleResponse(byte[] data)
        {
            Push(data);
        }
    }
}
