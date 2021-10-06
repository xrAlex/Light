using System;
using System.Globalization;
using System.Windows.Data;

namespace Light.Converters
{
    [ValueConversion(typeof(double), typeof(string))]
    internal class TemperatureToKelvin : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = System.Convert.ToDouble(value);
            return $"{Properties.Resources.ColorTemperature}: {Math.Round(val)} К";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
