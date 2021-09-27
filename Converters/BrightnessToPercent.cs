#region

using System;
using System.Globalization;
using System.Windows.Data;

#endregion

namespace Light.Converters
{
    [ValueConversion(typeof(double), typeof(string))]
    internal class BrightnessToPercent : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => System.Convert.ToSingle(value) * 100;
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => System.Convert.ToSingle(value) * 0.01;
    }
}
