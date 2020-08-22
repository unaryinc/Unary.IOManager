using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Unary.IOManager.Native;

namespace Unary.IOManager.Managers
{
    public class Keyboard
    {
        private App App;

        private readonly OutputFactory OutputManager;

        private Dictionary<VirtualKeyCode, bool> KeyboardState;

        private Dictionary<VirtualKeyCode, List<Action>> OnKeyPressSubscribers;
        private Dictionary<VirtualKeyCode, List<Action>> OnKeyReleaseSubscribers;

        public Keyboard(App NewApp, OutputFactory NewOutputManager)
        {
            App = NewApp;

            OutputManager = NewOutputManager;

            KeyboardState = new Dictionary<VirtualKeyCode, bool>();
            OnKeyPressSubscribers = new Dictionary<VirtualKeyCode, List<Action>>();
            OnKeyReleaseSubscribers = new Dictionary<VirtualKeyCode, List<Action>>();
        }

        private OutputFactory ModifiersDown(IEnumerable<VirtualKeyCode> modifierKeyCodes)
        {
            foreach (var key in modifierKeyCodes)
            {
                OutputManager.AddKeyDown(key);
            }
            return OutputManager;
        }

        private OutputFactory ModifiersUp(IEnumerable<VirtualKeyCode> modifierKeyCodes)
        {
            Stack<VirtualKeyCode> ReverseStack = new Stack<VirtualKeyCode>(modifierKeyCodes);
            while (ReverseStack.Count > 0)
            {
                OutputManager.AddKeyUp(ReverseStack.Pop());
            }
            return OutputManager;
        }

        private OutputFactory KeysPress(IEnumerable<VirtualKeyCode> keyCodes)
        {
            foreach (var key in keyCodes)
            {
                OutputManager.AddKeyPress(key);
            }
            return OutputManager;
        }

        public Keyboard KeyDown(VirtualKeyCode keyCode)
        {
            if(!App.Running) { return this; }
            OutputManager.AddKeyDown(keyCode).Dispatch();
            return this;
        }

        public Keyboard KeyUp(VirtualKeyCode keyCode)
        {
            if (!App.Running) { return this; }
            OutputManager.AddKeyUp(keyCode).Dispatch();
            return this;
        }

        public Keyboard KeyPress(VirtualKeyCode keyCode)
        {
            if (!App.Running) { return this; }
            OutputManager.AddKeyPress(keyCode).Dispatch();
            return this;
        }

        public Keyboard KeyPress(params VirtualKeyCode[] keyCodes)
        {
            if (!App.Running) { return this; }
            KeysPress(keyCodes).Dispatch();
            return this;
        }

        public Keyboard ModifiedKeyStroke(VirtualKeyCode modifierKeyCode, VirtualKeyCode keyCode)
        {
            if (!App.Running) { return this; }
            ModifiedKeyStroke(new[] { modifierKeyCode }, new[] { keyCode });
            return this;
        }

        public Keyboard ModifiedKeyStroke(IEnumerable<VirtualKeyCode> modifierKeyCodes, VirtualKeyCode keyCode)
        {
            if (!App.Running) { return this; }
            ModifiedKeyStroke(modifierKeyCodes, new[] { keyCode });
            return this;
        }

        public Keyboard ModifiedKeyStroke(VirtualKeyCode modifierKey, IEnumerable<VirtualKeyCode> keyCodes)
        {
            if (!App.Running) { return this; }
            ModifiedKeyStroke(new[] { modifierKey }, keyCodes);
            return this;
        }

        public Keyboard ModifiedKeyStroke(IEnumerable<VirtualKeyCode> modifierKeyCodes, IEnumerable<VirtualKeyCode> keyCodes)
        {
            if (!App.Running) { return this; }
            ModifiersDown(modifierKeyCodes);
            KeysPress(keyCodes);
            ModifiersUp(modifierKeyCodes);
            OutputManager.Dispatch();
            return this;
        }

        public Keyboard TextEntry(string text)
        {
            if (!App.Running) { return this; }
            if (text.Length > 8192) return this;
            OutputManager.AddCharacters(text).Dispatch();
            return this;
        }

        public Keyboard TextEntry(char character)
        {
            if (!App.Running) { return this; }
            OutputManager.AddCharacter(character).Dispatch();
            return this;
        }

        public bool IsKeyDown(VirtualKeyCode keyCode)
        {
            if (!App.Running) { return false; }
            short result = Methods.GetKeyState((ushort)keyCode);
            return (result < 0);
        }

        public bool IsKeyUp(VirtualKeyCode keyCode)
        {
            if (!App.Running) { return true; }
            return !IsKeyDown(keyCode);
        }

        public bool IsTogglingKeyInEffect(VirtualKeyCode keyCode)
        {
            short result = Methods.GetKeyState((ushort)keyCode);
            return (result & 0x01) == 0x01;
        }

        public void OnKeyPress(VirtualKeyCode Code, Action NewAction)
        {
            if(!KeyboardState.ContainsKey(Code))
            {
                KeyboardState[Code] = IsKeyDown(Code);
            }

            if(!OnKeyPressSubscribers.ContainsKey(Code))
            {
                OnKeyPressSubscribers[Code] = new List<Action>();
            }

            OnKeyPressSubscribers[Code].Add(NewAction);
        }

        public void OnKeyRelease(VirtualKeyCode Code, Action NewAction)
        {
            if (!KeyboardState.ContainsKey(Code))
            {
                KeyboardState[Code] = IsKeyDown(Code);
            }

            if (!OnKeyReleaseSubscribers.ContainsKey(Code))
            {
                OnKeyReleaseSubscribers[Code] = new List<Action>();
            }

            OnKeyReleaseSubscribers[Code].Add(NewAction);
        }

        private void OnKeyPressed(VirtualKeyCode Code)
        {
            foreach(var Subscriber in OnKeyPressSubscribers[Code])
            {
                Subscriber.Invoke();
            }
        }

        private void OnKeyReleased(VirtualKeyCode Code)
        {
            foreach (var Subscriber in OnKeyReleaseSubscribers[Code])
            {
                Subscriber.Invoke();
            }
        }

        public void Poll()
        {
            foreach(var Key in KeyboardState.ToArray())
            {
                if(Key.Value)
                {
                    if(IsKeyUp(Key.Key))
                    {
                        KeyboardState[Key.Key] = false;
                        OnKeyReleased(Key.Key);
                    }
                }
                else
                {
                    if (IsKeyDown(Key.Key))
                    {
                        KeyboardState[Key.Key] = true;
                        OnKeyPressed(Key.Key);
                    }
                }
            }
        }
    }
}
