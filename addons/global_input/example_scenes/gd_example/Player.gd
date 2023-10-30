extends CharacterBody2D


const SPEED = 300.0

func _physics_process(delta: float) -> void:
	# Get the input direction and handle the movement/deceleration.
	# As good practice, you should replace UI actions with custom gameplay actions.
	if (GlobalInput.is_action_pressed("mouse_left")):
		print(GlobalInput.get_mouse_position())
		
	velocity.x = int(GlobalInput.is_action_pressed("move_right")) - int(GlobalInput.is_action_pressed("move_left"))
	velocity.y = int(GlobalInput.is_action_pressed("move_down")) - int(GlobalInput.is_action_pressed("move_up"))
	velocity *= SPEED
	move_and_slide()
