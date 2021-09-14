﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Light.Converters
{
    internal class HourConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var hour = (int)value;
            var result = System.Convert.ToString(hour);

            if (result.Length < 2)
            {
                result = $"0{result}";
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var valid = int.TryParse((string)value, out var hour);
            if (valid)
            {
                if (hour is > 23 or < 0)
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