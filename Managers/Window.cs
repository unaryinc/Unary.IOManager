using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using Unary.IOManager.Native;

namespace Unary.IOManager.Managers
{
    public class Window
    {
        private App App;

        private bool ActiveState = false;
        private Methods.Rect RectangleState;
        private Methods.Rect TempRectangle;

        private List<Action<bool>> OnWindowFocusSubscribers;
        private List<Action<Methods.Rect>> OnWindowMoveSubscribers;

        public Window(App NewApp)
        {
            App = NewApp;

            RectangleState = new Methods.Rect();

            OnWindowFocusSubscribers = new List<Action<bool>>();
            OnWindowMoveSubscribers = new List<Action<Methods.Rect>>();
        }

        public void SetActive()
        {
            if(!App.Running) { return; }
            Methods.SetForegroundWindow(App.WindowHandle);
        }

        public bool IsActive()
        {
            if (!App.Running) { return false; }
            return ActiveState;
        }

        public Methods.Rect GetRect()
        {
            if (!App.Running) { return TempRectangle; }
            Methods.GetWindowRect(App.WindowHandle, ref TempRectangle);
            return TempRectangle;
        }

        public void SetRect(Methods.Rect NewRect)
        {
            if (!App.Running) { return; }
            Methods.SetWindowPos(App.WindowHandle, 0, NewRect.X, NewRect.Y, NewRect.Z, NewRect.W,
            (uint)(WindowFlag.SWP_NOZORDER | WindowFlag.SWP_NOSIZE | WindowFlag.SWP_SHOWWINDOW));
        }

        private void SendNewActive()
        {
            foreach(var ActiveSub in OnWindowFocusSubscribers)
            {
                ActiveSub.Invoke(ActiveState);
            }
        }

        private void SendNewMove()
        {
            foreach (var ActiveSub in OnWindowMoveSubscribers)
            {
                ActiveSub.Invoke(RectangleState);
            }
        }

        private void CheckActive()
        {
            if (Methods.GetForegroundWindow() == App.WindowHandle)
            {
                if (!ActiveState)
                {
                    ActiveState = true;
                    SendNewActive();
                }
            }
            else
            {
                if (ActiveState)
                {
                    ActiveState = false;
                    SendNewActive();
                }
            }
        }

        private void CheckMove()
        {
            if (GetRect() != RectangleState)
            {
                RectangleState = TempRectangle;
                SendNewMove();
            }
        }
        
        public void Poll()
        {
            CheckActive();
            CheckMove();
        }

        public void OnWindowFocus(Action<bool> NewAction)
        {
            OnWindowFocusSubscribers.Add(NewAction);
        }

        public void OnWindowMove(Action<Methods.Rect> NewAction)
        {
            OnWindowMoveSubscribers.Add(NewAction);
        }
    }
}
