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
        public XProcessorState(ProcessorStateParams parameters = null)
        {
            if (parameters == null)
            {
                SetDefaultParameters();
            }
            else
            {
                IsReceivePending = parameters.IsReceivePending;
                IsIdle = parameters.IsIdle;
                WaitFor = parameters.WaitFor;
                IsTransactionInProgress = parameters.IsTransactionInProgress;
            }
        }

        public bool IsReadyForProcessing(byte[] data)
        {
            if (WaitFor == null)
                return true;

            return data.Length >= WaitFor.ExpectedLength;
        }

        public XProcessorState UpdateWaitingFor(PacketDescriptor descriptor)
        {
            if (IsReceivePending)
                if (descriptor.Identifier != WaitFor.Identifier)
                    throw new Exception("Corrupted datablock received");

            ProcessorStateParams parameters = new ProcessorStateParams()
            {
                IsReceivePending = true,
                IsIdle = false,
                IsTransactionInProgress = IsTransactionInProgress,
                WaitFor = descriptor
            };

            return new XProcessorState(parameters);
        }

        public XProcessorState SetFreeForFurtherProcessing()
        {
            ProcessorStateParams parameters = new ProcessorStateParams()
            {
                IsReceivePending = false,
                IsIdle = !IsTransactionInProgress ? true : IsIdle,
                IsTransactionInProgress = IsTransactionInProgress,
                WaitFor = null
            };

            return new XProcessorState(parameters);
        }

        private void SetDefaultParameters()
        {
            IsReceivePending = false;
            IsIdle = true;
            WaitFor = null;
            IsTransactionInProgress = false;
        }

        public bool IsReceivePending { get; private set; }
        public bool IsTransactionInProgress { get; private set; }
        public bool IsIdle { get; private set; }
        public PacketDescriptor WaitFor { get; private set; }
    }

    public class ProcessorStateParams
    {
        public bool IsReceivePending { get; set; }
        public bool IsTransactionInProgress { get; set; }
        public bool IsIdle { get; set; }
        public PacketDescriptor WaitFor { get; set; }
    }
}
