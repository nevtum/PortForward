namespace GamingProtocol.Common
{
    public class IOQueue
    {
        public IOQueue()
        {
            Input = new TransmitQueue();
            Output = new TransmitQueue();
        }

        public TransmitQueue Input { get; private set; }
        public TransmitQueue Output { get; private set; }
    }
}
