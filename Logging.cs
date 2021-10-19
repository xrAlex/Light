using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace Sparky
{
    public class LoggingModule
    {
        private const string FilePath = ".\\";
        private bool _initialized;
        private static Assembly Assembly { get; } = typeof(App).Assembly;
        public static Logger Log;

        //string FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log");
        //    if (!Directory.Exists(pathToLog))
        //Directory.CreateDirectory(pathToLog);

        private void SetupExceptionHandling()
        {
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                Log.Fatal((Exception) e.ExceptionObject, "UnhandledException");
            };
  
            Application.Current.DispatcherUnhandledException += (s, e) =>
            {
                Log.Fatal(e.Exception, "DispatcherUnhandledException");
                e.Handled = true;
            };

            TaskScheduler.UnobservedTaskException += (s, e) =>
            {
                Log.Fatal(e.Exception, "UnobservedTaskException");
                e.SetObserved();
            };
        }

        public void Initialize()
        {
            if (_initialized) return;
            var filename = Path.Combine(FilePath, $"{Assembly.GetName().Name}_logs.log");

            Log = new LoggerConfiguration()
                .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Verbose)
                .WriteTo.File(
                    filename,
                    restrictedToMinimumLevel: LogEventLevel.Information,
                    rollingInterval: RollingInterval.Day,
                    rollOnFileSizeLimit: true,
                    retainedFileCountLimit: 5
                )
                .CreateLogger();

            SetupExceptionHandling();
            var info = CollectLaunchInformation();

            Log.Information(info);
            _initialized = true;
        }

        private string CollectLaunchInformation()
        {
            var baseInformation = "";
            try
            {
                var commandLine = Environment
                    .GetCommandLineArgs()
                    .Skip(1)
                    .Aggregate("", (current, arg) => current + arg);

                baseInformation =
                    "\n" +
                    $"{AppDomain.CurrentDomain.FriendlyName}" +
                    $" Build {Assembly.GetName().Version} \r\n" +
                    $"Command line: {commandLine} \r\n" +
                    "Github: https://github.com/xrAlex/Light \r\n" +
                    $"User system: {RuntimeInformation.OSDescription}({RuntimeInformation.ProcessArchitecture}) \r\n" +
                    $"User machine name {Environment.MachineName} \r\n" +
                    "Initializing Application... \r\n";
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Error when collect launch information");
            }

            return baseInformation;
        }
    }
}