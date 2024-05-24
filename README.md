# Godot-GlobalInput-Addon

**How to use:**
1. Create a project using the mono version of Godot
2. Download and drag GlobalInput's addon folder in your project's addon folder.
3. Create a temp node scene with a temp .cs script to be able to build the addon
4. Activate the addon.

**Dependencies**
SharpHook - enter "dotnet add package SharpHook" into the console/cli within the project directory

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

'GlobalInput = GetNode<GlobalInputCSharp>("/root/GlobalInput/GlobalInputCSharp") // initialize the variable'

Functions within GlobalInputCSharp:
1. GlobalInput.IsActionJustPressed(string action)
2. GlobalInput.IsActionPressed(string action)
3. GlobalInput.IsActionJustReleased(string action)
4. GlobalInput.IsKeyPressed(Key key)
5. GlobalInput.GetVector(string negativeX, string positiveX, string negativeY, string positiveY)