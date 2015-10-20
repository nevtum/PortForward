using System;
using System.Collections.Generic;

namespace PortForwardApp
{
    public class TransmitQueue
    {
        private Queue<byte[]> _queue = new Queue<byte[]>();

        public void Enqueue(byte[] data)
        {
            _queue.Enqueue(data);
        }

        public byte[] Next()
        {
            try
            {
                return _queue.Dequeue();
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}