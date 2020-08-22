using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security.AccessControl;

namespace Unary.IOManager.Native
{
    public static class Methods
    {
        public struct Rect
        {
            public int X { get; set; }
            public int Y { get; set; }
            public int Z { get; set; }
            public int W { get; set; }

            public static bool operator==(Rect Target, Rect Comparator)
            {
                if (Target.X == Comparator.X && Target.Y == Comparator.Y
                && Target.Z == Comparator.Z && Target.W == Comparator.W)
                {
                    return true;
                }
                return false;
            }

            public static bool operator!=(Rect Target, Rect Comparator)
            {
                if (Target.X != Comparator.X && Target.Y != Comparator.Y
                && Target.Z != Comparator.Z && Target.W != Comparator.W)
                {
                    return true;
                }
                return false;
            }

            public override string ToString()
            {
                string Result = default;
                Result += X + "." + Y + "." + Z + "." + W;
                return Result;
            }

            public override int GetHashCode()
            {
                return ToString().GetHashCode();
            }

            public override bool Equals(object obj)
            {
                if (obj is Rect Target)
                {
                    if (Target.X == X && Target.Y == Y
                    && Target.Z == Z && Target.W == W)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Pointer
        {
            public int X;
            public int Y;
            public static explicit operator Point(Pointer point) => new Point(point.X, point.Y);

            public static bool operator ==(Pointer Target, Pointer Comparator)
            {
                if (Target.X == Comparator.X && Target.Y == Comparator.Y)
                {
                    return true;
                }
                return false;
            }

            public static bool operator !=(Pointer Target, Pointer Comparator)
            {
                if (Target.X != Comparator.X && Target.Y != Comparator.Y)
                {
                    return true;
                }
                return false;
            }

            public override string ToString()
            {
                string Result = default;
                Result += X + "." + Y;
                return Result;
            }

            public override int GetHashCode()
            {
                return ToString().GetHashCode();
            }

            public override bool Equals(object obj)
            {
                if (obj is Rect Target)
                {
                    if (Target.X == X && Target.Y == Y)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        public delegate IntPtr HookProc(int code, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern short GetKeyState(ushort virtualKeyCode);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint SendInput(uint numberOfInputs, Output[] inputs, int sizeOfInputStructure);

        [DllImport("user32.dll")]
        public static extern uint MapVirtualKey(uint uCode, uint uMapType);

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hwnd, ref Rect rectangle);

        [DllImport("user32.dll")]
        public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, uint wFlags);

        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(out Pointer lpPoint);

        public static void GetCursorPos(out Point Output)
        {
            GetCursorPos(out Pointer lpPoint);
            Output = (Point)lpPoint;
        }

    }
}