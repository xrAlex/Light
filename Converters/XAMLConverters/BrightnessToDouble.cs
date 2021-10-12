using System;
using System.Globalization;
using System.Windows.Data;

namespace Light.Converters.XAMLConverters
{
    [ValueConversion(typeof(double), typeof(double))]
    internal sealed class BrightnessToDouble : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return System.Convert.ToDouble(value) * 100;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return System.Convert.ToDouble(value) * 0.01;
        }
    }
}
