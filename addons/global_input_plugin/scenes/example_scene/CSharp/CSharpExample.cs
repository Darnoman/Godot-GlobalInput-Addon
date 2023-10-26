using Godot;
using System;
using System.Collections.Generic;

public partial class CSharpExample : Node
{
	private Dictionary<string, long[]> Controls = new Dictionary<string, long[]>();
	private GlobalInput GlobalInput;
    // Called when the node enters the scene tree for the first time.
    public override void _EnterTree()
    {
		// You can do this via the Project Settings -> InputMap
         Controls.Add("test_keys", new long[]{(long)Key.W, (long)Key.A}); // adds key input
		 Controls.Add("test_mouse", new long[]{(long)MouseButton.Left, (long)MouseButton.Right}); // add mouse inputs
		 List<string> ControlKeyList = new List<string>(Controls.Keys);
		 foreach (string action in ControlKeyList){
			if (!InputMap.HasAction(action)){
				InputMap.AddAction(action);
			}
			foreach (long key in Controls[action]){
				if (action == "test_keys"){
					InputEventKey ev = new InputEventKey();
					ev.PhysicalKeycode = (Key)key;
					InputMap.ActionAddEvent(action, ev);
				}
				else if (action == "test_mouse"){
					InputEventMouseButton ev = new InputEventMouseButton();
					ev.ButtonIndex = (MouseButton)key;
					InputMap.ActionAddEvent(action, ev);
				}
			}
		 }
    }

    public override void _Ready()
	{
		GlobalInput = (GlobalInput) GetTree().Root.GetNode("GlobalInput"); // get the autload GlobalInput
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

		// Use it just like usual
		if (GlobalInput.IsActionJustPressed("test_keys")){
			GD.Print("Key Just Pressed");
		}

		if (GlobalInput.IsActionPressed("test_keys")){
			GD.Print("Key Is Pressed");
		}

		if (GlobalInput.IsActionJustReleased("test_keys")){
			GD.Print("Key Just Released");
		}

		if (GlobalInput.IsActionJustPressed("test_mouse")){
			GD.Print("Mouse Just Pressed");
		}

		if (GlobalInput.IsActionPressed("test_mouse")){
			GD.Print("Mouse Is Pressed");
		}

		if (GlobalInput.IsActionJustReleased("test_mouse")){
			GD.Print("Mouse Just Released");
		}
	}

	
    public override void _ExitTree()
    {
		List<string> ControlKeyList = new List<string>(Controls.Keys);
        foreach (string action in ControlKeyList){
			foreach (long key in Controls[action]){
				if (action == "test_keys"){
					InputEventKey ev = new InputEventKey();
					ev.PhysicalKeycode = (Key)key;
					InputMap.ActionEraseEvent(action, ev);
				}
				else if (action == "test_mouse"){
					InputEventMouseButton ev = new InputEventMouseButton();
					ev.ButtonIndex = (MouseButton)key;
					InputMap.ActionEraseEvent(action, ev);
				}
			}
			InputMap.EraseAction(action);
		 }
    }
}
