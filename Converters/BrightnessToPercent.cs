#region

using System;
using System.Globalization;
using System.Windows.Data;

#endregion

namespace Light.Converters
{
    internal class BrightnessToPercent : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => System.Convert.ToSingle(value) * 100;
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => System.Convert.ToSingle(value) * 0.01;
    }
}
