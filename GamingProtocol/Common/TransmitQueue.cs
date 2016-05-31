using System;
using System.Linq;
using System.Collections.Generic;

namespace GamingProtocol.Common
{
    public class TransmitQueue
    {
        private Queue<byte[]> _queue = new Queue<byte[]>();

        public void Enqueue(byte[] data)
        {
            _queue.Enqueue(data);
        }

        public byte[] Peek(int nr)
        {
            List<byte> flattened = new List<byte>();

            foreach (byte[] chunk in _queue.Take(nr).Where(c => c.Length > 0))
            {
                flattened.AddRange(chunk);
            }

            if (flattened.Count == 0)
                throw new Exception("What's going on!");

            return flattened.ToArray();
        }

        public void Purge(int nr)
        {
            for (int i = 0; i < nr; i++)
            {
                Next();
            }
        }

        public byte[] Next()
        {
            if (_queue.Count > 0)
                return _queue.Dequeue();

            return null;
        }

        public byte[] Next(int nr)
        {
            List<byte> data = new List<byte>();

            for (int i = 0; i < nr; i++)
            {
                byte[] dequeued = Next();
                
                if (dequeued == null)
                    return data.ToArray();

                data.AddRange(dequeued);
            }

            return data.ToArray();
        }
    }
}