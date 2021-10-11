using System;
using System.Globalization;
using System.Windows.Data;

namespace Light.Converters.XAMLConverters
{
    [ValueConversion(typeof(double), typeof(string))]
    internal sealed class BrightnessToPercent : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = System.Convert.ToDouble(value);
            return $"{Localization.LangDictionary.GetString("Loc_Brightness")}: {Math.Round(val)} %";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
