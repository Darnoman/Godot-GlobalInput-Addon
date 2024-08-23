extends CharacterBody2D


const SPEED = 300.0

func _physics_process(delta: float) -> void:
	# Get the input direction and handle the movement/deceleration.
	# As good practice, you should replace UI actions with custom gameplay actions.
	if (Input.is_action_just_pressed("mouse_left")):
		print("CLICKED")
	
	if (Input.is_action_just_released("mouse_left")):
		print("RELEASED")
	
	velocity.x = int(Input.is_action_pressed("move_right")) - int(Input.is_action_pressed("move_left"))
	velocity.y = int(Input.is_action_pressed("move_down")) - int(Input.is_action_pressed("move_up"))
	velocity *= SPEED
	move_and_slide()
