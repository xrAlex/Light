using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Light.Native
{
    internal static class Kernel32
    {

        [DllImport("kernel32.dll")]
        public static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder buffer, int size, string path);

        [DllImport("kernel32.dll")]
        public static extern int WritePrivateProfileString(string section, string key, string str, string path);
    }
}
