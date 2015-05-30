using System;
using System.Threading.Tasks;

namespace PortForward
{
    public class ConsoleClient : Client
    {
        private TaskFactory _taskFactory;

        public ConsoleClient()
        {
            _taskFactory = new TaskFactory();
        }

        public override void Transmit(byte[] message)
        {
            _taskFactory.StartNew(() => base.Transmit(message));
        }

        public override void HandleRx(object sender, EventArgs e)
        {
            byte[] bytes = (byte[])sender;
            string message = PortForward.Utilities.ByteStringConverter.GetString(bytes);
            Console.WriteLine("Message received: {0}", message);
        }
    }
}
