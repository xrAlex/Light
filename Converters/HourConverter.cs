using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Light.Converters
{
    class HourConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int hour = (int)value;
            string result = System.Convert.ToString(hour);

            if (result.Length < 2)
            {
                result = $"0{result}";
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool valid = int.TryParse((string)value, out int hour);
            if (valid)
            {
                if (hour > 23 || hour < 0)
                {
                    hour = 23;
                }
                return hour;
            }
            else
            {
                return DependencyProperty.UnsetValue;
            }
        }
    }
}