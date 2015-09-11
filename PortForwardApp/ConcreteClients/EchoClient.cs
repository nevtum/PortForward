using System;

namespace PortForward
{
    public class EchoClient : Client
    {
        public EchoClient(Socket socket)
            : base(socket)
        {
        }

        protected override void HandleResponse(byte[] data)
        {
            Push(data);
        }
    }
}
