using System.Windows;
using System.Windows.Data;

namespace WpfApp.Converters
{
    public class InvertedBooleanOpacityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is Boolean && (bool)value)
            {
                return 1.0;
            }
            return 0.0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is Double && (Double)value == 1.0)
            {
                return true;
            }
            return false;
        }
    }
}
