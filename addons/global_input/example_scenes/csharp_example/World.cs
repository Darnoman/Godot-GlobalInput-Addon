using Godot;
using System;
using System.Collections.Generic;

public partial class World : Node2D
{
	private Dictionary<string, long[]> Controls = new Dictionary<string, long[]>();
	private GlobalInputCSharp GlobalInput;

	
	// Called when the node enters the scene tree for the first time.
    public override void _EnterTree()
    {
		// You can do this via the Project Settings -> InputMap
		Controls.Add("move_left", new long[]{(long)Key.A}); // adds key input
		Controls.Add("move_right", new long[]{(long)Key.D}); 
		Controls.Add("move_up", new long[]{(long)Key.W}); 
		Controls.Add("move_down", new long[]{(long)Key.S});
		Controls.Add("mouse_left", new long[]{(long)MouseButton.Left}); 
		List<string> ControlKeyList = new List<string>(Controls.Keys);
		foreach (string action in ControlKeyList){
			if (!InputMap.HasAction(action)){
				InputMap.AddAction(action);
			}
			foreach (long key in Controls[action]){
				if (action == "move_left" || action == "move_right" || action == "move_up" || action == "move_down"){
					InputEventKey ev = new InputEventKey();
					ev.PhysicalKeycode = (Key)key;
					InputMap.ActionAddEvent(action, ev);
				}
				if (action == "mouse_left"){
					InputEventMouseButton ev = new InputEventMouseButton();
					ev.ButtonIndex = (MouseButton)key;
					InputMap.ActionAddEvent(action, ev);
				}
			}
		}
    }


	public override void _ExitTree()
    {
		List<string> ControlKeyList = new List<string>(Controls.Keys);
        foreach (string action in ControlKeyList){
			InputMap.EraseAction(action);
		 }
    }

}
