using PortForward;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace PortForwardApp.ConcreteClients
{
    public class VLC_Client : Client
    {
        TaskFactory _task;
        System.Timers.Timer _timer;

        public VLC_Client(Socket socket) : base(socket)
        {
            _timer = new System.Timers.Timer(5000);
            _timer.Elapsed += OnTimeOut;
            _timer.AutoReset = true;
            _timer.Start();

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

        private void OnTimeOut(object sender, System.Timers.ElapsedEventArgs e)
        {
            string message = "Timeout occurred";
            Console.WriteLine("{0}", message);

            using (StreamWriter w = File.AppendText("error.txt"))
            {
                string msg = string.Format("[ERROR] {0}: {1}", DateTime.Now, message);
                w.WriteLine(msg);
            }
        }

        public override void Push(byte[] data)
        {
            LogData(data, isTransmitting: true);
            base.Push(data);
        }

        protected override void HandleResponse(byte[] data)
        {
            _timer.Stop();
            _timer.Start();
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
