using PortForward.Utilities;
using System;

namespace PortForwardApp.Decoding
{
    public class AsciiDecoder : IDecoder
    {
        public string Decode(byte[] data)
        {
            return ByteStringConverter.GetString(data);
        }
    }
}
