using Godot;
using System;

public partial class CharacterBody2D : Godot.CharacterBody2D
{
	public const float Speed = 300.0f;
	public GlobalInputCSharp GlobalInput { get; set; }

    public override void _Ready()
    {
    }

    public override void _PhysicsProcess(double delta)
	{

		if (Input.IsActionJustPressed("mouse_left")){
			GD.Print("CLICKED");
		}

		if (Input.IsActionJustReleased("mouse_left")){
			GD.Print("RELEASED");
		}
		Vector2 velocity = Velocity;
		
		velocity.X = (Input.IsActionPressed("move_right") ? 1 : 0) - (Input.IsActionPressed("move_left") ? 1 : 0);
		velocity.Y = (Input.IsActionPressed("move_down") ? 1 : 0) - (Input.IsActionPressed("move_up") ? 1 : 0);
		Velocity = velocity * Speed;

		MoveAndSlide();
	}
}
