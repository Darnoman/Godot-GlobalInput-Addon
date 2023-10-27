# Godot-GlobalInput-Addon
Download and drag GlobalInput's addon folder in your project's addon folder.

Activate the addon.

It adds a GlobalInput singleton/addon.

Note: This was written in C# and uses System User32.dll, (so I think it'll only work on windows)

ALSO NOTE: Some of Godot's Keycode don't match up with Window's Virtual Keycode. The only things that worked is the letters A-Z, space, and the punctuation stuff ([ ] ; ' , . / ..etc).

It has is_action_pressed, is_action_just_pressed, and is_action_just_released
