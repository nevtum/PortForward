using System;
using System.Linq;
using System.Collections.Generic;

namespace GamingProtocol.Common
{
    public class TransmitQueue
    {
        private Queue<byte> _queue = new Queue<byte>();
        private Object _lockObj = new Object();

        public void Enqueue(byte[] data)
        {
            lock (_lockObj)
            {
                foreach (byte b in data)
                    _queue.Enqueue(b);
            }
        }

        public byte[] Peek(int nr)
        {
            return _queue.Take(nr).ToArray();
        }

        public void Purge(int nr)
        {
            lock (_lockObj)
            {
                for (int i = 0; i < nr; i++)
                {
                    Next();
                }
            }
        }

        private byte Next()
        {
            if (_queue.Count > 0)
                return _queue.Dequeue();

            throw new Exception("Cannot dequeue an empty queue!");
        }

        public byte[] Next(int nr)
        {
            byte[] data = new byte[nr];

            lock (_lockObj)
            {
                for (int i = 0; i < nr; i++)
                {
                    data[i] = _queue.Dequeue();
                }
            }

            return data;
        }

        public int Size()
        {
            lock(_lockObj)
            {
                return _queue.Count;
            }
        }
    }
}