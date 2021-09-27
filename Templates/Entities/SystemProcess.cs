using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Light.WinApi;

namespace Light.Templates.Entities
{
    public class SystemProcess : IDisposable
    {
        public nint Handle { get; }
        public SystemProcess(nint handle) => Handle = handle;

        public void Dispose()
        {
            if (Native.CloseHandle(Handle))
            {
                GC.SuppressFinalize(this);
            }
        }

        public string? TryGetExecutableFilePath()
        {
            var buffer = new StringBuilder(1024);
            var bufferSize = (uint)buffer.Capacity + 1;

            return Native.QueryFullProcessImageName(Handle, 0, buffer, ref bufferSize)
                ? buffer.ToString()
                : null;
        }

        public static SystemProcess TryOpen(uint processId)
        {
            var handle = Native.OpenProcess(ProcessAccessFlags.QueryLimitedInformation, false, processId);
            return handle != 0? new SystemProcess(handle) : null;
        }

        ~SystemProcess() => Dispose();
    }
}
