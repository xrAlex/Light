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

        private readonly string path = ".\\config.ini";
        private const int SIZE = 1024;

        private string GetValue(string section, string key, string def)
        {
            StringBuilder buffer = new(SIZE);
            GetPrivateString(section, key, def, buffer, SIZE, path);
            return buffer.ToString();
        }

        public string GetStringValue(string section, string key, string def) => GetValue(section, key, def);
        public int GetIntValue(string section, string key, string def) => Convert.ToInt32(GetValue(section, key, def));
        public float GetFloatValue(string section, string key, string def) => (float)Convert.ToDouble(GetValue(section, key, def));
        public bool GetBoolValue(string section, string key, string def) => Convert.ToBoolean(GetValue(section, key, def));
        public void WriteValue(string Section, string Key, string Value) => WritePrivateString(Section, Key, Value, path);
    }
}
