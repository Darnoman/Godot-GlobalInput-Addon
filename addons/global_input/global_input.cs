#if TOOLS
using Godot;
using System;

[Tool]
public partial class global_input : EditorPlugin
{
	private const string GlobalInputGdAutoloadName = "GlobalInput";
	public override void _EnterTree()
	{
		// Initialization of the plugin goes here.
		AddAutoloadSingleton(GlobalInputGdAutoloadName, "res://addons/global_input/autoload/global_input_gd/GlobalInputGd.tscn");
	}

	public override void _ExitTree()
	{
		// Clean-up of the plugin goes here.
		RemoveAutoloadSingleton(GlobalInputGdAutoloadName);
	}
}
#endif
