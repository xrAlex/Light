using System;
using System.Diagnostics;
using System.Reflection;
using Microsoft.Win32;

namespace Light.Models
{
    /// <summary>
    /// A model for working with register entries
    /// </summary>
    internal sealed class RegistryModel
    {
        private static readonly string AppNameKey = $"{AppDomain.CurrentDomain.FriendlyName}";
        private const string StartupPath = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";
        private const string GetGammaRangeValue = "REG QUERY \"HKLM\\Software\\Microsoft\\Windows NT\\CurrentVersion\\ICM\" /v GdiICMGammaRange";
        private const string SetDefaultGammaRangeValue = "REG ADD \"HKLM\\Software\\Microsoft\\Windows NT\\CurrentVersion\\ICM\" /v GdiICMGammaRange /t REG_DWORD /d 0 /f";
        private const string SetExtendedGammaRangeValue = "REG ADD \"HKLM\\Software\\Microsoft\\Windows NT\\CurrentVersion\\ICM\" /v GdiICMGammaRange /t REG_DWORD /d 256 /f";

        public bool IsAppStartupKeyFounded() => IsRegistryKeyFounded(Registry.CurrentUser, StartupPath, AppNameKey) != null;
        public void DeleteAppStartupKey() => DeleteKey(Registry.CurrentUser, StartupPath, AppNameKey);
        public void AddAppStartupKey() => AddKey(Registry.CurrentUser, StartupPath, AppNameKey, GetAppExecutingLocation);

        public bool IsExtendedGammaRangeActive() 
        {
            var output = ExecuteFromCMD(GetGammaRangeValue, false, true);
            return output.Contains("0x100");
        }

        public void SetDefaultGammaRangeKey() => ExecuteFromCMD(SetDefaultGammaRangeValue, true, false);
        public void SetExtendedGammaRangeKey() => ExecuteFromCMD(SetExtendedGammaRangeValue, true, false);

        /// <summary>
        /// Executes the command using CMD
        /// </summary>
        /// <remarks> It is used in order not to request administrator rights when starting the program </remarks>
        /// <returns> If redirectOutput = true, returns result of executing command </returns>
        private string ExecuteFromCMD(string command, bool shellExecute, bool redirectOutput)
        {
            var output = "";
            var processStartInfo = new ProcessStartInfo
            {
                UseShellExecute = shellExecute,
                FileName = "CMD.exe",
                Arguments = "/C " + command,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                Verb = "runas",
                RedirectStandardOutput = !shellExecute && redirectOutput
            };

            using var cmdProcess = Process.Start(processStartInfo);
            if (redirectOutput)
            {
                using var reader = cmdProcess?.StandardOutput;
                output = reader?.ReadToEnd();
            }
            cmdProcess?.WaitForExit();

            return output;
        }
        private RegistryKey GetSubKey(RegistryKey key, string subKey) => key.OpenSubKey(subKey, true);

        private object IsRegistryKeyFounded(RegistryKey key, string subKey, string valueName)
        {
            using var registryKey = key.OpenSubKey(subKey, true);
            var value = registryKey?.GetValue(valueName);
            return value;
        }

        private void AddKey<T>(RegistryKey key, string subKey, string valueName, T value)
        {
            using var registryKey = GetSubKey(key, subKey);
            registryKey?.SetValue(valueName, value);
        }

        private void DeleteKey(RegistryKey key, string subKey, string valueName)
        {
            using var registryKey = GetSubKey(key, subKey);
            registryKey?.DeleteValue(valueName, true);
        }

        private string GetAppExecutingLocation => $"\"{Assembly.GetExecutingAssembly().Location}\"";

#if DEBUG
        ~RegistryModel()
        {
            Debug.Print("RegistryModel Disposed");
        }
#endif
    }

}
