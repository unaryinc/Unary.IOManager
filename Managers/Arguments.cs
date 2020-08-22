using System.Drawing;
using Unary.IOManager.Native;

namespace Unary.IOManager.Managers
{
    public enum KeyboardType
    {
        OnKeyPress,
        OnKeyRelease
    }

    public enum MouseType
    {
        OnMouseMove,
        OnLeftReleased,
        OnRightReleased
    }
}