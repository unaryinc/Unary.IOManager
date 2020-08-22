using System;

namespace Unary.IOManager.Native
{
    public struct MouseOutput
    {
        public int X;
        public int Y;
        public uint MouseData;
        public uint Flags;
        public uint Time;
        public IntPtr ExtraInfo;
    }
}
