using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;


namespace Light.Converters
{
    [ValueConversion(typeof(int), typeof(string))]
    internal class MinConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var min = (int)value;
            var result = System.Convert.ToString(min);
            if (result.Length < 2) result = $"0{result}";

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var valid = int.TryParse((string)value, out var min);
            if (!valid) return DependencyProperty.UnsetValue;
            if (min is > 60 or < 0) min = 60;

            return min;
        }
    }
}