using System;
using System.Text;

namespace PortForward
{
    public class Bridge : IDisposable
    {
        private Port _portA;
        private Port _portB;

        public Bridge()
        {
            _portA = new Port();
            _portB = new Port();

            _portB.OnDataTransmitted += _portA.HandleReceived;
            _portA.OnDataTransmitted += _portB.HandleReceived;
        }

        public Port PortA
        {
            get
            {
                return _portA;
            }
        }

        public Port PortB
        {
            get
            {
                return _portB;
            }
        }

        public void Dispose()
        {
            _portB.OnDataTransmitted -= _portA.HandleReceived;
            _portA.OnDataTransmitted -= _portB.HandleReceived;
        }
    }
}
