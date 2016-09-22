using PortForwardGUI.Core;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;

namespace PortForwardGUI.Logging.ViewModels
{
    public class LogControlViewModel : BindableBase, IDisposable
    {
        private ObservableCollection<LoggedItem> _items;
        private ICollectionView _filtered;
        //private SubscriptionToken _dataRecievedToken;
        //private SubscriptionToken _dataTransmittedToken;
        private string _query;
        private LoggedItem _selectedItem;

        //public LogControlViewModel(IEventAggregator eventAggregator)
        public LogControlViewModel()
        {
            //PubSubEvent<Tuple<int, byte[]>> dataRecievedEvent = eventAggregator.GetEvent<DataRecieved>();
            //_dataRecievedToken = dataRecievedEvent.Subscribe(OnDataRecieved);

            //PubSubEvent<Tuple<int, byte[]>> dataTransmittedEvent = eventAggregator.GetEvent<DataTransmitted>();
            //_dataTransmittedToken = dataTransmittedEvent.Subscribe(OnDataRecieved);

            _items = new ObservableCollection<LoggedItem>();
            _filtered = CollectionViewSource.GetDefaultView(_items);
            RemoveFilters();

            _query = string.Empty;
        }

        public ICollectionView Items
        {
            get
            {
                return _filtered;
            }
        }

        public LoggedItem SelectedItem
        {
            get
            {
                return _selectedItem;
            }
            set
            {
                _selectedItem = value;
                OnPropertyChanged(() => SelectedItem);
            }
        }

        public string Query
        {
            get
            {
                return _query;
            }
            set
            {
                if (value == _query)
                    return;

                _query = value;
                OnPropertyChanged(() => Query);
                OnPropertyChanged(() => ClearQueryCommand);

                // Some fun stuff to try out GUI. Temporary code
                TrySomethingRandom();
            }
        }

        public DelegateCommand ClearQueryCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    Query = "";
                }, () => Query.Length > 0);
            }
        }

        // Some fun stuff to try out GUI. Temporary code
        private void TrySomethingRandom()
        {
            Random r = new Random(DateTime.Now.Millisecond);

            int result = r.Next(100);

            if (result > 20)
            {
                OnDataRecieved(new Tuple<int, byte[]>(result, new byte[] { 0x1, 0x2 }));
            }
            else
            {
                OnDataTransmitted(new Tuple<int, byte[]>(result, new byte[] { 0x1, 0x2 }));
            }

            if (_items.Count > 100)
                _items.Remove(_items.Last());
            }

        private void OnDataRecieved(Tuple<int, byte[]> payload)
        {
            _items.Insert(0, LoggedItem.IncomingData(payload.Item1, payload.Item2));
        }

        private void OnDataTransmitted(Tuple<int, byte[]> payload)
        {
            _items.Insert(0, LoggedItem.OutgoingData(payload.Item1, payload.Item2));
        }

        private void ApplyFilter(DataFlow direction)
        {
            _filtered.Filter = (obj) => ((LoggedItem)obj).Direction == direction;
        }

        private void RemoveFilters()
        {
            _filtered.Filter = (o) => true;
        }

        public void Dispose()
        {
            //_dataRecievedToken.Dispose();
            //_dataTransmittedToken.Dispose();
        }
    }
}
