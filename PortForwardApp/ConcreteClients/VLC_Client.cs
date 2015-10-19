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
                _app.OnTimeOutOccurred += OnTimeOut;
                _app.Run();
            });
        }

        private void OnTimeOut(object sender, EventArgs e)
        {
            Log("ERROR", "Timeout occurred", "error.txt");
        }

        public override void Push(byte[] data)
        {
            Log("Tx", BitConverter.ToString(data), "log.txt");
            base.Push(data);
        }

        protected override void HandleResponse(byte[] data)
        {
            _app.ResetTimeout();
            Log("Rx", BitConverter.ToString(data), "log.txt");
        }

        private void Log(string header, string message, string filename)
        {
            Console.WriteLine("{0}: {1}", header, message);

            using (StreamWriter w = File.AppendText(filename))
            {
                string msg = string.Format("[{0}] {1}: {2}", header, DateTime.Now, message);
                w.WriteLine(msg);
            }
        }
    }
}
