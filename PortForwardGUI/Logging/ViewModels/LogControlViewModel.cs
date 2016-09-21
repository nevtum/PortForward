using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortForwardGUI.Logging.ViewModels
{
    public class LogControlViewModel : BindableBase
    {
        public List<LoggedItem> Items
        {
            get
            {
                List<LoggedItem> items = new List<LoggedItem>();
                items.Add(LoggedItem.IncomingData(1, new byte[] { 0x1, 0x2, 0x3 }));
                items.Add(LoggedItem.IncomingData(2, new byte[] { 0x1, 0x2, 0x3, 0x3F }));
                items.Add(LoggedItem.OutgoingData(3, new byte[] { 0x1, 0x2D, 0x3B, 0x8A }));
                items.Add(LoggedItem.IncomingData(4, new byte[] { 0xE, 0xFF, 0x2A }));
                return items;
            }
        }
    }
}
