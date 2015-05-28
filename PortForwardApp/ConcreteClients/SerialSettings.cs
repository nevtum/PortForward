using System;

namespace PortForwardApp.ConcreteClients
{
    public class SerialSettings
    {
        public int BaudRate { get; set; }

        public System.IO.Ports.Parity Parity { get; set; }

        public int DataBits { get; set; }

        public System.IO.Ports.StopBits StopBits { get; set; }

        public string PortName { get; set; }
    }
}
