#region

using System.Runtime.InteropServices;
using System.Text;

#endregion

namespace Light.Native
{
    internal static class Kernel32
    {
        private const string Dll = "Kernel32.dll";

        [DllImport(Dll)]
        public static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder buffer, int size, string path);

        [DllImport(Dll)]
        public static extern int WritePrivateProfileString(string section, string key, string str, string path);
    }
}
