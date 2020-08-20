using Unary.IOManager.Native;

namespace Unary.IOManager.Managers
{
    public class InputManager
    {
        private WindowManager WindowManager;

        public InputManager(WindowManager Manager)
        {
            WindowManager = Manager;
        }

        public bool IsKeyDown(VirtualKeyCode keyCode)
        {
            short result = Methods.GetKeyState((ushort)keyCode);
            return (result < 0);
        }

        public bool IsKeyUp(VirtualKeyCode keyCode)
        {
            return !IsKeyDown(keyCode);
        }
    }
}
