using System;
using System.Text;
using System.Linq;

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

        public PacketDescriptor Class()
        {
            return _descriptor;
        }

        public string GetASCII(int startindex, int endindex)
        {
            System.Diagnostics.Debug.Assert(startindex < endindex);
            System.Diagnostics.Debug.Assert(startindex > 0);
            System.Diagnostics.Debug.Assert(startindex < _descriptor.ExpectedLength);
            System.Diagnostics.Debug.Assert(endindex > 0);
            System.Diagnostics.Debug.Assert(endindex < _descriptor.ExpectedLength);

            if (!_validCRC)
                return "INVALID CRC";

            byte[] array = _data
                .Skip(startindex - 1)
                .Take(endindex - (startindex - 1))
                .Reverse()
                .ToArray();

            return System.Text.Encoding.ASCII.GetString(array);
        }

        public decimal GetBCD(int startindex, int endindex)
        {
            System.Diagnostics.Debug.Assert(startindex < endindex);
            System.Diagnostics.Debug.Assert(startindex > 0);
            System.Diagnostics.Debug.Assert(startindex < _descriptor.ExpectedLength);
            System.Diagnostics.Debug.Assert(endindex > 0);
            System.Diagnostics.Debug.Assert(endindex < _descriptor.ExpectedLength);

            if (!_validCRC)
                return -1;

            byte[] array = _data
                .Skip(startindex - 1)
                .Take(endindex - (startindex - 1))
                .Reverse()
                .ToArray();

            decimal result = 0;
            foreach (byte bcd in array)
            {
                result *= 100;
                result += (10 * (bcd >> 4));
                result += bcd & 0xf;
            }

            return result;
        }

        public decimal GetPercent(int startindex, int endindex)
        {
            return decimal.Divide(GetBCD(startindex, endindex), 100);
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
            int mod = 0x00;
            for (int i = 1; i < _descriptor.ExpectedLength - 2; i++)
            {
                mod = mod ^ data[i];
                //Console.WriteLine("{0:X2} => {1:X2}", x, mod);
            }

            int upper = mod & 0xF0;
            int lower = mod & 0x0F;

            return new byte[] { (byte)lower, (byte)upper };
        }
    }
}
