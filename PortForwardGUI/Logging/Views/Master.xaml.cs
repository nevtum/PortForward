using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace PortForwardGUI.Logging.Views
{
    /// <summary>
    /// Interaction logic for Master.xaml
    /// </summary>
    public partial class Master : UserControl
    {
        public Master()
        {
            InitializeComponent();

            ObservableCollection<LoggedItem> items = new ObservableCollection<LoggedItem>();
            items.Add(LoggedItem.IncomingData(1, new byte[] { 0x1, 0x2, 0x3 }));
            items.Add(LoggedItem.IncomingData(2, new byte[] { 0x1, 0x2, 0x3 }));
            items.Add(LoggedItem.OutgoingData(3, new byte[] { 0x1, 0x2, 0x3 }));
            items.Add(LoggedItem.IncomingData(4, new byte[] { 0x1, 0x2, 0x3 }));

            _mydatagrid.ItemsSource = items;
        }
    }
}
