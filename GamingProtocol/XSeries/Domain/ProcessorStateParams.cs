namespace GamingProtocol.XSeries.Domain
{
    public class ProcessorStateParams
    {
        public bool IsReceivePending { get; set; }
        public bool IsTransactionInProgress { get; set; }
        public bool IsIdle { get; set; }
        public PacketDescriptor WaitFor { get; set; }
    }
}
