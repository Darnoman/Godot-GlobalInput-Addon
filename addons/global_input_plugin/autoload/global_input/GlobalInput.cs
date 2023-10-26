using Godot;
using Godot.Collections;
using System;
using System.Runtime.InteropServices;

public partial class GlobalInput : Node
{
	// Both
	
	[DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
    private static extern short GetKeyState(int nVirtKey);

	[DllImport("user32.dll", SetLastError = true)]
	private static extern uint SendInput(uint nInputs, Input[] pInputs, int cbSize);

	[DllImport("user32.dll")]
	private static extern IntPtr GetMessageExtraInfo();
	
	[StructLayout(LayoutKind.Sequential)]
	public struct HardwareInput{
		public uint uMsg;
		public ushort wParamL;
		public ushort wParamH;
	}
	
	[Flags]
	public enum InputType{
		Mouse = 0,
		Keyboard = 1,
		Hardware = 2
	}
	
	[StructLayout(LayoutKind.Explicit)]
	public struct InputUnion{
		[FieldOffset(0)] public MouseInput mi;
		[FieldOffset(0)] public KeyboardInput ki;
		[FieldOffset(0)] public HardwareInput hi;
	}
	
	public struct Input{
		public int type;
		public InputUnion u;

        public static explicit operator Input(Variant v)
        {
            throw new NotImplementedException();
        }
    }
	// Mouse
	/// <summary>
	/// Sets the mouse cursor to the give x and y cordinat without imitating mouse movement.
	/// </summary>
	/// <param name="x">The x position</param>
	/// <param name="y">The y position</param>
	/// <returns></returns>
	[DllImport("user32.dll", EntryPoint = "SetCursorPos")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool SetCursorPos(int x, int y);      

	/// <summary>
	/// Sets the given variable to the current mouse position
	/// </summary>
	/// <param name="lpMousePoint">the variable used to store the current mouse position</param>
	/// <returns></returns>
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool GetCursorPos(out MousePoint lpMousePoint);
	

	/// <summary>
	/// Imitates mouse events
	/// </summary>
	/// <param name="dwFlags"> the event you want to do</param>
	/// <param name="dx"> x position of mouse</param>
	/// <param name="dy"> y position of mouse</param>
	/// <param name="dwData"></param>
	/// <param name="dwExtraInfo"></param>
    [DllImport("user32.dll")]
    private static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);
	
	[Flags]
	public enum MouseButton{
		Left = 0,
		Middle = 1,
		Right = 2,
		XButton1 = 3,
		XButton2 = 4
	}
	
	[Flags]
	public enum MouseEventFlags{
		Absolute = 0x8000,
		HWheel = 0x01000,
		Move = 0x0001,
		MoveNoCoalesce = 0x2000,
		LeftDown = 0x0002,
		LeftUp = 0x0004,
		RightDown = 0x0008,
		RightUp = 0x0010,
		MiddleDown = 0x0020,
		MiddleUp = 0x0040,
		VirtualDesk = 0x4000,
		Wheel = 0x0800,
		XDown = 0x0080,
		XUp = 0x0100
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct MouseInput{
		public int dx;
		public int dy;
		public uint mouseData;
		public uint dwFlags;
		public uint time;
		public IntPtr dwExtraInfo;
	}

	[StructLayout(LayoutKind.Sequential)]
    public struct MousePoint{
        public int X;
        public int Y;

        public MousePoint(int x, int y){
            X = x;
            Y = y;
        }

        public override string ToString(){
            return $"({X}, {Y})";
        }
    }
	
	public MousePoint GetCursorPosition(){
		MousePoint currentMousePoint;
		var gotPoint = GetCursorPos(out currentMousePoint);
		if (!gotPoint) { currentMousePoint = new MousePoint(0,0); }
		return currentMousePoint;
	}

	public Vector2 GetMousePosition(){
		MousePoint position = GetCursorPosition();
		return new Vector2(position.X, position.Y);
	}

	public void SetCursorPosition(int x, int y){
		SetCursorPos(x, y);
	}

	public void SetCursorPosition(MousePoint point){
		SetCursorPosition(point.X, point.Y);
	}
	
	public void MouseEvent(MouseEventFlags value, MousePoint? point = null){
		if(point == null){ // default value of point
			point = GetCursorPosition();
		}
		MousePoint position = (MousePoint)point;
		mouse_event((int)value, position.X, position.Y, 0, 0);
	}
	/*
	
	*/
	public short GetMouseKeyState(MouseButton button){
		short state = GetKeyState((int)button);
		return state;
	}
	
	// Keyboard
	/*
	
	*/
	[DllImport("user32.dll", SetLastError = true)]
	static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);
	/*
	
	*/
	[DllImport("user32.dll")]static extern short VkKeyScan(char ch);
	/*
	
	*/
	[Flags]
	public enum KeyEventF{
		KeyDown = 0x0000,
		ExtendedKey = 0x0001,
		KeyUp = 0x0002,
		Unicode = 0x0004,
		Scancode = 0x0008
	}
	/*

	*/
	[StructLayout(LayoutKind.Explicit)]
	struct KeycodeHelper
	{
		[FieldOffset(0)]public short Value;
		[FieldOffset(0)]public byte Low;
		[FieldOffset(1)]public byte High;

        public override string ToString()
        {
            return $"Value: {Value} \nLow: {Low} \nHigh: {High}";
        }
    }
	/*

	*/
	[StructLayout(LayoutKind.Sequential)]
	public struct KeyboardInput{
		public ushort wVk;
		public ushort wScan;
		public uint dwFlags;
		public uint time;
		public IntPtr dwExtraInfo;
	}

	private Dictionary<int, int> GodotToKeyCode = new Dictionary<int, int>() {

	};
	

	public void KeyboardEvent(byte keycode, byte scancode, int keyFlag, int extraInfo){
		keybd_event(keycode, scancode, keyFlag, extraInfo);
	}

	public void KeyboardEvent(byte keycode, int keyFlag){
		keybd_event(keycode, 0, keyFlag, 0);
	}

	public short GetKeyboardState(int keycode){
		return GetKeyState(keycode);
	}

    // Godot Section
	/*
	{
		<StringName action>: {
			<int EventCode>: {
				"pressedState", false,
				...
			},
			<int EventCode>: {
				"pressedState", false,
				...
			},
		}
	}
	*/
	public Dictionary ActionDictionary = new Dictionary();

    public override void _Ready()
    {
		foreach(string action in InputMap.GetActions()){
			ActionDictionary.Add(action, new Dictionary<string,Dictionary<string,bool>>());
			foreach(InputEvent e in InputMap.ActionGetEvents(action)){
				if (e is InputEventMouseButton eventMouseButton){
					((Dictionary)ActionDictionary[action]).Add(eventMouseButton.ButtonIndex.ToString(), new Dictionary<string, bool>
													{
														{ "pressedState", false },
														{ "pressedPrevState", false },
														{ "clickedState", false },
														{ "clickedPrevState", false },
														{ "releasedState", false},
														{ "releasedPrevState", false}
													});
				}
				if (e is InputEventKey eventKey){
					if (!((Dictionary)ActionDictionary[action]).ContainsKey(eventKey.AsText())){
						((Dictionary)ActionDictionary[action]).Add(eventKey.AsText(), new Dictionary<string, bool>
													{
														{ "pressedState", false },
														{ "pressedPrevState", false },
														{ "clickedState", false },
														{ "clickedPrevState", false },
														{ "releasedState", false},
														{ "releasedPrevState", false}
													});
					}
				}
			}
		}
    }

	public bool IsActionJustPressed(string action){
		foreach(InputEvent e in InputMap.ActionGetEvents(action)){ // get all inputs within action
			if (e is InputEventMouseButton eventMouseButton){ // if input is a key
				Dictionary EventDictionary = ((Dictionary)((Dictionary)ActionDictionary[action])[eventMouseButton.ButtonIndex.ToString()]);
				EventDictionary["clickedPrevState"] = EventDictionary["clickedState"];
				EventDictionary["clickedState"] = GetKeyboardState(GetInputEventIdentifyer(eventMouseButton)) < 0;
				bool state = (bool)EventDictionary["clickedState"];
				bool prevState = (bool)EventDictionary["clickedPrevState"];
				if (state && !prevState){
					return true;
				}
			}
			else if (e is InputEventKey eventKey){
				Dictionary EventDictionary = ((Dictionary)((Dictionary)ActionDictionary[action])[eventKey.AsText()]);
				EventDictionary["clickedPrevState"] = EventDictionary["clickedState"];
				EventDictionary["clickedState"] = GetKeyboardState(GetInputEventIdentifyer(eventKey)) < 0;
				bool state = (bool)EventDictionary["clickedState"];
				bool prevState = (bool)EventDictionary["clickedPrevState"];
				if (state && !prevState){
					return true;
				}
			}
		}
		return false;
	}

	public bool IsActionPressed(string action){
		foreach(InputEvent e in InputMap.ActionGetEvents(action)){ // get all inputs within action
			if (e is InputEventMouseButton eventMouseButton){ // if input is a mouse button
				Dictionary EventDictionary = ((Dictionary)((Dictionary)ActionDictionary[action])[eventMouseButton.ButtonIndex.ToString()]);
				EventDictionary["pressedPrevState"] = EventDictionary["pressedState"];
				EventDictionary["pressedState"] = GetKeyboardState(GetInputEventIdentifyer(eventMouseButton)) < 0;
				bool state = (bool)EventDictionary["pressedState"];
				if (state){ // get state and see whether or not button is being pressed
					return true;
				}
			}
			else if (e is InputEventKey eventKey){ // if input is a mouse button
				Dictionary EventDictionary = ((Dictionary)((Dictionary)ActionDictionary[action])[eventKey.AsText()]);
				EventDictionary["pressedPrevState"] = EventDictionary["pressedState"];
				EventDictionary["pressedState"] = GetKeyboardState(GetInputEventIdentifyer(eventKey)) < 0;
				bool state = (bool)EventDictionary["pressedState"];
				if (state){ // get state and see whether or not button is being pressed
					return true;
				}
			}
		}
		return false;
	}

	public bool IsActionJustReleased(string action){
		foreach(InputEvent e in InputMap.ActionGetEvents(action)){
			if (e is InputEventMouseButton eventMouseButton){
				Dictionary EventDictionary = ((Dictionary)((Dictionary)ActionDictionary[action])[eventMouseButton.ButtonIndex.ToString()]);
				EventDictionary["releasedPrevState"] = EventDictionary["releasedState"];
				EventDictionary["releasedState"] = GetKeyboardState(GetInputEventIdentifyer(eventMouseButton)) < 0;
				bool state = (bool)EventDictionary["releasedState"];
				bool prevState = (bool)EventDictionary["releasedPrevState"];
				if (!state && prevState){
					return true;
				}
			}
			else if (e is InputEventKey eventKey){
				Dictionary EventDictionary = ((Dictionary)((Dictionary)ActionDictionary[action])[eventKey.AsText()]);
				EventDictionary["releasedPrevState"] = EventDictionary["releasedState"];
				EventDictionary["releasedState"] = GetKeyboardState(GetInputEventIdentifyer(eventKey)) < 0;
				bool state = (bool)EventDictionary["releasedState"];
				bool prevState = (bool)EventDictionary["releasedPrevState"];
				if (!state && prevState){
					return true;
				}
			}
		}
		return false;
	}

	public int GetInputEventIdentifyer(InputEvent e){
		if (e is InputEventMouseButton eventMouseButton){
			return (int)eventMouseButton.ButtonIndex;
		}
		else if (e is InputEventKey eventKey){
			char c = (Char)(int)eventKey.PhysicalKeycode;
			KeycodeHelper helper = new KeycodeHelper { Value = VkKeyScan(c)};
			byte VirtualKeyCode = helper.Low;
			byte ModifiyerState = helper.High;
			Key GodotKeyText = eventKey.KeyLabel;
			return (int)VirtualKeyCode;
		}
		return 0;
	}
}
