using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Unary.IOManager.Native;

namespace Unary.IOManager.Managers
{
    public class KeyboardManager
    {
        private readonly OutputManager OutputManager;

        public KeyboardManager(OutputManager Manager)
        {
            OutputManager = Manager;
        }

        private OutputManager ModifiersDown(IEnumerable<VirtualKeyCode> modifierKeyCodes)
        {
            foreach (var key in modifierKeyCodes)
            {
                OutputManager.AddKeyDown(key);
            }
            return OutputManager;
        }

        private OutputManager ModifiersUp(IEnumerable<VirtualKeyCode> modifierKeyCodes)
        {
            Stack<VirtualKeyCode> ReverseStack = new Stack<VirtualKeyCode>(modifierKeyCodes);
            while (ReverseStack.Count > 0)
            {
                OutputManager.AddKeyUp(ReverseStack.Pop());
            }
            return OutputManager;
        }

        private OutputManager KeysPress(IEnumerable<VirtualKeyCode> keyCodes)
        {
            foreach (var key in keyCodes)
            {
                OutputManager.AddKeyPress(key);
            }
            return OutputManager;
        }

        public KeyboardManager KeyDown(VirtualKeyCode keyCode)
        {
            OutputManager.AddKeyDown(keyCode).Dispatch();
            return this;
        }

        public KeyboardManager KeyUp(VirtualKeyCode keyCode)
        {
            OutputManager.AddKeyUp(keyCode).Dispatch();
            return this;
        }

        public KeyboardManager KeyPress(VirtualKeyCode keyCode)
        {
            OutputManager.AddKeyPress(keyCode).Dispatch();
            return this;
        }

        public KeyboardManager KeyPress(params VirtualKeyCode[] keyCodes)
        {
            KeysPress(keyCodes).Dispatch();
            return this;
        }

        public KeyboardManager ModifiedKeyStroke(VirtualKeyCode modifierKeyCode, VirtualKeyCode keyCode)
        {
            ModifiedKeyStroke(new[] { modifierKeyCode }, new[] { keyCode });
            return this;
        }

        public KeyboardManager ModifiedKeyStroke(IEnumerable<VirtualKeyCode> modifierKeyCodes, VirtualKeyCode keyCode)
        {
            ModifiedKeyStroke(modifierKeyCodes, new[] { keyCode });
            return this;
        }

        public KeyboardManager ModifiedKeyStroke(VirtualKeyCode modifierKey, IEnumerable<VirtualKeyCode> keyCodes)
        {
            ModifiedKeyStroke(new[] { modifierKey }, keyCodes);
            return this;
        }

        public KeyboardManager ModifiedKeyStroke(IEnumerable<VirtualKeyCode> modifierKeyCodes, IEnumerable<VirtualKeyCode> keyCodes)
        {
            ModifiersDown(modifierKeyCodes);
            KeysPress(keyCodes);
            ModifiersUp(modifierKeyCodes);
            OutputManager.Dispatch();
            return this;
        }

        public KeyboardManager TextEntry(string text)
        {
            if (text.Length > 8192) return this;
            OutputManager.AddCharacters(text).Dispatch();
            return this;
        }

        public KeyboardManager TextEntry(char character)
        {
            OutputManager.AddCharacter(character).Dispatch();
            return this;
        }
    }
}
