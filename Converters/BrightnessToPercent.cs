using System;
using System.Globalization;
using System.Windows.Data;

namespace Light.Converters
{
    [ValueConversion(typeof(double), typeof(string))]
    internal class BrightnessToPercent : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = System.Convert.ToDouble(value);
            return $"Яркость: {Math.Round(val)} %";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = (string)value;
            var convertedStr = System.Convert.ToDouble(val.Trim('%', ' '));
            return convertedStr;
        }
    }
}
