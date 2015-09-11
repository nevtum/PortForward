using System;
using System.Threading.Tasks;

namespace PortForward
{
    public class ConsoleClient : Client
    {
        private TaskFactory _taskFactory;

        public ConsoleClient(Port port)
            : base(port)
        {
            _taskFactory = new TaskFactory();
        }

        public override void Push(byte[] data)
        {
            _taskFactory.StartNew(() => base.Push(data));
        }

        protected override void HandleResponse(byte[] data)
        {
            string message = PortForward.Utilities.ByteStringConverter.GetString(data);
            Console.WriteLine("Message received: {0}", message);
        }
    }
}
