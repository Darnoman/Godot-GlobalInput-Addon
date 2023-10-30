extends Node2D


var controls = {"move_left": [KEY_A],
				"move_right": [KEY_D],
				"move_up": [KEY_W],
				"move_down": [KEY_S],
				"mouse_left": [MOUSE_BUTTON_LEFT]
				}

func _enter_tree() -> void:
	#you can do this via ProjectSettings -> InputMap
	var ev
	for action in controls:
		if not InputMap.has_action(action):
			InputMap.add_action(action)
		for key in controls[action]:
			if action == "move_left" or action == "move_right" or action == "move_up" or action == "move_down":
				ev = InputEventKey.new()
				ev.physical_keycode = key
				InputMap.action_add_event(action, ev)
			if action == "mouse_left":
				ev = InputEventMouseButton.new()
				ev.button_index = key
				InputMap.action_add_event(action, ev)
	pass

func _exit_tree() -> void:
	var ev
	for action in controls:
		InputMap.erase_action(action)
	pass
