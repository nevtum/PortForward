using PortForward;
using System;
using System.IO.Ports;

namespace PortForwardApp.ConcreteClients
{
    /// <summary>
    /// Not tested yet
    /// </summary>
    public class SerialTestClient : Client, IDisposable
    {
        private SerialPort _serialPort;
        private Object _lockObj;

        public SerialTestClient(SerialSettings settings)
        {
            _serialPort = new SerialPort(settings.PortName,
                settings.BaudRate,
                settings.Parity,
                settings.DataBits,
                settings.StopBits);

            _lockObj = new Object();

            _serialPort.DataReceived += OnSerialDataReceived;
        }

        public override void HandleRx(object sender, EventArgs e)
        {
            byte[] bytes = (byte[])sender;

            lock (_lockObj)
            {
                _serialPort.Write(bytes, 0, bytes.Length);
            }
        }

        private void OnSerialDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            byte[] bytes;

            lock (_lockObj)
            {
                int size = _serialPort.BytesToRead;
                bytes = new byte[size];
                _serialPort.Read(bytes, 0, size);
            }

            System.Diagnostics.Debug.Assert(bytes != null);

            Transmit(bytes);
        }

        void IDisposable.Dispose()
        {
            _serialPort.DataReceived -= OnSerialDataReceived;
        }
    }
}
