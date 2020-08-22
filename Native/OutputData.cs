using System.Runtime.InteropServices;

namespace Unary.IOManager.Native
{
    [StructLayout(LayoutKind.Explicit)]
    public struct OutputData
    {
        [FieldOffset(0)]
        public MouseOutput Mouse;
        [FieldOffset(0)]
        public KeyboardOutput Keyboard;
        [FieldOffset(0)]
        public HardwareOutput Hardware;
    }
}
