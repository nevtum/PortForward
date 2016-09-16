using PortForward.Utilities.Decoding;
using PortForwardApp.Logging;
using System;
using System.IO.Ports;

namespace PortForward
{
    public class BetterSerialClient : Client, IDisposable
    {
        private SerialPort _serialPort;
        private Object _isIOBusy;
        private IDecoder _decoder;
        private ILogger _logger;

        public BetterSerialClient(Socket socket,
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

            ConfigureSerialPortHandlers();
        }

        private void ConfigureSerialPortHandlers()
        {
            byte[] buffer = new byte[1024];

            Action startNewReadHandle = delegate { };

            AsyncCallback callback = (ar) =>
            {
                try
                {
                    byte[] received;

                    lock(_isIOBusy)
                    {
                        int actualLength = _serialPort.BaseStream.EndRead(ar);
                        received = new byte[actualLength];
                        Buffer.BlockCopy(buffer, 0, received, 0, actualLength);
                    }

                    System.Diagnostics.Debug.Assert(received != null);
                    Push(received);
                }
                catch (Exception e)
                {
                    _logger.Error("Uh oh! Some async method went wrong!");
                    _logger.Error(e.Message);
                }

                // start a new read handle after success or fail
                startNewReadHandle();
            };

            // connect handle to kick off async read from serial port
            startNewReadHandle = delegate
            {
                _serialPort.BaseStream.BeginRead(buffer, 0, buffer.Length, callback, null);
            };

            // invoke the first read handle
            startNewReadHandle();
        }

        protected override void HandleResponse(byte[] data)
        {
            lock (_isIOBusy)
            {
                _serialPort.Write(data, 0, data.Length);
                string message = _decoder.Decode(data);
                _logger.Log("SERIAL TX", DateTime.Now, message);
            }
        }

        void IDisposable.Dispose()
        {
            // Tear down serial port handlers
        }
    }
}
