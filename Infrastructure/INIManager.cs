using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Text;
using Light.WinApi;

namespace Light.Infrastructure
{
    /// <summary>
    /// Contains methods for working with .ini files
    /// </summary>
    internal static class INIManager
    {
        private const string Path = ".\\config.ini";
        private const int MaxStringSize = 1024;

        /// <summary>
        /// Converts a string from .ini file to specified type
        /// </summary>
        /// <returns> Converted value <typeparamref name="T"></typeparamref></returns>
        public static T GetValue<T>(string section, string key, T defaultValue) where T : IConvertible
        {
            StringBuilder buffer = new(MaxStringSize);
            Native.GetPrivateProfileString(section, key, defaultValue.ToString(), buffer, MaxStringSize, Path);

            try
            {
                return (T)Convert.ChangeType(buffer.ToString(), typeof(T), CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                Logging.Write("Error when loading settings", ex); ;
                return defaultValue;
            }
        }

        /// <summary>
        /// Writes data to .ini file
        /// </summary>
        public static void WriteValue<T>(string section, string key, T value)
        {
           Native.WritePrivateProfileString(section, key, value.ToString(), Path);
        }
    }
}