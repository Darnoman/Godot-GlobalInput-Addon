# Godot-GlobalInput-Addon

**Note: This was written in C# and uses System User32.dll, (so I think it'll only work on windows)**

**How to use:**
1. Download and drag GlobalInput's addon folder in your project's addon folder.
2. Create a temp node scene with a temp .cs script to be able to build the addon
3. Activate the addon.

**Setup Video**
https://youtu.be/oJLsgq3i-yw

**Both Language Functions:**
1. IsActionJustPressed(string action)
2. IsActionPressed(string action)
3. IsActionJustRelease(string action)

**C# Only Functions**
1. SetMousePosition(int x, int y)
2. SetMouseEvent(MouseEventFlags value, MousePoint? point)
