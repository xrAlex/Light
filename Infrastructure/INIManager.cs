using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Light.Infrastructure
{
    public class INIManager
    {
        [DllImport("kernel32.dll", EntryPoint = "GetPrivateProfileString")]
        private static extern int GetPrivateString(string section, string key, string def, StringBuilder buffer, int size, string path);

        [DllImport("kernel32.dll", EntryPoint = "WritePrivateProfileString")]
        private static extern int WritePrivateString(string section, string key, string str, string path);

        private const string path = ".\\config.ini";
        private const int SIZE = 1024;

        public T GetValue<T>(string section, string key, string def) where T : IConvertible
        {
            StringBuilder buffer = new(SIZE);
            GetPrivateString(section, key, def, buffer, SIZE, path);

            return (T)Convert.ChangeType(buffer.ToString(), typeof(T));
        }

        public void WriteValue(string section, string key, string value) => WritePrivateString(section, key, value, path);
    }
}
