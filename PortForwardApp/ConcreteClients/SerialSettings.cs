using System;
using System.IO.Ports;

namespace PortForward
{
    public class SerialSettings
    {
        public int BaudRate { get; set; }

        public Parity Parity { get; set; }

        public int DataBits { get; set; }

        public StopBits StopBits { get; set; }

        public string PortName { get; set; }
    }
}
