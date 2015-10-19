using PortForward;
using System;
using System.IO;
using System.Threading.Tasks;

namespace PortForwardApp.ConcreteClients
{
    public class VLC_Client : Client
    {
        private TaskFactory _task;
        private TransmitQueue _outputQueue;
        private VLC_App _app;

        public VLC_Client(Socket socket) : base(socket)
        {
            _outputQueue = new TransmitQueue();
            _app = new VLC_App(_outputQueue);
            _app.OnTimeOutOccurred += OnTimeOut;

            _task = new TaskFactory();

            _task.StartNew(() =>
            {
                while(true)
                {
                    byte[] data = _outputQueue.Next();

                    if (data != null)
                        Push(data);
                }
            });

            _task.StartNew(() =>
            {
                _app.Run();
            });
        }

        private void OnTimeOut(object sender, EventArgs e)
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
            _app.ResetTimeout();
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
