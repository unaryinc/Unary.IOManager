using System;

namespace Unary.IOManager.Native
{
    public struct KeyboardInput
    {
        public ushort KeyCode;
        public ushort Scan;
        public uint Flags;
        public uint Time;
        public IntPtr ExtraInfo;
    }
}
