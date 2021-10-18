using Light.WinApi;
using System;
using System.IO;
using System.Text;

namespace Light.Infrastructure
{
    internal sealed class SystemProcess : IDisposable
    {
        private nint Handle { get; }
        private SystemProcess(nint handle) => Handle = handle;

        /// <summary>
        /// Method tries to get the executable path of the process
        /// </summary>
        /// <returns> If successful, returns <see cref="string"/> executable path of process </returns>
        public string TryGetProcessPath()
        {
            var buffer = new StringBuilder(1024);
            var bufferSize = (uint)buffer.Capacity + 1;

            return Native.QueryFullProcessImageName(Handle, 0, buffer, ref bufferSize) ? buffer.ToString() : null;
        }

        public static string TryGetProcessFileName(nint handle)
        {
            var pId = GetId(handle);
            using var process = TryOpenProcess(pId);
            var processPath = process?.TryGetProcessPath();
            var processFileName = Path.GetFileNameWithoutExtension(processPath);
            if (string.IsNullOrWhiteSpace(processPath) || string.IsNullOrWhiteSpace(processFileName))
            {
                return null;
            }
            return processFileName;
        }


        /// <summary>
        /// Gets process id by window handle
        /// </summary>
        /// <returns> Process <see cref="uint"/> id </returns>
        public static uint GetId(nint handle)
        {
            Native.GetWindowThreadProcessId(handle, out var pId);
            return pId;
        }

        /// <summary>
        /// Method tries to get system process object
        /// </summary>
        /// <returns> If successful, returns an object of type <see cref="SystemProcess"/> </returns>
        public static SystemProcess TryOpenProcess(uint pId)
        {
            var handle = Native.OpenProcess(ProcessAccessFlags.QueryLimitedInformation, false, pId);
            return handle != 0? new SystemProcess(handle) : null;
        }

        public void Dispose()
        {
            if (Native.CloseHandle(Handle))
            {
                GC.SuppressFinalize(this);
            }
        }

        ~SystemProcess() => Dispose();
    }
}
