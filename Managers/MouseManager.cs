using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Unary.IOManager.Native;

namespace Unary.IOManager.Managers
{
    public class MouseManager
    {
        private readonly OutputManager OutputManager;

        public MouseManager(OutputManager Manager)
        {
            OutputManager = Manager;
        }

        public int MouseWheelClickSize { get; set; } = 120;

        public MouseManager MoveMouseBy(int pixelDeltaX, int pixelDeltaY)
        {
            OutputManager.AddRelativeMouseMovement(pixelDeltaX, pixelDeltaY).Dispatch();
            return this;
        }

        public MouseManager MoveMouseTo(double absoluteX, double absoluteY)
        {
            OutputManager.AddAbsoluteMouseMovement((int)Math.Truncate(absoluteX), (int)Math.Truncate(absoluteY)).Dispatch();
            return this;
        }

        public MouseManager MoveMouseToPositionOnVirtualDesktop(double absoluteX, double absoluteY)
        {
            OutputManager.AddAbsoluteMouseMovementOnVirtualDesktop((int)Math.Truncate(absoluteX), (int)Math.Truncate(absoluteY)).Dispatch();
            return this;
        }

        public MouseManager LeftButtonDown()
        {
            OutputManager.AddMouseButtonDown(MouseButton.LeftButton).Dispatch();
            return this;
        }

        public MouseManager LeftButtonUp()
        {
            OutputManager.AddMouseButtonUp(MouseButton.LeftButton).Dispatch();
            return this;
        }

        public MouseManager LeftButtonClick()
        {
            OutputManager.AddMouseButtonClick(MouseButton.LeftButton).Dispatch();
            return this;
        }

        public MouseManager LeftButtonDoubleClick()
        {
            OutputManager.AddMouseButtonDoubleClick(MouseButton.LeftButton).Dispatch();
            return this;
        }

        public MouseManager MiddleButtonDown()
        {
            OutputManager.AddMouseButtonDown(MouseButton.MiddleButton).Dispatch();
            return this;
        }

        public MouseManager MiddleButtonUp()
        {
            OutputManager.AddMouseButtonUp(MouseButton.MiddleButton).Dispatch();
            return this;
        }

        public MouseManager MiddleButtonClick()
        {
            OutputManager.AddMouseButtonClick(MouseButton.MiddleButton).Dispatch();
            return this;
        }

        public MouseManager MiddleButtonDoubleClick()
        {
            OutputManager.AddMouseButtonDoubleClick(MouseButton.MiddleButton).Dispatch();
            return this;
        }

        public MouseManager RightButtonDown()
        {
            OutputManager.AddMouseButtonDown(MouseButton.RightButton).Dispatch();
            return this;
        }

        public MouseManager RightButtonUp()
        {
            OutputManager.AddMouseButtonUp(MouseButton.RightButton).Dispatch();
            return this;
        }

        public MouseManager RightButtonClick()
        {
            OutputManager.AddMouseButtonClick(MouseButton.RightButton).Dispatch();
            return this;
        }

        public MouseManager RightButtonDoubleClick()
        {
            OutputManager.AddMouseButtonDoubleClick(MouseButton.RightButton).Dispatch();
            return this;
        }

        public MouseManager XButtonDown(int buttonId)
        {
            OutputManager.AddMouseXButtonDown(buttonId).Dispatch();
            return this;
        }

        public MouseManager XButtonUp(int buttonId)
        {
            OutputManager.AddMouseXButtonUp(buttonId).Dispatch();
            return this;
        }

        public MouseManager XButtonClick(int buttonId)
        {
            OutputManager.AddMouseXButtonClick(buttonId).Dispatch();
            return this;
        }

        public MouseManager XButtonDoubleClick(int buttonId)
        {
            OutputManager.AddMouseXButtonDoubleClick(buttonId).Dispatch();
            return this;
        }

        public MouseManager VerticalScroll(int scrollAmountInClicks)
        {
            OutputManager.AddMouseVerticalWheelScroll(scrollAmountInClicks * MouseWheelClickSize).Dispatch();
            return this;
        }

        public MouseManager HorizontalScroll(int scrollAmountInClicks)
        {
            OutputManager.AddMouseHorizontalWheelScroll(scrollAmountInClicks * MouseWheelClickSize).Dispatch();
            return this;
        }
    }
}
