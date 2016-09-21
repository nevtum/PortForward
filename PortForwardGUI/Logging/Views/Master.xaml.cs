using PortForwardGUI.Logging.ViewModels;
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
            DataContext = new LogControlViewModel();
        }
    }
}
