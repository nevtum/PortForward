using Prism.Events;
using System;

namespace PortForwardGUI.Core
{
    public class DataRecieved : PubSubEvent<Tuple<int, byte[]>>
    {
    }
}