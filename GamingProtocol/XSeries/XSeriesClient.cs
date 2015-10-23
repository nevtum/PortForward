using GamingProtocol.Common;
using PortForward;
using PortForward.Utilities.Decoding;
using System;

namespace GamingProtocol.XSeries
{
    public class XSeriesClient : Client
    {
        private IDecoder _decoder;
        private IOQueue _queue;
        private int _peekCounter;

        public XSeriesClient(Socket socket, IDecoder decoder)
            : base(socket)
        {
            _decoder = decoder;
            _queue = new IOQueue();
            _peekCounter = 1;
        }

        protected override void HandleResponse(byte[] data)
        {
            _queue.Input.Enqueue(data);

            byte[] sob = _queue.Input.Peek(1);

            if (sob[0] != 0xFF)
            {
                _queue.Input.Purge(1);
            }

            byte[] chunk = _queue.Input.Peek(_peekCounter);

            if (chunk.Length < 128)
            {
                _peekCounter++;
            }
            else
            {
                byte[] datablock = _queue.Input.Next(_peekCounter);
                _peekCounter = 1;

                Console.WriteLine(_decoder.Decode(datablock));
            }
        }
    }
}
