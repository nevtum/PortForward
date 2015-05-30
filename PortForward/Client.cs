using System;

namespace PortForward
{
    public abstract class Client
    {
        private Port _port;

        public virtual void Initialize(Port port)
        {
            if (_port != null)
                throw new InvalidOperationException("Another port is already initialized!");

            _port = port;
            port.OnDataRecieved += HandleResponse;
        }

        public virtual void Push(byte[] message)
        {
            _port.Transmit(message);
        }

        public abstract void HandleResponse(object sender, EventArgs e);

        public void Dispose()
        {
            _port.OnDataRecieved -= HandleResponse;
        }
    }
}