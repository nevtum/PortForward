using System;

namespace PortForward
{
    public abstract class Client
    {
        private Port _port;

        public Client(Port port)
        {
            _port = port;
            port.OnDataRecieved += OnDataRecieved;
        }

        public virtual void Push(byte[] data)
        {
            _port.Transmit(data);
        }

        public abstract void HandleResponse(byte[] data);

        public void Dispose()
        {
            _port.OnDataRecieved -= OnDataRecieved;
        }

        private void OnDataRecieved(object sender, EventArgs e)
        {
            byte[] data = (byte[])sender;
            HandleResponse(data);
        }
    }
}