using Light.Templates.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Light.WinApi
{
    internal static partial class Native
    {
        [DllImport("shell32.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern nint SHAppBarMessage(uint dwMessage, ref TaskBarData pData);
    }
}
