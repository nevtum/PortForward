using System;
using System.Text;

namespace PortForward
{
    public class Bridge : IDisposable
    {
        private Port _portA;
        private Port _portB;

        public Bridge(Client clientA, Client clientB)
        {
            _portA = new Port();
            _portB = new Port();

            _portB.OnDataTransmitted += _portA.HandleReceived;
            _portA.OnDataTransmitted += _portB.HandleReceived;

            clientA.Initialize(_portA);
            clientB.Initialize(_portB);
        }

        public void Dispose()
        {
            _portB.OnDataTransmitted -= _portA.HandleReceived;
            _portA.OnDataTransmitted -= _portB.HandleReceived;
        }
    }
}
