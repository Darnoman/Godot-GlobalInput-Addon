#region Header
#01. tool

#02. class_name
class_name GlobalInputGd
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

#-----------------------------------------------------------
#07. exports
#-----------------------------------------------------------

#-----------------------------------------------------------
#08. variables
#-----------------------------------------------------------
@onready var GlobalInputCSharpPackedScene = preload("res://addons/global_input/autoload/global_input_csharp/GlobalInputCSharp.tscn")
var GlobalInputCSharp;

#-----------------------------------------------------------
#09. methods
#-----------------------------------------------------------
func _ready() -> void:
	_initialize_global_input_c_sharp()
	pass

func get_mouse_position() -> Vector2:
	return GlobalInputCSharp.GetMousePosition()

func is_action_just_pressed(action:StringName) -> bool:
	return GlobalInputCSharp.IsActionJustPressed(action)

func is_action_pressed(action:StringName) -> bool:
	return GlobalInputCSharp.IsActionPressed(action)

func is_action_just_release(action:StringName) -> bool:
	return GlobalInputCSharp.IsActionJustRelease(action)

func get_vector(negative_x:StringName, positive_x:StringName, negative_y:StringName, positive_y:StringName) -> Vector2:
	return GlobalInputCSharp.GetVector(negative_x, positive_x, negative_y, positive_y)

func is_anything_pressed():
	return GlobalInputCSharp.IsAnythingPressed()

func is_key_pressed(keycode: Key):
	return GlobalInputCSharp.IsKeyPressed(keycode)

func _initialize_global_input_c_sharp():
	GlobalInputCSharp = GlobalInputCSharpPackedScene.instantiate()
	add_child(GlobalInputCSharp)
	pass
#-----------------------------------------------------------
#10. signal methods
#-----------------------------------------------------------
#endregion
