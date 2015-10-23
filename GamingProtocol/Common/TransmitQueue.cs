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

            foreach (byte[] chunk in _queue.Take(nr))
            {
                flattened.AddRange(chunk);
            }

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
            try
            {
                return _queue.Dequeue();
            }
            catch (Exception e)
            {
                return null;
            }
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