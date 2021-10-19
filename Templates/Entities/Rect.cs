using System.Runtime.InteropServices;

namespace Sparky.Templates.Entities
{
    [StructLayout(LayoutKind.Sequential)]
    internal readonly struct Rect
    {
        public int Left { get; }

        public int Top { get; }

        public int Right { get; }

        public int Bottom { get; }
    }
}