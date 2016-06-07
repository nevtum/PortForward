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
        private int _bufferSize;
        private XProcessorState _state;

        public XSeriesApp(IOQueue queue, IDecoder decoder)
        {
            _bufferSize = 1;
            _queue = queue;
            _decoder = decoder;
            _state = new XProcessorState();
        }

        public void Process()
        {
            _bufferSize = _queue.Input.Size();
            byte[] chunk = _queue.Input.Peek(_bufferSize);

            if (!_state.IsReceivePending && _bufferSize > 0)
            {
                int trimLength = TrimLength(chunk);
                _queue.Input.Purge(trimLength);
                _bufferSize -= trimLength;
                // chunk becomes stale at this point
            }

            if (_bufferSize <= 0)
                return;

            PacketDescriptor descriptor = GetPacketInfo(chunk);
            _state.UpdateWaitingFor(descriptor);

            if (_state.IsReadyForProcessing(chunk))
            {
                ProcessDatablock();
                _state.SetFreeForFurtherProcessing();
                return;
            }
        }

        private int TrimLength(byte[] chunk)
        {
            int trimLength = 0;

            for (int i = 0; i < _bufferSize; i++)
            {
                if (chunk[i] == 0xFF)
                    break;

                trimLength = i + 1;
            }

            return trimLength;
        }

        private void ProcessDatablock()
        {
            byte[] data = _queue.Input.Next(_bufferSize);
            _bufferSize = _queue.Input.Size();

            Console.WriteLine(_state.WaitingForDescriptor().Identifier);
            Console.WriteLine(_decoder.Decode(data));

            XDataBlock datablock = new XDataBlock(_state.WaitingForDescriptor(), data);
            Console.WriteLine("ValidCRC: {0}", datablock.IsValidCRC());

            // TODO
            // Validate datablock with CRC check
            // Publish event datablock received
            // Wrap bytes in a datablock value object
        }

        private PacketDescriptor GetPacketInfo(byte[] data)
        {
            // Good enough implementation for now!

            if (data.Length < MIN_IDENTIFICATION_LENGTH)
                return null;

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
            else if (data[1] == 0x10)
            {
                return new PacketDescriptor()
                {
                    Identifier = "PDB1",
                    ExpectedLength = 128,
                };
            }
            else if (data[1] == 0x11)
            {
                return new PacketDescriptor()
                {
                    Identifier = "PDB2",
                    ExpectedLength = 128,
                };
            }
            else
                return null;
        }
    }
}
