using System;
using System.Runtime.InteropServices;

namespace Unary.IOManager.Native
{
    public static class Methods
    {
        public delegate IntPtr HookProc(int code, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern short GetKeyState(ushort virtualKeyCode);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint SendInput(uint numberOfInputs, Input[] inputs, int sizeOfInputStructure);

        [DllImport("user32.dll")]
        public static extern uint MapVirtualKey(uint uCode, uint uMapType);

        [DllImport("user32", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        public static extern IntPtr SetWindowsHookEx(int idHook, HookProc callback, IntPtr hInstance, uint threadId);

        [DllImport("user32", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        public static extern bool UnhookWindowsHookEx(IntPtr hInstance);

        [DllImport("user32", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        public static extern IntPtr CallNextHookEx(IntPtr idHook, int nCode, int wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        public static extern IntPtr LoadLibrary(string lpFileName);
    }
}