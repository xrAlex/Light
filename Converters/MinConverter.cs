using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Light.Converters
{
    class MinConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int min = (int)value;
            string result = System.Convert.ToString(min);

            if (result.Length < 2)
            {
                result = $"0{result}";
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool valid = int.TryParse((string)value, out int min);
            if (valid)
            {
                if (min > 60 || min < 0)
                {
                    min = 60;
                }
                return min;
            }
            else
            {
                return DependencyProperty.UnsetValue;
            }          
        }
    }
}