using PortForward.Utilities.Decoding;
using PortForwardApp.Logging;
using System;
using System.IO.Ports;

namespace PortForward
{
    public class SerialClient : Client, IDisposable
    {
        private SerialPort _serialPort;
        private Object _isIOBusy;
        private IDecoder _decoder;
        private ILogger _logger;

        public SerialClient(Socket socket,
            SerialSettings settings,
            IDecoder decoder,
            ILogger logger)
            : base(socket)
        {
            _decoder = decoder;
            _logger = logger;

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
                string message = _decoder.Decode(data);
                _logger.Log("[SERIAL TX] {0}: {1}", DateTime.Now, message);
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

                string message = _decoder.Decode(bytes);
                _logger.Log("[SERIAL RX] {0}: {1}", DateTime.Now, message);
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
