# Godot-GlobalInput-Addon

**Note: This was written in C# and uses System User32.dll, (so I think it'll only work on windows)**

**How to use:**
1. Download and drag GlobalInput's addon folder in your project's addon folder.
2. Create a temp node scene with a temp .cs script to be able to build the addon
3. Activate the addon.

**Setup Video**
https://youtu.be/oJLsgq3i-yw

**Methods must be used in process or physics process**

**Both Language Functions:**
1. IsActionJustPressed(string action)
2. IsActionPressed(string action)
3. IsActionJustRelease(string action)
4. GetMousePosition() // returns it's position relative to top left of primary screen

**C# Only Functions**
1. SetMousePosition(int x, int y) // used to set mouse position without imitating mouse movement (teleports cursor)
2. SetMouseEvent(MouseEventFlags value, MousePoint? point) // used to move mouse to position - imitates mouse movement


**Things That is Missing**
1. Mouse wheel scroll input is not being captured
2. Find a neater way to convert Godot keycodes to Window VirtualKeycodes
3. Good documentation
