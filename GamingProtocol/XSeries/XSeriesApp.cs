using GamingProtocol.Common;
using PortForward.Utilities.Decoding;
using System;

namespace GamingProtocol.XSeries
{
    class XSeriesApp
    {
        private IOQueue _queue;
        private int _peekCounter;
        private IDecoder _decoder;

        public XSeriesApp(IOQueue queue, IDecoder decoder)
        {
            _peekCounter = 1;
            _queue = queue;
            _decoder = decoder;
        }

        public void Process()
        {
            byte[] sob = _queue.Input.Peek(1);

            if (sob[0] != 0xFF)
                _queue.Input.Purge(1);

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
