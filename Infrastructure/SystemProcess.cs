﻿using Light.WinApi;
using System;
using System.Text;

namespace Light.Infrastructure
{
    public class SystemProcess : IDisposable
    {
        private nint Handle { get; }
        private SystemProcess(nint handle) => Handle = handle;

        /// <summary>
        /// Method tries to get the executable path of the process
        /// </summary>
        /// <returns> If successful, returns executable path of process </returns>
        public string TryGetProcessPath()
        {
            var buffer = new StringBuilder(1024);
            var bufferSize = (uint)buffer.Capacity + 1;

            return Native.QueryFullProcessImageName(Handle, 0, buffer, ref bufferSize) ? buffer.ToString() : null;
        }

        /// <summary>
        /// Gets process id by window handle
        /// </summary>
        /// <returns> Process id </returns>
        public static uint GetId(nint handle)
        {
            Native.GetWindowThreadProcessId(handle, out var pId);
            return pId;
        }

        /// <summary>
        /// Method tries to get system process object
        /// </summary>
        /// <returns> If successful, returns an object of type SystemProcess </returns>
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