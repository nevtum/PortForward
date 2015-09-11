using System;

namespace PortForward
{
    public class EchoClient : Client
    {
        public EchoClient(Port port)
            : base(port)
        {
        }

        protected override void HandleResponse(byte[] data)
        {
            Push(data);
        }
    }
}
