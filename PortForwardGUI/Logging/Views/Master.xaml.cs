using System.Collections.Generic;
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

            List<LoggedItem> items = new List<LoggedItem>();
            items.Add(LoggedItem.IncomingData(1, new byte[] { 0x1, 0x2, 0x3 }));
            items.Add(LoggedItem.IncomingData(2, new byte[] { 0x1, 0x2, 0x3, 0x3F }));
            items.Add(LoggedItem.OutgoingData(3, new byte[] { 0x1, 0x2D, 0x3B, 0x8A }));
            items.Add(LoggedItem.IncomingData(4, new byte[] { 0xE, 0xFF, 0x2A }));

            _myListView.ItemsSource = items;
        }
    }
}
