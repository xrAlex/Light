using System;
using System.Globalization;
using System.Windows.Data;

namespace Sparky.Converters.XAMLConverters
{
    [ValueConversion(typeof(double), typeof(string))]
    internal sealed class BrightnessToPercent : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = System.Convert.ToDouble(value);
            return $"{Localization.LangDictionary.GetString("Loc_Brightness")}: {Math.Round(val * 100)} %";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
