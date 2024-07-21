#region Header
#01. tool

#02. class_name

#03. extends
extends Node
#endregion

#region Documentation
#-----------------------------------------------------------
#04. # docstring
## hoge
#-----------------------------------------------------------
#endregion

#region Body
#05. signals
#-----------------------------------------------------------

#-----------------------------------------------------------
#06. enums
#-----------------------------------------------------------
enum InputType {
	Mouse = 0,
	Keyboard = 1
}

enum MouseEventFlags {
	Absolute = 0x8000,
	HWheel = 0x01000,
	Move = 0x0001,
	MoveNoCoalesce = 0x2000,
	LeftDown = 0x0002,
	LeftUp = 0x0004,
	RightDown = 0x0008,
	RightUp = 0x0010,
	MiddleDown = 0x0020,
	MiddleUp = 0x0040,
	VirtualDesk = 0x4000,
	Wheel = 0x0800,
	XDown = 0x0080,
	XUp = 0x0100
}

enum KeyEventFlags {
	KeyDown = 0x0000,
	ExtendedKey = 0x0001,
	KeyUp = 0x0002,
	Unicode = 0x0004,
	Scancode = 0x0008
	}

enum MapVirtualKeyTypes {
	VkToScn = 0x00,
	ScnToVk = 0x01,
	VkToChar = 0x02,
	ScnToVkEx = 0x03,
	VkToScnEx = 0x04,
}
#-----------------------------------------------------------
#08. variables
#-----------------------------------------------------------
@onready var global_input_csharp_packed_scene = preload("res://addons/global_input/autoloads/global_input_csharp/GlobalInputCSharp.tscn")
var global_input_csharp; # to get this in C# use GetNode("/root/GlobalInput") 
						 # or if you want the C# version of this node GetNode("/root/GlobalInput/GlobalInputCSharp")

# godot keycode to window keycodes
var godot_key_to_window_key: Dictionary
var godot_mouse_to_window_mouse: Dictionary
#-----------------------------------------------------------
#09. methods
#-----------------------------------------------------------
func _ready() -> void:
	_initialize_global_input_c_sharp()
	pass

#region General Mouse and Keyboard Functions

## Returns the mouse position relative to the top left corner of the primary screen.
func get_mouse_position() -> Vector2:
	return global_input_csharp.GetMousePosition()

## Sets the mouse position to the given vector x and y.
## This teleports the mouse and does not simulate moving the mouse to the point.
func set_mouse_position(position:Vector2) -> void:
	global_input_csharp.SetMousePosition(position)

## Imitates keyboard inputs.
## Keyboard parameters: (KeyEventFlags flag, int keycode, int scancode = 0)
## Mouse parameters: (MouseEventFlags flags, Vector2? mouseOffset = null)
func send_input(type: InputType, parameters: Array) -> int:
	return global_input_csharp.SendInput(type, parameters)
#endregion

#region Godot Section

func map_virtual_key(code: int, map_type:MapVirtualKeyTypes = MapVirtualKeyTypes.VkToScn) -> int:
	return global_input_csharp.MapVirtualKey(code, map_type)

## Is similar to Input.is_action_just_pressed().
func is_action_just_pressed(action:StringName) -> bool:
	return global_input_csharp.IsActionJustPressed(action)

## Is similar to Input.is_action_pressed().
func is_action_pressed(action:StringName) -> bool:
	return global_input_csharp.IsActionPressed(action)

## Is similar to Input.is_action_just_released().
func is_action_just_released(action:StringName) -> bool:
	return global_input_csharp.IsActionJustReleased(action)

## Is similar to Input.get_vector().
func get_vector(negative_x:StringName, positive_x:StringName, negative_y:StringName, positive_y:StringName) -> Vector2:
	return global_input_csharp.GetVector(negative_x, positive_x, negative_y, positive_y)

## Is similar to Input.is_anything_pressed().
func is_anything_pressed():
	return global_input_csharp.IsAnythingPressed()

## Is similar to Input.is_key_pressed().
func is_key_pressed(keycode: Key):
	return global_input_csharp.IsKeyPressed(keycode)
#endregion

func _initialize_global_input_c_sharp():
	global_input_csharp = global_input_csharp_packed_scene.instantiate()
	add_child(global_input_csharp)
	
	# initialize the constant dictionaries
	godot_key_to_window_key = global_input_csharp.GodotKeyToWindowKey
	godot_mouse_to_window_mouse = global_input_csharp.GodotMouseToWindowMouse
	pass
#-----------------------------------------------------------
#10. signal methods
#-----------------------------------------------------------
