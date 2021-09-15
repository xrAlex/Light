using System;
using System.Text;

namespace Light.Infrastructure
{
    public static class INIManager
    {
        private const string Path = ".\\config.ini";
        private const int MaxSize = 1024;

        public static T GetValue<T>(string section, string key, string def) where T : IConvertible
        {
            StringBuilder buffer = new(MaxSize);
            Native.Kernel32.GetPrivateString(section, key, def, buffer, MaxSize, Path);

            return (T)Convert.ChangeType(buffer.ToString(), typeof(T));
        }

        public static void WriteValue(string section, string key, string value) => Native.Kernel32.WritePrivateString(section, key, value, Path);
    }
}