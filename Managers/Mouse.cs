using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Unary.IOManager.Native;

namespace Unary.IOManager.Managers
{
    public class Mouse
    {
        private App App;

        private readonly OutputFactory OutputManager;

        private Point TempCoords;
        private Point CoordsState;
        private bool LeftPressedState;
        private bool RightPressedState;

        private List<Action<Point>> OnMoveSubscribers;

        private List<Action> OnLeftPressedSubscribers;
        private List<Action> OnLeftReleasedSubscribers;

        private List<Action> OnRightPressedSubscribers;
        private List<Action> OnRightReleasedSubscribers;

        public Mouse(App NewApp, OutputFactory Manager)
        {
            App = NewApp;
            OutputManager = Manager;

            TempCoords = new Point();
            CoordsState = new Point();
            LeftPressedState = IsLeftDown();
            RightPressedState = IsRightDown();

            OnMoveSubscribers = new List<Action<Point>>();

            OnLeftPressedSubscribers = new List<Action>();
            OnLeftReleasedSubscribers = new List<Action>();

            OnRightPressedSubscribers = new List<Action>();
            OnRightReleasedSubscribers = new List<Action>();
        }

        public int MouseWheelClickSize { get; set; } = 120;

        public Mouse MoveMouseBy(int pixelDeltaX, int pixelDeltaY)
        {
            if(!App.Running) { return this; }
            OutputManager.AddRelativeMouseMovement(pixelDeltaX, pixelDeltaY).Dispatch();
            return this;
        }

        public Mouse MoveMouseTo(double absoluteX, double absoluteY)
        {
            if (!App.Running) { return this; }
            OutputManager.AddAbsoluteMouseMovement((int)Math.Truncate(absoluteX), (int)Math.Truncate(absoluteY)).Dispatch();
            return this;
        }

        public Mouse MoveMouseToPositionOnVirtualDesktop(double absoluteX, double absoluteY)
        {
            if (!App.Running) { return this; }
            OutputManager.AddAbsoluteMouseMovementOnVirtualDesktop((int)Math.Truncate(absoluteX), (int)Math.Truncate(absoluteY)).Dispatch();
            return this;
        }

        public Mouse LeftButtonDown()
        {
            if (!App.Running) { return this; }
            OutputManager.AddMouseButtonDown(MouseButton.LeftButton).Dispatch();
            return this;
        }

        public Mouse LeftButtonUp()
        {
            if (!App.Running) { return this; }
            OutputManager.AddMouseButtonUp(MouseButton.LeftButton).Dispatch();
            return this;
        }

        public Mouse LeftButtonClick()
        {
            if (!App.Running) { return this; }
            OutputManager.AddMouseButtonClick(MouseButton.LeftButton).Dispatch();
            return this;
        }

        public Mouse LeftButtonDoubleClick()
        {
            if (!App.Running) { return this; }
            OutputManager.AddMouseButtonDoubleClick(MouseButton.LeftButton).Dispatch();
            return this;
        }

        public Mouse MiddleButtonDown()
        {
            if (!App.Running) { return this; }
            OutputManager.AddMouseButtonDown(MouseButton.MiddleButton).Dispatch();
            return this;
        }

        public Mouse MiddleButtonUp()
        {
            if (!App.Running) { return this; }
            OutputManager.AddMouseButtonUp(MouseButton.MiddleButton).Dispatch();
            return this;
        }

        public Mouse MiddleButtonClick()
        {
            if (!App.Running) { return this; }
            OutputManager.AddMouseButtonClick(MouseButton.MiddleButton).Dispatch();
            return this;
        }

        public Mouse MiddleButtonDoubleClick()
        {
            if (!App.Running) { return this; }
            OutputManager.AddMouseButtonDoubleClick(MouseButton.MiddleButton).Dispatch();
            return this;
        }

        public Mouse RightButtonDown()
        {
            if (!App.Running) { return this; }
            OutputManager.AddMouseButtonDown(MouseButton.RightButton).Dispatch();
            return this;
        }

        public Mouse RightButtonUp()
        {
            if (!App.Running) { return this; }
            OutputManager.AddMouseButtonUp(MouseButton.RightButton).Dispatch();
            return this;
        }

        public Mouse RightButtonClick()
        {
            if (!App.Running) { return this; }
            OutputManager.AddMouseButtonClick(MouseButton.RightButton).Dispatch();
            return this;
        }

