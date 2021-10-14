using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            var exText = $"[{DateTime.Now:dd.MM.yyy HH:mm:ss.fff}] " + $"Caller: {callerName}\n" +
                         $"[{logMessage}]\n";
            if (ex != null)
            {
                exText += $"[{ex.TargetSite.DeclaringType}.{ex.TargetSite.Name}()] " +
                          $"{ex.Message}]\n" +
                          $"[StackTrace]\n{ex.StackTrace}";
            }

            exText += "\r\n\n";

            lock (Sync)
            {
#if DEBUG
                Console.WriteLine($"[{callerName}] {exText}");
#endif
                File.AppendAllText(filename, exText, Encoding.GetEncoding("Windows-1251"));
            }
        }
    }
}