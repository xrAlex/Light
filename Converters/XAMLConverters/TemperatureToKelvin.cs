using System;
using System.Globalization;
using System.Windows.Data;

namespace Light.Converters.XAMLConverters
{
    [ValueConversion(typeof(double), typeof(string))]
    internal sealed class TemperatureToKelvin : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = System.Convert.ToDouble(value);
            return $"{Localization.LangDictionary.GetString("Loc_ColorTemperature")}: {Math.Round(val)} К";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
