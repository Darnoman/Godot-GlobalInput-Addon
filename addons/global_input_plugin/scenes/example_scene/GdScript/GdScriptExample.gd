extends Node

var controls = {"test_key": [KEY_W, KEY_A],
				"test_mouse": [MOUSE_BUTTON_LEFT, MOUSE_BUTTON_RIGHT]}

func _enter_tree() -> void:
	#you can do this via ProjectSettings -> InputMap
	var ev
	for action in controls:
		if not InputMap.has_action(action):
			InputMap.add_action(action)
		for key in controls[action]:
			if action == "test_key":
				ev = InputEventKey.new()
				ev.physical_keycode = key
				InputMap.action_add_event(action, ev)
			if action == "test_mouse":
				ev = InputEventMouseButton.new()
				ev.button_index = key
				InputMap.action_add_event(action,ev)
	pass

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	if GlobalInput.IsActionJustPressed("test_key"):
		print("key just pressed")
	if GlobalInput.IsActionPressed("test_key"):
		print("key is pressed")
	if GlobalInput.IsActionJustReleased("test_key"):
		print("key just released")
	if GlobalInput.IsActionJustPressed("test_mouse"):
		print("mouse just pressed")
	if GlobalInput.IsActionPressed("test_mouse"):
		print("mouse is pressed")
	if GlobalInput.IsActionJustReleased("test_mouse"):
		print("mouse just released")
	pass


func _exit_tree() -> void:
	var ev
	for action in controls:
		for key in controls[action]:
			if action == "test_key":
				ev = InputEventKey.new()
				ev.physical_keycode = key
				InputMap.action_erase_event(action, ev)
			if action == "test_mouse":
				ev = InputEventMouseButton.new()
				ev.button_index = key
				InputMap.action_erase_event(action,ev)
		InputMap.erase_action(action)
	pass
