using Godot;
using System;

public partial class CharacterBody2D : Godot.CharacterBody2D
{
	public const float Speed = 300.0f;
	public GlobalInputCSharp GlobalInput;

    public override void _Ready()
    {
        GlobalInput = GetNode<GlobalInputCSharp>("/root/GlobalInput/GlobalInputCSharp");
    }

    public override void _PhysicsProcess(double delta)
	{

		if (GlobalInput.IsActionPressed("mouse_left")){
			GD.Print(GlobalInput.GetMousePosition());
		}
		Vector2 velocity = Velocity;
		
		velocity.X = (GlobalInput.IsActionPressed("move_right") ? 1 : 0) - (GlobalInput.IsActionPressed("move_left") ? 1 : 0);
		velocity.Y = (GlobalInput.IsActionPressed("move_down") ? 1 : 0) - (GlobalInput.IsActionPressed("move_up") ? 1 : 0);
		Velocity = velocity * Speed;

		MoveAndSlide();
	}
}
