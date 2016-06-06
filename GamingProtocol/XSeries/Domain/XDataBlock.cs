using System;

namespace GamingProtocol.XSeries.Domain
{
    public class XDataBlock
    {
        private PacketDescriptor _descriptor;
        private byte[] _data;
        private byte[] _crc;
        private bool _validCRC;

        public XDataBlock(PacketDescriptor descriptor, byte[] data)
        {
            _descriptor = descriptor;
            _data = data;
            _validCRC = false;
            Validate();
        }

        public bool IsValidCRC()
        {
            return _validCRC;
        }

        private void Validate()
        {
            _crc = CalculateCRC(_data);

            for (int i = 0; i < 2; i++)
            {
                int index = _descriptor.ExpectedLength - 2 + i;
                if (_crc[i] != _data[index])
                    return;
            }

            _validCRC = true;
        }

        private byte[] CalculateCRC(byte[] data)
        {
            // Incorrect implementation!!

            int mod = 0xff;
            for (int i = 1; i < 127; i++)
            {
                mod = mod ^ data[i];
            }

            int upper = mod & 0xF0;
            int lower = mod & 0x0F;

            return new byte[] { (byte)upper, (byte)lower };
        }
    }
}
