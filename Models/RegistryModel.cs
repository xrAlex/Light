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
        private const string StartupPath = "\"HKLM\\Software\\Microsoft\\Windows\\CurrentVersion\\Run\" ";
        private static readonly string AppNameKey = $"/v {AppDomain.CurrentDomain.FriendlyName} ";
        private static readonly string AppNameParam = $"/t REG_SZ /d \"{Assembly.GetExecutingAssembly().Location} -silent\" /f";

        private const string AddCommand = "REG ADD ";
        private const string CheckCommand = "REG QUERY ";
        private const string DeleteCommand = "REG DELETE ";

        private const string GammaRangePath = "\"HKLM\\Software\\Microsoft\\Windows NT\\CurrentVersion\\ICM\" ";
        private const string GammaRangeKey = "/v GdiICMGammaRange ";
        private const string GammaRangeDefaultParam = "/t REG_DWORD /d 0 /f";
        private const string GammaRangeExtendedParam = "/t REG_DWORD /d 256 /f";

        public bool IsAppStartupKeyFounded()
        {
            var output = ExecuteFromCMD(CheckCommand + StartupPath + AppNameKey, false, true);
            return output.Contains("Light.exe");
        }

        public bool IsExtendedGammaRangeActive()
        {
            var output = ExecuteFromCMD(CheckCommand + GammaRangePath + GammaRangeKey, false, true);
            return output.Contains("0x100");
        }

        public void AddAppStartupKey()
        {
            ExecuteFromCMD(AddCommand + StartupPath + AppNameKey + AppNameParam, true, false);
        }

        public void DeleteAppStartupKey()
        {
            ExecuteFromCMD(DeleteCommand + StartupPath + AppNameKey + "/f", true, false);
        }

        public void SetDefaultGammaRangeKey()
        {
            ExecuteFromCMD(AddCommand + GammaRangePath + GammaRangeKey + GammaRangeDefaultParam, true, false);
        }
        public void SetExtendedGammaRangeKey()
        {
            ExecuteFromCMD(AddCommand + GammaRangePath + GammaRangeKey + GammaRangeExtendedParam, true, false);
        }

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

#if DEBUG
        ~RegistryModel()
        {
            DebugConsole.Print("RegistryModel Disposed");
        }
#endif
    }

}
