#region

using System;
using System.Runtime.InteropServices;
using System.Text;

#endregion

namespace Light.Native
{
    [Flags]
    internal enum ProcessAccessFlags : uint
    {
        All = 0x001F0FFF,
        Terminate = 0x00000001,
        CreateThread = 0x00000002,
        VirtualMemoryOperation = 0x00000008,
        VirtualMemoryRead = 0x00000010,
        VirtualMemoryWrite = 0x00000020,
        DuplicateHandle = 0x00000040,
        CreateProcess = 0x000000080,
        SetQuota = 0x00000100,
        SetInformation = 0x00000200,
        QueryInformation = 0x00000400,
        QueryLimitedInformation = 0x00001000,
        Synchronize = 0x00100000
    }
    //ProcessAccessFlags.QueryLimitedInformation
    internal static class Kernel32
    {
        private const string Dll = "Kernel32.dll";

        [DllImport(Dll, SetLastError = true)]
        public static extern int GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, int nSize, string lpFileName);

        [DllImport(Dll, SetLastError = true)]
        public static extern int WritePrivateProfileString(string lpAppName, string lpKeyName, string lpString, string lpFileName);

        public delegate bool EnumWindowsProc(nint hWnd, nint lParam);

        [DllImport(Dll, SetLastError = true)]
        public static extern nint OpenProcess(ProcessAccessFlags processAccess, bool bInheritHandle, uint processId);

        [DllImport(Dll, SetLastError = true)]
        public static extern bool QueryFullProcessImageName(nint hPrc, uint dwFlags, StringBuilder lpExeName, ref uint lpdwSize);

        [DllImport(Dll, SetLastError = true)]
        public static extern bool CloseHandle(nint hObj);
    }
}
