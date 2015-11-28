namespace GamingProtocol.XSeries.Domain
{
    public class PacketDescriptor
    {
        public string Identifier { get; set; }
        public int ExpectedLength { get; set; }
        public int ExpectedRxTimeoutMs { get; set; }
        public int ExpectedTxTimeoutMs { get; set; }
    }
}
