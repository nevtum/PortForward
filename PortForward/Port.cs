using System;

namespace PortForward
{
    public interface Transmittable
    {
        void Transmit(byte[] message);
    }

    public interface Receivable
    {
        void HandleReceived(object sender, EventArgs e);
    }

    public class Port : Transmittable, Receivable
    {
        public void Transmit(byte[] message)
        {
            OnDataTransmitted(message, EventArgs.Empty);
        }

        public void HandleReceived(object sender, EventArgs e)
        {
            OnDataRecieved(sender, e);
        }

        public event EventHandler OnDataTransmitted = delegate { };
        public event EventHandler OnDataRecieved = delegate { };
    }
}
