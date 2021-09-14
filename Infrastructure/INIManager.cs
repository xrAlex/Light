using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Light.Infrastructure
{
    public class INIManager
    {
        private const string Path = ".\\config.ini";
        private const int MaxSize = 1024;

        public T GetValue<T>(string section, string key, string def) where T : IConvertible
        {
            StringBuilder buffer = new(MaxSize);
            GetPrivateString(section, key, def, buffer, MaxSize, Path);

            return (T)Convert.ChangeType(buffer.ToString(), typeof(T));
        }

        public void WriteValue(string section, string key, string value) => WritePrivateString(section, key, value, Path);

        #region DllImport

        [DllImport("kernel32.dll", EntryPoint = "GetPrivateProfileString")]
        private static extern int GetPrivateString(string section, string key, string def, StringBuilder buffer,
            int size, string path);

        [DllImport("kernel32.dll", EntryPoint = "WritePrivateProfileString")]
        private static extern int WritePrivateString(string section, string key, string str, string path);

        #endregion
    }
}