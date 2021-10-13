using System;
using System.Runtime.CompilerServices;

namespace Light
{
    public static class DebugConsole
    {
        public static void Print(string str, [CallerMemberName] string callerName = null)
        {
            Console.WriteLine($"[{callerName}] {str}");
        }
    }
}