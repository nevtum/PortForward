using PortForward;
using PortForward.Utilities;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace PortForwardApp.ConcreteClients
{
    public class VLC_Client : Client
    {
        TaskFactory _task;

        public VLC_Client(Socket socket) : base(socket)
        {
            _task = new TaskFactory();

            _task.StartNew(() =>
            {
                while (true)
                {
                    byte[] data = { 0x05, 0x00, 0x01, 0xA0, 0x56, 0x22, 0x26, 0x0D };
                    Thread.Sleep(500);
                    Push(new byte[] { 0x05, 0x00, 0x01, 0xD2, 0x06, 0x16, 0xE8, 0x0D });
                    Thread.Sleep(500);
                    Push(new byte[] { 0x05, 0x00, 0x01, 0xC2, 0x06, 0x15, 0x9B, 0x0D });
                    Thread.Sleep(500);
                    Push(new byte[] { 0x05, 0x00, 0x01, 0xD2, 0x06, 0x16, 0xE8, 0x0D });
                    Thread.Sleep(500);
                    Push(new byte[] { 0x05, 0x00, 0x01, 0xC2, 0x06, 0x15, 0x9B, 0x0D });
                    Thread.Sleep(500);
                    Push(new byte[] { 0x05, 0x00, 0x01, 0xD2, 0x06, 0x16, 0xE8, 0x0D });
                    Thread.Sleep(500);
                    Push(new byte[] { 0x05, 0x00, 0x01, 0xC2, 0x06, 0x15, 0x9B, 0x0D });
                    Thread.Sleep(500);
                    Push(data);
                }
            });
        }

        public override void Push(byte[] data)
        {
            LogData(data, isTransmitting: true);
            base.Push(data);
        }

        protected override void HandleResponse(byte[] data)
        {
            LogData(data, isTransmitting: false);
        }

        private void LogData(byte[] data, bool isTransmitting)
        {
            string direction = isTransmitting ? "Tx" : "Rx";

            string message = BitConverter.ToString(data);
            Console.WriteLine("{0}: {1}", direction, message);

            using (StreamWriter w = File.AppendText("log.txt"))
            {
                string msg = string.Format("[{0}] {1}: {2}", direction, DateTime.Now, message);
                w.WriteLine(msg);
            }
        }
    }
}
