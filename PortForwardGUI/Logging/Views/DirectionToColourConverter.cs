using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace PortForwardGUI.Logging.Views
{
    public class DirectionToColourConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DataFlow direction = (DataFlow)value;

            if (direction == DataFlow.Incoming)
                return new SolidColorBrush(Colors.Blue);
            else if (direction == DataFlow.Outgoing)
                return new SolidColorBrush(Colors.Green);

            throw new ArgumentException("Cannot identify dataflow direction");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
