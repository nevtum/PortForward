using GamingProtocol.Common;
using GamingProtocol.XSeries.Domain;
using PortForward.Utilities.Decoding;
using System;

namespace GamingProtocol.XSeries
{
    class XSeriesApp
    {
        private IOQueue _queue;
        private int _peekCounter;
        private IDecoder _decoder;
        private int MIN_IDENTIFICATION_LENGTH = 2;

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

            PacketDescriptor descriptor = GetPacketInfo(chunk);

            if (chunk.Length < descriptor.ExpectedLength)
            {
                _peekCounter++;
            }
            else
            {
                ProcessDatablock(descriptor);
            }
        }

        private void ProcessDatablock(PacketDescriptor descriptor)
        {
            byte[] datablock = _queue.Input.Next(_peekCounter);
            _peekCounter = 1;

            Console.WriteLine(descriptor.Identifier);
            Console.WriteLine(_decoder.Decode(datablock));

            // TODO
            // Publish event datablock received
            // Wrap bytes in a datablock value object
        }

        private PacketDescriptor GetPacketInfo(byte[] data)
        {
            // Good enough implementation for now!

            if (data.Length < MIN_IDENTIFICATION_LENGTH)
                return NullPacketDescriptor();

            if (data[1] == 0x00)
            {
                return new PacketDescriptor()
                {
                    Identifier = "SDB",
                    ExpectedLength = 128,
                };
            }
            else if (data[1] == 0x22)
            {
                return new PacketDescriptor()
                {
                    Identifier = "MDB",
                    ExpectedLength = 128,
                };
            }
            else
                return NullPacketDescriptor();
        }

        private PacketDescriptor NullPacketDescriptor()
        {
            return new PacketDescriptor()
            {
                Identifier = "UNKNOWN",
                ExpectedLength = 0,
                ExpectedRxTimeoutMs = 0,
                ExpectedTxTimeoutMs = 0
            };
        }
    }
}
