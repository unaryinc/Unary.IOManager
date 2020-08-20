using System.Runtime.InteropServices;

namespace Unary.IOManager.Native
{
    [StructLayout(LayoutKind.Explicit)]
    public struct InputData
    {
        [FieldOffset(0)]
        public MouseInput Mouse;
        [FieldOffset(0)]
        public KeyboardInput Keyboard;
        [FieldOffset(0)]
        public HardwareInput Hardware;
    }
}
