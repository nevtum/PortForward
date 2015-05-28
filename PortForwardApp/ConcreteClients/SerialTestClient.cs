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

        public SerialTestClient(SerialSettings settings)
        {
            _serialPort = new SerialPort(settings.PortName,
                settings.BaudRate,
                settings.Parity,
                settings.DataBits,
                settings.StopBits);

            _serialPort.DataReceived += OnSerialDataReceived;
        }

        public override void HandleRx(object sender, EventArgs e)
        {
            byte[] bytes = (byte[])sender;
            _serialPort.Write(bytes, 0, bytes.Length);
        }

        private void OnSerialDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            byte[] bytes = (byte[])sender;
            Transmit(bytes);
        }

        void IDisposable.Dispose()
        {
            _serialPort.DataReceived -= OnSerialDataReceived;
        }
    }
}
