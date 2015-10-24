using GamingProtocol.Common;
using PortForward;
using PortForward.Utilities.Decoding;
using System.Threading.Tasks;

namespace GamingProtocol.XSeries
{
    public class XSeriesClient : Client
    {
        private IOQueue _queue;
        private TaskFactory _task;
        private XSeriesApp _processor;

        public XSeriesClient(Socket socket)
            : base(socket)
        {
            _queue = new IOQueue();
            _task = new TaskFactory();
            _processor = new XSeriesApp(_queue, new RawByteDecoder());

            _task.StartNew(() =>
            {
                while (true)
                {
                    byte[] data = _queue.Output.Next();

                    if (data != null)
                        Push(data);
                }
            });
        }

        protected override void HandleResponse(byte[] data)
        {
            _queue.Input.Enqueue(data);
            _processor.Process();
        }
    }
}
