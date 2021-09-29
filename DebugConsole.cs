using System.Diagnostics;

namespace Light
{
    public static class DebugConsole
    {
        public static void Print(string str) => Trace.WriteLine(str);
    }
}