using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unary.IOManager.Managers;

namespace Unary.IOManager
{
    public class IOManager
    {
        private readonly OutputManager OutputManager;

        public KeyboardManager Keyboard { get; private set; }
        public MouseManager Mouse { get; private set; }

        public WindowManager Window { get; private set; }

        private readonly InputManager InputManager;

        public IOManager(Process TargetProcess)
        {
            OutputManager = new OutputManager();

            Keyboard = new KeyboardManager(OutputManager);
            Mouse = new MouseManager(OutputManager);

            Window = new WindowManager(TargetProcess);

            InputManager = new InputManager(Window);
        }
    }
}