        public Mouse RightButtonDoubleClick()
        {
            if (!App.Running) { return this; }
            OutputManager.AddMouseButtonDoubleClick(MouseButton.RightButton).Dispatch();
            return this;
        }

        public Mouse XButtonDown(int buttonId)
        {
            if (!App.Running) { return this; }
            OutputManager.AddMouseXButtonDown(buttonId).Dispatch();
            return this;
        }

        public Mouse XButtonUp(int buttonId)
        {
            if (!App.Running) { return this; }
            OutputManager.AddMouseXButtonUp(buttonId).Dispatch();
            return this;
        }

        public Mouse XButtonClick(int buttonId)
        {
            if (!App.Running) { return this; }
            OutputManager.AddMouseXButtonClick(buttonId).Dispatch();
            return this;
        }

        public Mouse XButtonDoubleClick(int buttonId)
        {
            if (!App.Running) { return this; }
            OutputManager.AddMouseXButtonDoubleClick(buttonId).Dispatch();
            return this;
        }

        public Mouse VerticalScroll(int scrollAmountInClicks)
        {
            if (!App.Running) { return this; }
            OutputManager.AddMouseVerticalWheelScroll(scrollAmountInClicks * MouseWheelClickSize).Dispatch();
            return this;
        }

        public Mouse HorizontalScroll(int scrollAmountInClicks)
        {
            if (!App.Running) { return this; }
            OutputManager.AddMouseHorizontalWheelScroll(scrollAmountInClicks * MouseWheelClickSize).Dispatch();
            return this;
        }

        public bool IsLeftDown()
        {
            if (!App.Running) { return false; }
            short result = Methods.GetKeyState((ushort)VirtualKeyCode.LBUTTON);
            return (result < 0);
        }

        public bool IsLeftUp()
        {
            if (!App.Running) { return true; }
            return !IsLeftDown();
        }

        public bool IsRightDown()
        {
            if (!App.Running) { return false; }
            short result = Methods.GetKeyState((ushort)VirtualKeyCode.RBUTTON);
            return (result < 0);
        }

        public bool IsRightUp()
        {
            if (!App.Running) { return true; }
            return !IsRightDown();
        }

        public Point GetCursor()
        {
            if (!App.Running) { return TempCoords; }
            Methods.GetCursorPos(out TempCoords);
            return TempCoords;
        }

        private void OnMoved()
        {
            foreach(var Subscriber in OnMoveSubscribers)
            {
                Subscriber.Invoke(CoordsState);
            }
        }

        public void Poll()
        {
            Methods.GetCursorPos(out TempCoords);

            if (TempCoords != CoordsState)
            {
                CoordsState = TempCoords;
                OnMoved();
            }

            if(LeftPressedState)
            {
                if(IsLeftUp())
                {
                    LeftPressedState = false;
                    foreach (var Sub in OnLeftReleasedSubscribers)
                    {
                        Sub.Invoke();
                    }
                }
            }
            else
            {
                if (IsLeftDown())
                {
                    LeftPressedState = true;
                    foreach (var Sub in OnLeftPressedSubscribers)
                    {
                        Sub.Invoke();
                    }
                }
            }

            if (RightPressedState)
            {
                if (IsRightUp())
                {
                    RightPressedState = false;
                    foreach (var Sub in OnRightReleasedSubscribers)
                    {
                        Sub.Invoke();
                    }
                }
            }
            else
            {
                if (IsRightDown())
                {
                    RightPressedState = true;
                    foreach (var Sub in OnRightPressedSubscribers)
                    {
                        Sub.Invoke();
                    }
                }
            }
        }

        public void OnMouseMove(Action<Point> NewAction)
        {
            OnMoveSubscribers.Add(NewAction);
        }

        public void OnMouseLeftPressed(Action NewAction)
        {
            OnLeftPressedSubscribers.Add(NewAction);
        }

        public void OnMouseLeftReleased(Action NewAction)
        {
            OnLeftReleasedSubscribers.Add(NewAction);
        }

        public void OnMouseRightPressed(Action NewAction)
        {
            OnRightPressedSubscribers.Add(NewAction);
        }

        public void OnMouseRightReleased(Action NewAction)
        {
            OnRightReleasedSubscribers.Add(NewAction);
        }
    }
}
