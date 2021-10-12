﻿using System;
using System.Globalization;
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
        public static T GetValue<T>(string section, string key, string defaultValue) where T : IConvertible
        {
            StringBuilder buffer = new(MaxStringSize);
            Native.GetPrivateProfileString(section, key, defaultValue, buffer, MaxStringSize, Path);

            return (T)Convert.ChangeType(buffer.ToString(), typeof(T));
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