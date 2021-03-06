﻿using NetMQ;
using PortForward;
using PortForward.Utilities;
using System;
using System.Threading.Tasks;

namespace Distributed
{
    public class ZMQClient : Client
    {
        private NetMQContext _context;
        private NetMQSocket _pubSocket;
        private NetMQSocket _subSocket;
        private ILogger _logger;
        private string _topic = "";
        private string _remote;

        public ZMQClient(string publisherAddress, Socket socket, ILogger logger)
            : base(socket)
        {
            _logger = logger;
            _remote = string.Format("tcp://{0}:4040", publisherAddress);

            _context = NetMQContext.Create();
            _pubSocket = _context.CreatePublisherSocket();
            _subSocket = _context.CreateSubscriberSocket();

            _pubSocket.Bind("tcp://*:4040");
            Task.Factory.StartNew(ListenerThread);
        }

        protected override void HandleResponse(byte[] data)
        {
            NetMQMessage message = new NetMQMessage();
            message.Append(data);
            _pubSocket.SendMessage(message, dontWait: true);
        }

        private void ListenerThread()
        {
            int minReconnectDelay = 1;
            int reconnectDelay = minReconnectDelay;
            int maxReconnectDelay = 60;

            while (true)
            {
                try
                {
                    _subSocket.Connect(_remote);
                    _subSocket.Subscribe(_topic);
                    reconnectDelay = minReconnectDelay;

                    while (true)
                    {
                        byte[] bytes = _subSocket.Receive();

                        Push(bytes);
                    }
                }
                catch (Exception e)
                {
                    _logger.Error("Could not connect to remote publisher!");
                    _logger.Error("Trying to re-connect in {0}s", reconnectDelay);
                    System.Threading.Thread.Sleep(reconnectDelay * 1000);

                    reconnectDelay *= 2;

                    if (reconnectDelay > maxReconnectDelay)
                        reconnectDelay = maxReconnectDelay;
                }
            }
        }
    }
}
