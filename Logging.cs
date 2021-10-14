using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace Light
{
    public class Logging
    {
        private const string FilePath = ".\\";

        //string FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log");
        //    if (!Directory.Exists(pathToLog))
        //Directory.CreateDirectory(pathToLog);

        private static readonly object Sync = new();
        public static void Write(string logMessage, Exception ex = null, [CallerMemberName] string callerName = null)
        {
            var filename = Path.Combine(FilePath, $"{AppDomain.CurrentDomain.FriendlyName}_{DateTime.Now:dd.MM.yyy}.log");
            var fullText = $"[{DateTime.Now:dd.MM.yyy HH:mm:ss.fff}] " +  $"Caller: {callerName}\n" +
                           $"[{logMessage}]\n" +
                           $"[{ex?.TargetSite.DeclaringType}.{ex?.TargetSite.Name}()] " +
                           $"{ex?.Message}]\n" +
                           $"[StackTrace]\n{ex?.StackTrace}\r\n\n";
            lock (Sync)
            {
                File.AppendAllText(filename, fullText, Encoding.GetEncoding("Windows-1251"));
            }
        }
    }

    public static class DebugConsole
    {
        public static void Print(string str, [CallerMemberName] string callerName = null)
        {
            Console.WriteLine($"[{callerName}] {str}");
        }
    }
}