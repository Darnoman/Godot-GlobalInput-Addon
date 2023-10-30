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

func get_mouse_position():
	return GlobalInputCSharp.GetMousePosition()

func is_action_just_pressed(action:StringName):
	return GlobalInputCSharp.IsActionJustPressed(action)

func is_action_pressed(action:StringName):
	return GlobalInputCSharp.IsActionPressed(action)

func is_action_just_release(action:StringName):
	return GlobalInputCSharp.IsActionJustRelease(action)

func _initialize_global_input_c_sharp():
	GlobalInputCSharp = GlobalInputCSharpPackedScene.instantiate()
	add_child(GlobalInputCSharp)
	pass
#-----------------------------------------------------------
#10. signal methods
#-----------------------------------------------------------
#endregion
