using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Unary.IOManager.Managers;

namespace Unary.IOManager
{
    public class IOManager
    {
        private readonly OutputFactory OutputFactory;

        public App App { get; private set; }
        public Window Window { get; private set; }
        public Keyboard Keyboard { get; private set; }
        public Mouse Mouse { get; private set; }

        public IOManager(string TargetProcess)
        {
            OutputFactory = new OutputFactory();
            App = new App(TargetProcess);
            Window = new Window(App);
            Keyboard = new Keyboard(App, OutputFactory);
            Mouse = new Mouse(App, OutputFactory);
        }

        public void Init()
        {
            App.Init();
        }

        public void Poll()
        {
            App.Poll();

            if(App.Running)
            {
                Window.Poll();
                Keyboard.Poll();
                Mouse.Poll();
            }
        }
    }
}
