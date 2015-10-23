using PortForward.Utilities;
using System;

namespace PortForward.Utilities.Decoding
{
    public class AsciiDecoder : IDecoder
    {
        public string Decode(byte[] data)
        {
            return ByteStringConverter.GetString(data);
        }
    }
}
