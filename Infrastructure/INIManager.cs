using System;
using System.Text;

namespace Light.Infrastructure
{
    public static class INIManager
    {
        private const string Path = ".\\config.ini";
        private const int MaxStringSize = 1024;

        public static T GetValue<T>(string section, string key, string def) where T : IConvertible
        {
            StringBuilder buffer = new(MaxStringSize);
            Native.Kernel32.GetPrivateProfileString(section, key, def, buffer, MaxStringSize, Path);

            return (T)Convert.ChangeType(buffer.ToString(), typeof(T));
        }

        public static void WriteValue(string section, string key, string value) => Native.Kernel32.WritePrivateProfileString(section, key, value, Path);
    }
}