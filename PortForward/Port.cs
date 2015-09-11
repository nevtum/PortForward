using System;

namespace PortForward
{
    public interface Transmittable
    {
        void Transmit(byte[] data);
    }

    public interface Receivable
    {
        void HandleReceived(object sender, EventArgs e);
    }

    public class Port : Transmittable, Receivable
    {
        public void Transmit(byte[] data)
        {
            OnDataTransmitted(data, EventArgs.Empty);
        }

        public void HandleReceived(object sender, EventArgs e)
        {
            OnDataRecieved(sender, e);
        }

        public event EventHandler OnDataTransmitted = delegate { };
        public event EventHandler OnDataRecieved = delegate { };
    }
}
