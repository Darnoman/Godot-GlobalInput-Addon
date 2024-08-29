# Godot-GlobalInput-Addon

**How to use:**
1. Create a project using the mono version of Godot
2. Download and drag GlobalInput's addon folder in your project's addon folder.
3. Create a temp node scene with a temp .cs script to be able to build the addon
4. Activate the addon.

**--------------------------------------------------**

**Dependencies**
SharpHook - enter "dotnet add package SharpHook" into the console/cli within the project's directory

**--------------------------------------------------**

**Setup Video**
https://youtu.be/oJLsgq3i-yw - for v 0.1 (but should still work)

**--------------------------------------------------**
Issue: When a button is held down and the window becomes out of focus, it fires a "just released" event.

Use Input as normal, just now it captures inputs when window is out of focus.
