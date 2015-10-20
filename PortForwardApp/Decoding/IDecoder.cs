namespace PortForwardApp.Decoding
{
    public interface IDecoder
    {
        string Decode(byte[] data);
    }
}
