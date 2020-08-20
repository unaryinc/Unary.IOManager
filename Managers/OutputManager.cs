using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Unary.IOManager.Native;

namespace Unary.IOManager.Managers
{
    public class OutputManager : IEnumerable<Input>
    {
        private readonly List<Input> List;

        public OutputManager()
        {
            List = new List<Input>();
        }

        public Input[] ToArray()
        {
            return List.ToArray();
        }

        public IEnumerator<Input> GetEnumerator()
        {
            return List.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Input this[int position]
        {
            get
            {
                return List[position];
            }
        }

        public static bool IsExtendedKey(VirtualKeyCode keyCode)
        {
            if (keyCode == VirtualKeyCode.MENU ||
                keyCode == VirtualKeyCode.RMENU ||
                keyCode == VirtualKeyCode.CONTROL ||
                keyCode == VirtualKeyCode.RCONTROL ||
                keyCode == VirtualKeyCode.INSERT ||
                keyCode == VirtualKeyCode.DELETE ||
                keyCode == VirtualKeyCode.HOME ||
                keyCode == VirtualKeyCode.END ||
                keyCode == VirtualKeyCode.PRIOR ||
                keyCode == VirtualKeyCode.NEXT ||
                keyCode == VirtualKeyCode.RIGHT ||
                keyCode == VirtualKeyCode.UP ||
                keyCode == VirtualKeyCode.LEFT ||
                keyCode == VirtualKeyCode.DOWN ||
                keyCode == VirtualKeyCode.NUMLOCK ||
                keyCode == VirtualKeyCode.CANCEL ||
                keyCode == VirtualKeyCode.SNAPSHOT ||
                keyCode == VirtualKeyCode.DIVIDE)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public OutputManager AddKeyDown(VirtualKeyCode keyCode)
        {
            Input Down = new Input
            {
                Type = (uint)InputType.Keyboard,
                Data =
                {
                    Keyboard = new KeyboardInput
                    {
                        KeyCode = (ushort)keyCode,
                        Scan = (ushort)(Methods.MapVirtualKey((uint)keyCode, 0) & 0xFFU),
                        Flags = IsExtendedKey(keyCode) ? (uint)KeyboardFlag.ExtendedKey : 0,
                        Time = 0,
                        ExtraInfo = IntPtr.Zero
                    }
                }
            };

            List.Add(Down);
            return this;
        }

        public OutputManager AddKeyUp(VirtualKeyCode keyCode)
        {
            Input Up = new Input
            {
                Type = (uint)InputType.Keyboard,
                Data =
                {
                    Keyboard = new KeyboardInput
                    {
                        KeyCode = (ushort)keyCode,
                        Scan = (ushort)(Methods.MapVirtualKey((uint)keyCode, 0) & 0xFFU),
                        Flags = (uint) (IsExtendedKey(keyCode) ? KeyboardFlag.KeyUp | 
                        KeyboardFlag.ExtendedKey : KeyboardFlag.KeyUp),
                        Time = 0,
                        ExtraInfo = IntPtr.Zero
                    }
                }
            };

            List.Add(Up);
            return this;
        }

        public OutputManager AddKeyPress(VirtualKeyCode keyCode)
        {
            AddKeyDown(keyCode);
            AddKeyUp(keyCode);
            return this;
        }

        public OutputManager AddCharacter(char character)
        {
            ushort scanCode = character;

            Input Down = new Input
            {
                Type = (uint)InputType.Keyboard,
                Data =
                {
                    Keyboard = new KeyboardInput
                    {
                        KeyCode = 0,
                        Scan = scanCode,
                        Flags = (uint)KeyboardFlag.Unicode,
                        Time = 0,
                        ExtraInfo = IntPtr.Zero
                    }
                }
            };

            Input Up = new Input
            {
                Type = (uint)InputType.Keyboard,
                Data =
                {
                    Keyboard = new KeyboardInput
                    {
                        KeyCode = 0,
                        Scan = scanCode,
                        Flags = (uint)(KeyboardFlag.KeyUp | KeyboardFlag.Unicode),
                        Time = 0,
                        ExtraInfo = IntPtr.Zero
                    }
                }
            };

            if ((scanCode & 0xFF00) == 0xE000)
            {
                Down.Data.Keyboard.Flags |= (uint)KeyboardFlag.ExtendedKey;
                Up.Data.Keyboard.Flags |= (uint)KeyboardFlag.ExtendedKey;
            }

            List.Add(Down);
            List.Add(Up);
            return this;
        }

        public OutputManager AddCharacters(IEnumerable<char> characters)
        {
            foreach (var character in characters)
            {
                AddCharacter(character);
            }
            return this;
        }

        public OutputManager AddCharacters(string characters)
        {
            return AddCharacters(characters.ToCharArray());
        }

        public OutputManager AddRelativeMouseMovement(int x, int y)
        {
            Input Movement = new Input { Type = (uint)InputType.Mouse };
            Movement.Data.Mouse.Flags = (uint)MouseFlag.Move;
            Movement.Data.Mouse.X = x;
            Movement.Data.Mouse.Y = y;

            List.Add(Movement);

            return this;
        }

        public OutputManager AddAbsoluteMouseMovement(int absoluteX, int absoluteY)
        {
            Input Movement = new Input { Type = (uint)InputType.Mouse };
            Movement.Data.Mouse.Flags = (uint)(MouseFlag.Move | MouseFlag.Absolute);
            Movement.Data.Mouse.X = absoluteX;
            Movement.Data.Mouse.Y = absoluteY;

            List.Add(Movement);

            return this;
        }

        public OutputManager AddAbsoluteMouseMovementOnVirtualDesktop(int absoluteX, int absoluteY)
        {
            var Movement = new Input { Type = (uint)InputType.Mouse };
            Movement.Data.Mouse.Flags = (uint)(MouseFlag.Move | MouseFlag.Absolute | MouseFlag.VirtualDesk);
            Movement.Data.Mouse.X = absoluteX;
            Movement.Data.Mouse.Y = absoluteY;

            List.Add(Movement);

            return this;
        }

        public OutputManager AddMouseButtonDown(MouseButton button)
        {
            var Down = new Input { Type = (uint)InputType.Mouse };
            Down.Data.Mouse.Flags = (uint)ToMouseButtonDownFlag(button);
            List.Add(Down);

            return this;
        }

        public OutputManager AddMouseXButtonDown(int xButtonId)
        {
            var Down = new Input { Type = (uint)InputType.Mouse };
            Down.Data.Mouse.Flags = (uint)MouseFlag.XDown;
            Down.Data.Mouse.MouseData = (uint)xButtonId;
            List.Add(Down);

            return this;
        }

        public OutputManager AddMouseButtonUp(MouseButton button)
        {
            var Up = new Input { Type = (uint)InputType.Mouse };
            Up.Data.Mouse.Flags = (uint)ToMouseButtonUpFlag(button);
            List.Add(Up);

            return this;
        }

        public OutputManager AddMouseXButtonUp(int xButtonId)
        {
            var Up = new Input { Type = (uint)InputType.Mouse };
            Up.Data.Mouse.Flags = (uint)MouseFlag.XUp;
            Up.Data.Mouse.MouseData = (uint)xButtonId;
            List.Add(Up);

            return this;
        }

        public OutputManager AddMouseButtonClick(MouseButton button)
        {
            return AddMouseButtonDown(button).AddMouseButtonUp(button);
        }

        public OutputManager AddMouseXButtonClick(int xButtonId)
        {
            return AddMouseXButtonDown(xButtonId).AddMouseXButtonUp(xButtonId);
        }

        public OutputManager AddMouseButtonDoubleClick(MouseButton button)
        {
            return AddMouseButtonClick(button).AddMouseButtonClick(button);
        }

        public OutputManager AddMouseXButtonDoubleClick(int xButtonId)
        {
            return AddMouseXButtonClick(xButtonId).AddMouseXButtonClick(xButtonId);
        }

        public OutputManager AddMouseVerticalWheelScroll(int scrollAmount)
        {
            var Scroll = new Input { Type = (uint)InputType.Mouse };
            Scroll.Data.Mouse.Flags = (uint)MouseFlag.VerticalWheel;
            Scroll.Data.Mouse.MouseData = (uint)scrollAmount;
            List.Add(Scroll);

            return this;
        }

        public OutputManager AddMouseHorizontalWheelScroll(int scrollAmount)
        {
            var Scroll = new Input { Type = (uint)InputType.Mouse };
            Scroll.Data.Mouse.Flags = (uint)MouseFlag.HorizontalWheel;
            Scroll.Data.Mouse.MouseData = (uint)scrollAmount;

            List.Add(Scroll);

            return this;
        }

        private static MouseFlag ToMouseButtonDownFlag(MouseButton button)
        {
            switch (button)
            {
                case MouseButton.LeftButton:
                    return MouseFlag.LeftDown;

                case MouseButton.MiddleButton:
                    return MouseFlag.MiddleDown;

                case MouseButton.RightButton:
                    return MouseFlag.RightDown;

                default:
                    return MouseFlag.LeftDown;
            }
        }

        private static MouseFlag ToMouseButtonUpFlag(MouseButton button)
        {
            switch (button)
            {
                case MouseButton.LeftButton:
                    return MouseFlag.LeftUp;

                case MouseButton.MiddleButton:
                    return MouseFlag.MiddleUp;

                case MouseButton.RightButton:
                    return MouseFlag.RightUp;

                default:
                    return MouseFlag.LeftUp;
            }
        }

        public bool Dispatch()
        {
            Input[] Result = ToArray();
            var Successfull = Methods.SendInput((uint)Result.Length, Result, Marshal.SizeOf(typeof(Input)));
            if(Successfull != Result.Length)
            {
                return false;
            }

            return true;
        }
    }
}
