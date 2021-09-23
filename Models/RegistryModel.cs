using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace Light.Models
{
    public class RegistryModel
    {
        private const string AppNameKey = "Light";
        private const string StartupPath = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";
        private const string GammaRangeKey = "GdiICMGammaRange";
        private const string GammaRangePath = "Software\\Microsoft\\Windows NT\\CurrentVersion\\ICM";

        public bool IsAppStartupKeyFounded() => IsRegistryKeyFounded(Registry.CurrentUser, StartupPath, AppNameKey) != null;
        public bool IsExtendedGammaRangeActive() 
        {
            var value = IsRegistryKeyFounded(Registry.LocalMachine, GammaRangePath, GammaRangeKey);
            return value != null && Convert.ToInt32(value) == 256;
        }

        public void DeleteAppStartupKey() => DeleteKey(Registry.CurrentUser, StartupPath, AppNameKey);
        public void AddAppStartupKey() => AddKey(Registry.CurrentUser, StartupPath, AppNameKey, GetAppExecutingLocation);
        public void SetDefaultGammaRangeKey() => DeleteKey(Registry.LocalMachine, GammaRangePath, GammaRangeKey);
        public void SetExtendedGammaRangeKey() => AddKey(Registry.LocalMachine, GammaRangePath, GammaRangeKey, 256);
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
    }
}
