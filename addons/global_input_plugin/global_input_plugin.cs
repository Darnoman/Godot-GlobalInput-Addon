#if TOOLS
using Godot;
using System;

[Tool]
public partial class global_input_plugin : EditorPlugin
{

	private const string GlobalInputAutoloadName = "GlobalInput";
	public override void _EnterTree()
	{
		// Initialization of the plugin goes here.
		AddAutoloadSingleton(GlobalInputAutoloadName, "res://addons/global_input_plugin/autoload/global_input/GlobalInput.tscn");
	}

	public override void _ExitTree()
	{
		// Clean-up of the plugin goes here.
		RemoveAutoloadSingleton(GlobalInputAutoloadName);
	}
}
#endif
