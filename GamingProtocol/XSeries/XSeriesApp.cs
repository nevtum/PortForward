using GamingProtocol.Common;
using GamingProtocol.XSeries.Domain;
using PortForward.Utilities.Decoding;
using System;

namespace GamingProtocol.XSeries
{
    class XSeriesApp
    {
        private readonly IDecoder _decoder;
        private int MIN_IDENTIFICATION_LENGTH = 2;
        private IOQueue _queue;
        private int _peekCounter;
        private XProcessorState _state;

        public XSeriesApp(IOQueue queue, IDecoder decoder)
        {
            _peekCounter = 1;
            _queue = queue;
            _decoder = decoder;
            _state = new XProcessorState();
        }

        public void Process()
        {
            if (_state.IsTransactionInProgress)
                return; // Return for now. Add more logic later!

            if (!_state.IsReceivePending)
            {
                byte[] sob = _queue.Input.Peek(1);

                if (sob[0] != 0xFF)
                    _queue.Input.Purge(1);
            }

            byte[] chunk = _queue.Input.Peek(_peekCounter);

            PacketDescriptor descriptor = GetPacketInfo(chunk);
            _state.UpdateWaitingFor(descriptor);

            if (chunk.Length < _state.WaitFor.ExpectedLength)
            {
                _peekCounter++;
            }
            else
            {
                ProcessDatablock();
            }
        }

        private void ProcessDatablock()
        {
            byte[] datablock = _queue.Input.Next(_peekCounter);
            _peekCounter = 1;

            Console.WriteLine(_state.WaitFor.Identifier);
            Console.WriteLine(_decoder.Decode(datablock));

            // TODO
            // Validate datablock with CRC check
            // Publish event datablock received
            // Wrap bytes in a datablock value object

            _state.SetFreeForFurtherProcessing();
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
