using Godot;
using System;

public partial class TestScene : Node2D
{
	GlobalInput GI;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GI = GetTree().Root.GetNode<GlobalInput>("GlobalInput");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (GI.IsActionPressed("test_mouse")){
			GD.Print("Test");
		}
	}
}
