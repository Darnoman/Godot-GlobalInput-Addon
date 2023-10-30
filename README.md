# Godot-GlobalInput-Addon

**Note: This was written in C# and uses System User32.dll, (so I think it'll only work on windows)**

**How to use:**
1. Download and drag GlobalInput's addon folder in your project's addon folder.
2. Create a temp node scene with a temp .cs script to be able to build the addon
3. Activate the addon.

**Setup Video**
https://youtu.be/oJLsgq3i-yw - for v 0.1 (but should still work)

**How To Use**

**GdScript:**
After activiating the pluggin, access the GlobalInput singleton/autoload with 'GlobalInput'. 
Functions within GlobalInput autoload:
1. GlobalInput.is_action_just_pressed(action: String)
2. GlobalInput.is_action_pressed(action: String)
3. GlobalInput.is_action_just_released(action: String)
4. GlobalInput.get_mouse_position()

**C#:**
After activating the pluggin, access the GlobalInputCSharp singleton/autoload with:
'GlobalInputCSharp GlobalInput; // declare the variable'
(within the _Ready function)
'GlobalInput = GetNode<GlobalInputCSharp>("/root/GlobalInput/GlobalInputCSharp") // declare the variable'
Functions within GlobalInputCSharp:
1. GlobalInput.IsActionJustPressed(string action)
2. GlobalInput.IsActionPressed(string action)
3. GlobalInput.IsActionJustReleased(string action)
4. GlobalInput.GetMousePosition()

**Things That is Missing**
1. Mouse wheel scroll input is not being captured
2. Find a neater way to convert Godot keycodes to Window VirtualKeycodes
