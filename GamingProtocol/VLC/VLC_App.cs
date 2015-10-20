using GamingProtocol.Common;
using System;
using System.Threading;

namespace GamingProtocol.VLC
{
    public class VLC_App
    {
        System.Timers.Timer _timer;
        private IOQueue _queue;

        public VLC_App(IOQueue queue)
        {
            _queue = queue;
        }

        public void Run()
        {
            InitializeTimeoutSettings();

            while (true)
            {
                byte[] data = { 0x05, 0x00, 0x01, 0xA0, 0x56, 0x22, 0x26, 0x0D };
                Thread.Sleep(500);
                _queue.Output.Enqueue(new byte[] { 0x05, 0x00, 0x01, 0xD2, 0x06, 0x16, 0xE8, 0x0D });
                Thread.Sleep(500);
                _queue.Output.Enqueue(new byte[] { 0x05, 0x00, 0x01, 0xC2, 0x06, 0x15, 0x9B, 0x0D });
                Thread.Sleep(500);
                _queue.Output.Enqueue(new byte[] { 0x05, 0x00, 0x01, 0xD2, 0x06, 0x16, 0xE8, 0x0D });
                Thread.Sleep(500);
                _queue.Output.Enqueue(new byte[] { 0x05, 0x00, 0x01, 0xC2, 0x06, 0x15, 0x9B, 0x0D });
                Thread.Sleep(500);
                _queue.Output.Enqueue(new byte[] { 0x05, 0x00, 0x01, 0xD2, 0x06, 0x16, 0xE8, 0x0D });
                Thread.Sleep(500);
                _queue.Output.Enqueue(new byte[] { 0x05, 0x00, 0x01, 0xC2, 0x06, 0x15, 0x9B, 0x0D });
                Thread.Sleep(500);
                _queue.Output.Enqueue(data);
            }
        }

        private void InitializeTimeoutSettings()
        {
            _timer = new System.Timers.Timer(5000);
            _timer.Elapsed += (s, e) =>
            {
                OnTimeOutOccurred(this, EventArgs.Empty);
            };

            _timer.AutoReset = true;
            _timer.Start();
        }

        public void ResetTimeout()
        {
            _timer.Stop();
            _timer.Start();
        }

        public event EventHandler OnTimeOutOccurred = delegate { };
    }
}
