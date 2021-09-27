#region

using System;
using System.Globalization;
using System.Windows.Data;

#endregion

namespace Light.Converters
{
    [ValueConversion(typeof(double), typeof(string))]
    internal class TemperatureToKelvin : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = System.Convert.ToDouble(value);
            return $"Температура: {Math.Round(val)} К";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = (string)value;
            var dble = System.Convert.ToDouble(val.Trim('K', ' '));
            return $"{val} %";
        }
    }
}
