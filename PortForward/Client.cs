using System;

namespace PortForward
{
    public abstract class Client
    {
        private Socket _socket;

        public Client(Socket socket)
        {
            _socket = socket;
            socket.OnDataRecieved += OnDataRecieved;
        }

        public virtual void Push(byte[] data)
        {
            _socket.Transmit(data);
        }

        protected abstract void HandleResponse(byte[] data);

        public void Dispose()
        {
            _socket.OnDataRecieved -= OnDataRecieved;
        }

        private void OnDataRecieved(object sender, EventArgs e)
        {
            byte[] data = (byte[])sender;
            HandleResponse(data);
        }
    }
}