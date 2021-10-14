using System;
using System.Globalization;
using System.Windows.Data;

namespace Light.Converters.XAMLConverters
{
    [ValueConversion(typeof(string), null)]
    internal sealed class LocalizationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var localizationKey = (string)value;
            var result = Localization.LangDictionary.GetString(localizationKey);
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
