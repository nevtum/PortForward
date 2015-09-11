using System;
using System.IO.Ports;

namespace PortForward
{
    public class SerialTestClient : Client, IDisposable
    {
        private SerialPort _serialPort;
        private Object _isIOBusy;

        public SerialTestClient(SerialSettings settings, Socket socket)
            : base(socket)
        {
            _serialPort = new SerialPort(settings.PortName,
                settings.BaudRate,
                settings.Parity,
                settings.DataBits,
                settings.StopBits);

            _isIOBusy = new Object();

            _serialPort.DataReceived += OnSerialDataReceived;

            if (_serialPort.IsOpen == false)
                _serialPort.Open();
        }

        protected override void HandleResponse(byte[] data)
        {
            lock (_isIOBusy)
            {
                _serialPort.Write(data, 0, data.Length);
            }
        }

        private void OnSerialDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            byte[] bytes;

            lock (_isIOBusy)
            {
                int size = _serialPort.BytesToRead;
                bytes = new byte[size];
                _serialPort.Read(bytes, 0, size);
            }

            System.Diagnostics.Debug.Assert(bytes != null);

            Push(bytes);
        }

        void IDisposable.Dispose()
        {
            if (_serialPort.IsOpen == true)
                _serialPort.Close();

            _serialPort.DataReceived -= OnSerialDataReceived;
        }
    }
}
