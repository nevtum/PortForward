namespace GamingProtocol.XSeries.Domain
{
    /// <summary>
    /// An XSeries data processing state machine.
    /// </summary>
    public class XProcessorState
    {
        private ProcessorStateParams _params;

        public XProcessorState(ProcessorStateParams parameters = null)
        {
            if (parameters == null)
            {
                _params = ProcessorStateParams.InitialState();
                return;
            }

            _params = parameters;
        }

        public bool IsReadyForProcessing(byte[] data)
        {
            if (_params.WaitFor == null)
                return true;

            return data.Length >= _params.WaitFor.ExpectedLength;
        }

        public void UpdateWaitingFor(PacketDescriptor descriptor)
        {
            _params = _params.UpdateWaitingFor(descriptor);
            // To register a timeout callback if applicable
        }

        public void SetFreeForFurtherProcessing()
        {
            _params = _params.SetAvailableForNewReads();
        }

        public string PacketIdentifier()
        {
            if (_params.WaitFor != null)
                return _params.WaitFor.Identifier;

            return "UNKNOWN";
        }

        public bool IsReceivePending
        {
            get
            {
                return _params.IsReceivePending;
            }
        }

        public bool IsTransactionInProgress
        {
            get
            {
                return _params.IsTransmissionPending;
            }
        }
    }
}
