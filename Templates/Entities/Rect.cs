﻿#region

using System.Runtime.InteropServices;

#endregion

namespace Light.Templates.Entities
{
    [StructLayout(LayoutKind.Sequential)]
    internal readonly struct Rect
    {
        public int Left { get; }

        public int Top { get; }

        public int Right { get; }

        public int Bottom { get; }

        public Rect(int left, int top, int right, int bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }
    }
}