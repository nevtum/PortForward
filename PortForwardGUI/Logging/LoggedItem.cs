using System;

namespace PortForwardGUI.Logging
{
    public class LoggedItem
    {
        public DateTime TimeLogged { get; private set; }
        public DataFlow Direction { get; private set; }
        public byte[] Data { get; private set; }

        public static LoggedItem IncomingData(byte[] data)
        {
            return new LoggedItem()
            {
                TimeLogged = DateTime.Now,
                Direction = DataFlow.Incoming,
                Data = data
            };
        }

        public static LoggedItem OutgoingData(byte[] data)
        {
            return new LoggedItem()
            {
                TimeLogged = DateTime.Now,
                Direction = DataFlow.Outgoing,
                Data = data
            };
        }
    }
}
