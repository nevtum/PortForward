using System;

namespace PortForward
{
    public class DecoratorClient : Client
    {
        private Client _inner;
        private Guid _id;

        public DecoratorClient(Client client)
        {
            _id = Guid.NewGuid();
            _inner = client;
        }

        public override void Initialize(Port port)
        {
            port.OnDataRecieved += HandleRx;
            _inner.Initialize(port);
        }

        public override void Transmit(byte[] message)
        {
            Console.WriteLine("{0}: Some decorated pre Tx behaviour", _id);
            _inner.Transmit(message);
            Console.WriteLine("{0}: Some decorated post Tx behaviour", _id);
        }

        public override void HandleRx(object sender, EventArgs e)
        {
            Console.WriteLine("{0}: Some decorated Rx behaviour", _id);
        }
    }
}
