# Unary.IOManager
C# library for IO management of external windows

Full rewrite of [this library](https://github.com/michaelnoonan/inputsimulator) (I did not like this library at all, so I decided to rewrite it completely.)

Examples
==========

Example: Standart usage of a library
-------------
```csharp
IOManager.IOManager IO = new IOManager.IOManager("notepad++");

IO.App.OnAppEnable((Enabled) => { Console.WriteLine("Is app enabled: " + Enabled); });

IO.Window.OnWindowFocus((Focus) => { Console.WriteLine("Focus: " + Focus); });
IO.Window.OnWindowMove((Rect) => { Console.WriteLine("Window moved to:" + Rect); });

IO.Keyboard.OnKeyPress(VirtualKeyCode.F2, () => { Console.WriteLine("F2 got pressed"); });
IO.Keyboard.OnKeyRelease(VirtualKeyCode.F2, () => { Console.WriteLine("F2 got released"); });

IO.Mouse.OnMouseLeftPressed(() => { Console.WriteLine("LMB got pressed"); });
IO.Mouse.OnMouseLeftReleased(() => { Console.WriteLine("LMB got released"); });
IO.Mouse.OnMouseRightPressed(() => { Console.WriteLine("RMB got pressed"); });
IO.Mouse.OnMouseRightReleased(() => { Console.WriteLine("RMB got released"); });

IO.Mouse.OnMouseMove((NewPos) => { Console.WriteLine("Mouse coords: " + NewPos); });

IO.Init();

Console.WriteLine("App process name: " + IO.App.ProcessName);
Console.WriteLine("App process window handle: " + IO.App.WindowHandle);

IO.Window.SetActive();
IO.Window.SetRect(new Methods.Rect() { X = 0, Y = 0, W = 640, Z = 480 });

IO.Keyboard.KeyPress(VirtualKeyCode.F1);

IO.Mouse.MoveMouseBy(100, 100);
IO.Mouse.LeftButtonClick();

while (true)
{
  IO.Poll();
  Thread.Sleep(30);
}
```

Example: Single key press
-------------
```csharp
public void PressTheSpacebar()
{
  IO.Keyboard.SimulateKeyPress(VirtualKeyCode.SPACE);
}
```

Example: Key-down and Key-up
------------
```csharp
public void ShoutHello()
{
  // Simulate each key stroke
  IO.Keyboard.KeyDown(VirtualKeyCode.SHIFT);
  IO.Keyboard.KeyPress(VirtualKeyCode.VK_H);
  IO.Keyboard.KeyPress(VirtualKeyCode.VK_E);
  IO.Keyboard.KeyPress(VirtualKeyCode.VK_L);
  IO.Keyboard.KeyPress(VirtualKeyCode.VK_L);
  IO.Keyboard.KeyPress(VirtualKeyCode.VK_O);
  IO.Keyboard.KeyPress(VirtualKeyCode.VK_1);
  IO.Keyboard.KeyUp(VirtualKeyCode.SHIFT);

  // Alternatively you can simulate text entry to acheive the same end result
  IO.Keyboard.SimulateTextEntry("HELLO!");
}
```

Example: Modified keystrokes such as CTRL-C
--------------
```csharp
public void SimulateSomeModifiedKeystrokes()
{
  // CTRL-C (effectively a copy command in many situations)
  IO.Keyboard.ModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_C);

  // You can simulate chords with multiple modifiers
  // For example CTRL-K-C whic is simulated as
  // CTRL-down, K, C, CTRL-up
  IO.Keyboard.ModifiedKeyStroke(VirtualKeyCode.CONTROL, new [] {VirtualKeyCode.VK_K, VirtualKeyCode.VK_C});

  // You can simulate complex chords with multiple modifiers and key presses
  // For example CTRL-ALT-SHIFT-ESC-K which is simulated as
  // CTRL-down, ALT-down, SHIFT-down, press ESC, press K, SHIFT-up, ALT-up, CTRL-up
  IO.Keyboard.ModifiedKeyStroke(
    new[] { VirtualKeyCode.CONTROL, VirtualKeyCode.MENU, VirtualKeyCode.SHIFT },
    new[] { VirtualKeyCode.ESCAPE, VirtualKeyCode.VK_K });
}
```

Example: Simulate text entry
--------
```csharp
public void SayHello()
{
  IO.Keyboard.TextEntry("Say hello!");
}
```

Example: Determine the state of different types of keys
------------
```csharp
public void GetKeyStatus()
{
  // Determines if the shift key is currently down
  var isShiftKeyDown = IO.Keyboard.IsKeyDown(VirtualKeyCode.SHIFT);

  // Determines if the caps lock key is currently in effect (toggled on)
  var isCapsLockOn = IO.Keyboard.IsTogglingKeyInEffect(VirtualKeyCode.CAPITAL);
}
```
