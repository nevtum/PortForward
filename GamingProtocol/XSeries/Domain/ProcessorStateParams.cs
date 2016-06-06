﻿using System;

namespace GamingProtocol.XSeries.Domain
{
    public class ProcessorStateParams
    {
        public bool IsReceivePending { get; private set; }
        public bool IsIdle { get; private set; }
        public PacketDescriptor WaitFor { get; private set; }

        public static ProcessorStateParams InitialState()
        {
            return new ProcessorStateParams()
            {
                IsReceivePending = false,
                IsIdle = true,
                WaitFor = null,
            };
        }

        public ProcessorStateParams SetAvailableForNewReads()
        {
            return new ProcessorStateParams()
            {
                IsReceivePending = false,
                IsIdle = true,
                WaitFor = null
            };
        }

        public ProcessorStateParams UpdateWaitingFor(PacketDescriptor descriptor)
        {
            if (descriptor == null)
                throw new ArgumentNullException("Must specify a packet descriptor");

            if (WaitFor == null && IsReceivePending)
                if (descriptor.Identifier != WaitFor.Identifier)
                    throw new Exception("Corrupted datablock received");

            return new ProcessorStateParams()
            {
                IsReceivePending = true,
                IsIdle = false,
                WaitFor = descriptor
            };
        }
    }
}
