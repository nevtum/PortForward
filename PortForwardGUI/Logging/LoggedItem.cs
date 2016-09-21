using System;

namespace PortForwardGUI.Logging
{
    public class LoggedItem
    {
        public LoggedItemId Id { get; private set; }
        public DateTime TimeLogged { get; private set; }
        public DataFlow Direction { get; private set; }
        public byte[] Data { get; private set; }

        public static LoggedItem IncomingData(int id, byte[] data)
        {
            return new LoggedItem()
            {
                Id = new LoggedItemId(id),
                TimeLogged = DateTime.Now,
                Direction = DataFlow.Incoming,
                Data = data
            };
        }

        public static LoggedItem OutgoingData(int id, byte[] data)
        {
            return new LoggedItem()
            {
                Id = new LoggedItemId(id),
                TimeLogged = DateTime.Now,
                Direction = DataFlow.Outgoing,
                Data = data
            };
        }
    }
}
