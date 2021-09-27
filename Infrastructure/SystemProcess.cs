using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Light.WinApi;

namespace Light.Infrastructure
{
    public class SystemProcess : IDisposable
    {
        private nint Handle { get; }
        private SystemProcess(nint handle) => Handle = handle;

        public void Dispose()
        {
            if (Native.CloseHandle(Handle))
            {
                GC.SuppressFinalize(this);
            }
        }

        /// <summary>
        /// Метод получает id процесса по дескриптору его кона
        /// </summary>
        /// <returns> id процесса </returns>
        public static uint GetId(nint handle)
        {
            Native.GetWindowThreadProcessId(handle, out var pId);
            return pId;
        }

        /// <summary>
        /// Метод пытется получить исполняемый путь процесса
        /// </summary>
        /// <returns> Вслучаем успеха возвращает исполняемый путь процесса </returns>
        public string TryGetProcessPath()
        {
            var buffer = new StringBuilder(1024);
            var bufferSize = (uint)buffer.Capacity + 1;

            return Native.QueryFullProcessImageName(Handle, 0, buffer, ref bufferSize)? buffer.ToString() : null;
        }

        /// <summary>
        /// Метод пытется получить доступ к объекту системного процесса
        /// </summary>
        /// <returns> В случае успеха возвращает объект типа SystemProcess </returns>
        public static SystemProcess TryOpenProcess(uint pId)
        {
            var handle = Native.OpenProcess(ProcessAccessFlags.QueryLimitedInformation, false, pId);
            return handle != 0? new SystemProcess(handle) : null;
        }

        ~SystemProcess() => Dispose();
    }
}
