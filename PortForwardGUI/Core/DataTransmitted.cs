using Prism.Events;
using System;

namespace PortForwardGUI.Core
{
    public class DataTransmitted : PubSubEvent<Tuple<int, byte[]>>
    {
    }
}
