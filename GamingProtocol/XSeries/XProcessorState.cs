using GamingProtocol.XSeries.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GamingProtocol.XSeries
{
    /// <summary>
    /// An XSeries data processing state machine.
    /// </summary>
    public class XProcessorState
    {
        private Object _lockObj = new Object();

        public XProcessorState()
        {
            // Needs more analysis of valid states
            IsTransactionInProgress = false;
            SetFreeForFurtherProcessing();
        }

        public bool IsReceivePending { get; private set; }
        public bool IsTransactionInProgress { get; private set; }
        public bool IsIdle { get; private set; }
        public PacketDescriptor WaitFor { get; private set; }

        public bool IsReadyForProcessing(byte[] data)
        {
            if (WaitFor == null)
                return true;

            return data.Length >= WaitFor.ExpectedLength;
        }

        public void UpdateWaitingFor(PacketDescriptor descriptor)
        {
            if (IsReceivePending)
                if (descriptor.Identifier != WaitFor.Identifier)
                    throw new Exception("Corrupted datablock received");

            lock (_lockObj)
            {
                WaitFor = descriptor;
                IsReceivePending = true;
                IsIdle = false;
            }
        }

        public void SetFreeForFurtherProcessing()
        {
            lock (_lockObj)
            {
                if (!IsTransactionInProgress)
                    IsIdle = true;

                IsReceivePending = false;

                WaitFor = new PacketDescriptor()
                {
                    Identifier = "UNKNOWN",
                    ExpectedLength = 0,
                    ExpectedRxTimeoutMs = 0,
                    ExpectedTxTimeoutMs = 0
                };
            }
        }
    }
}
