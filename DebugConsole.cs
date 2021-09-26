#region

using System.Diagnostics;
using System.Security;

#endregion

namespace Light
{
    [SuppressUnmanagedCodeSecurity]
    public static class DebugConsole
    {
        public static void Print(string str) => Trace.WriteLine(str);
    }
}