using System;

namespace PortForward.Utilities.Decoding
{
    public class RawByteDecoder : IDecoder
    {
        public string Decode(byte[] data)
        {
            return BitConverter.ToString(data);
        }
    }
}
