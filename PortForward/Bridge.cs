using System;
using System.Text;

namespace PortForward
{
    public class Bridge : IDisposable
    {
        private Socket _socketA;
        private Socket _socketB;

        public Bridge()
        {
            _socketA = new Socket();
            _socketB = new Socket();

            _socketB.OnDataTransmitted += _socketA.HandleReceived;
            _socketA.OnDataTransmitted += _socketB.HandleReceived;
        }

        public Socket SocketA
        {
            get
            {
                return _socketA;
            }
        }

        public Socket SocketB
        {
            get
            {
                return _socketB;
            }
        }

        public void Dispose()
        {
            _socketB.OnDataTransmitted -= _socketA.HandleReceived;
            _socketA.OnDataTransmitted -= _socketB.HandleReceived;
        }
    }
}
