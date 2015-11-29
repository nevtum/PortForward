using System;

namespace GamingProtocol.XSeries.Domain
{
    public class ProcessorStateParams
    {
        public bool IsReceivePending { get; private set; }
        public bool IsTransactionInProgress { get; private set; }
        public bool IsIdle { get; private set; }
        public PacketDescriptor WaitFor { get; private set; }

        public static ProcessorStateParams InitialState()
        {
            return new ProcessorStateParams()
            {
                IsReceivePending = false,
                IsIdle = true,
                WaitFor = null,
                IsTransactionInProgress = false,
            };
        }

        public ProcessorStateParams SetAvailableForNewReads()
        {
            return new ProcessorStateParams()
            {
                IsReceivePending = false,
                IsIdle = !IsTransactionInProgress ? true : IsIdle,
                IsTransactionInProgress = IsTransactionInProgress,
                WaitFor = null
            };
        }

        public ProcessorStateParams UpdateWaitingFor(PacketDescriptor descriptor)
        {
            if (IsReceivePending)
                if (descriptor.Identifier != WaitFor.Identifier)
                    throw new Exception("Corrupted datablock received");

            return new ProcessorStateParams()
            {
                IsReceivePending = true,
                IsIdle = false,
                IsTransactionInProgress = IsTransactionInProgress,
                WaitFor = descriptor
            };
        }
    }
}
