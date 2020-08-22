using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Unary.IOManager.Native;

namespace Unary.IOManager.Managers
{
    public class App
    {
        public Process Process { get; private set; }
        public string ProcessName { get; private set; }
        public IntPtr WindowHandle { get; private set; }

        public bool Running = false;

        private List<Action<bool>> OnAppEnableSubscribers;

        public App(string NewProcessName)
        {
            Process = null;
            ProcessName = NewProcessName;
            WindowHandle = IntPtr.Zero;
            OnAppEnableSubscribers = new List<Action<bool>>();
        }

        public void Init()
        {
            while(!Running)
            {
                Process = Process.GetProcessesByName(ProcessName).FirstOrDefault();

                if (Process != null && Process.MainWindowHandle != null && Process.MainWindowHandle != IntPtr.Zero)
                {
                    Running = true;
                    WindowHandle = Process.MainWindowHandle;
                    SendOnEnable();
                }

                Thread.Sleep(100);
            }
        }

        private void SendOnEnable()
        {
            foreach(var Subscriber in OnAppEnableSubscribers)
            {
                Subscriber.Invoke(Running);
            }
        }

        public void Poll()
        {
            if (Running)
            {
                if (Process.HasExited)
                {
                    Process.WaitForExit();
                    Running = false;
                    Process = null;
                    WindowHandle = IntPtr.Zero;
                    SendOnEnable();
                }
            }
            else
            {
                Process = Process.GetProcessesByName(ProcessName).FirstOrDefault();

                if (Process != null && Process.MainWindowHandle != null && Process.MainWindowHandle != IntPtr.Zero)
                {
                    Running = true;
                    WindowHandle = Process.MainWindowHandle;
                    SendOnEnable();
                }
            }
        }

        public void OnAppEnable(Action<bool> NewAction)
        {
            OnAppEnableSubscribers.Add(NewAction);
        }
    }
}
