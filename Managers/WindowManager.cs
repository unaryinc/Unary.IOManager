using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Unary.IOManager.Native;

namespace Unary.IOManager.Managers
{
    public class WindowManager
    {
        private Process Process;
        
        private IntPtr WindowHandle;

        private Methods.HookProc WindowHook;
        private Methods.HookProc KeyboardHook;
        private Methods.HookProc MouseHook;

        private IntPtr WindowHookHandle;
        private IntPtr KeyboardHookHandle;
        private IntPtr MouseHookHandle;

        public WindowManager(Process TargetProcess)
        {
            Process = TargetProcess;
            WindowHandle = Process.MainWindowHandle;
            WindowHookHandle = Methods.SetWindowsHookEx((int)HookType.WH_CALLWNDPROC, OnWindow, WindowHandle, 0);
            KeyboardHookHandle = Methods.SetWindowsHookEx((int)HookType.WH_KEYBOARD_LL, OnKeyboard, WindowHandle, 0);
            MouseHookHandle = Methods.SetWindowsHookEx((int)HookType.WH_MOUSE_LL, OnMouse, WindowHandle, 0);
        }

        public IntPtr OnWindow(int code, IntPtr wParam, IntPtr lParam)
        {


            return Methods.CallNextHookEx(WindowHookHandle, code, (int)wParam, lParam);
        }

        public IntPtr OnKeyboard(int code, IntPtr wParam, IntPtr lParam)
        {
            if (code >= 0 && wParam == (IntPtr)KeyboardInputType.WM_KEYDOWN)
            {
                VirtualKeyCode KeyCode = (VirtualKeyCode)(ushort)Marshal.ReadIntPtr(lParam);

                Console.WriteLine(KeyCode);
            }

            return Methods.CallNextHookEx(KeyboardHookHandle, code, (int)wParam, lParam);
        }

        public IntPtr OnMouse(int code, IntPtr wParam, IntPtr lParam)
        {


            return Methods.CallNextHookEx(MouseHookHandle, code, (int)wParam, lParam);
        }

        ~WindowManager()
        {
            Methods.UnhookWindowsHookEx(WindowHookHandle);
            Methods.UnhookWindowsHookEx(KeyboardHookHandle);
            Methods.UnhookWindowsHookEx(MouseHookHandle);
        }
    }
}
