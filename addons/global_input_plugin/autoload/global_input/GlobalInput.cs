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
	
	public MousePoint GetMousePointPosition(){
		MousePoint currentMousePoint;
		var gotPoint = GetCursorPos(out currentMousePoint);
		if (!gotPoint) { currentMousePoint = new MousePoint(0,0); }
		return currentMousePoint;
	}

	public Vector2 GetMousePosition(){
		MousePoint position = GetMousePointPosition();
		return new Vector2(position.X, position.Y);
	}

	public void SetMousePosition(int x, int y){
		SetCursorPos(x, y);
	}

	public void SetMousePosition(MousePoint point){
		SetMousePosition(point.X, point.Y);
	}
	
	public void SetMouseEvent(MouseEventFlags value, MousePoint? point = null){
		if(point == null){ // default value of point
			point = GetMousePointPosition();
		}
		MousePoint position = (MousePoint)point;
		mouse_event((int)value, position.X, position.Y, 0, 0);
	}

	public short GetMouseKeyState(MouseButton button){
		short state = GetKeyState((int)button);
		return state;
	}
	
	// Keyboard
	[DllImport("user32.dll", SetLastError = true)]
	static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

	[System.Runtime.InteropServices.DllImport("user32.dll")]
	private static extern short VkKeyScan(char ch);

	[Flags]
	public enum KeyEventF{
		KeyDown = 0x0000,
		ExtendedKey = 0x0001,
		KeyUp = 0x0002,
		Unicode = 0x0004,
		Scancode = 0x0008
	}

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
	Dictionary<string, int> GodotKeyToWindowKey = new() {
		// none
		{Key.None.ToString(), 0x00},
		// special
		{Key.Backspace.ToString(), 0x08},
		{Key.Tab.ToString(), 0x09},
		{Key.Clear.ToString(), 0x0C},
		{Key.Enter.ToString(), 0x0D},
		// modifiers
		{Key.Shift.ToString(), 0x10},
		{Key.Ctrl.ToString(), 0x11},
		{Key.Alt.ToString(), 0x12},
		{Key.Pause.ToString(), 0x13},
		{Key.Capslock.ToString(), 0x14},
		{Key.Escape.ToString(), 0x1B},
		// space
		{Key.Space.ToString(), 0x20},
		// island buttons
		{Key.Pageup.ToString(), 0x21},
		{Key.Pagedown.ToString(), 0x22},
		{Key.End.ToString(), 0x23},
		{Key.Home.ToString(), 0x24},
		// arrow keys
		{Key.Left.ToString(), 0x25},
		{Key.Up.ToString(), 0x26},
		{Key.Right.ToString(), 0x27},
		{Key.Down.ToString(), 0x28},
		// 
		{Key.Print.ToString(), 0x2C},
		{Key.Insert.ToString(), 0x2D},
		{Key.Delete.ToString(), 0x2E},
		{Key.Help.ToString(), 0x2F},
		// top numbers
		{Key.Key0.ToString(), 0x30},
		{Key.Key1.ToString(), 0x31},
		{Key.Key2.ToString(), 0x32},
		{Key.Key3.ToString(), 0x33},
		{Key.Key4.ToString(), 0x34},
		{Key.Key5.ToString(), 0x35},
		{Key.Key6.ToString(), 0x36},
		{Key.Key7.ToString(), 0x37},
		{Key.Key8.ToString(), 0x38},
		{Key.Key9.ToString(), 0x39},
		// letters
		{Key.A.ToString(), 0x41},
		{Key.B.ToString(), 0x42},
		{Key.C.ToString(), 0x43},
		{Key.D.ToString(), 0x44},
		{Key.E.ToString(), 0x45},
		{Key.F.ToString(), 0x46},
		{Key.G.ToString(), 0x47},
		{Key.H.ToString(), 0x48},
		{Key.I.ToString(), 0x49},
		{Key.J.ToString(), 0x4A},
		{Key.K.ToString(), 0x4B},
		{Key.L.ToString(), 0x4C},
		{Key.M.ToString(), 0x4D},
		{Key.N.ToString(), 0x4E},
		{Key.O.ToString(), 0x4F},
		{Key.P.ToString(), 0x50},
		{Key.Q.ToString(), 0x51},
		{Key.R.ToString(), 0x52},
		{Key.S.ToString(), 0x53},
		{Key.T.ToString(), 0x54},
		{Key.U.ToString(), 0x55},
		{Key.V.ToString(), 0x56},
		{Key.W.ToString(), 0x57},
		{Key.X.ToString(), 0x58},
		{Key.Y.ToString(), 0x59},
		{Key.Z.ToString(), 0x5A},
		// window key
		{"Windows", 0x5B},
		// numberpad numbers
		{Key.Kp0.ToString(), 0x60},
		{Key.Kp1.ToString(), 0x61},
		{Key.Kp2.ToString(), 0x62},
		{Key.Kp3.ToString(), 0x63},
		{Key.Kp4.ToString(), 0x64},
		{Key.Kp5.ToString(), 0x65},
		{Key.Kp6.ToString(), 0x66},
		{Key.Kp7.ToString(), 0x67},
		{Key.Kp8.ToString(), 0x68},
		{Key.Kp9.ToString(), 0x69},
		// nubmerpad operators
		{Key.KpMultiply.ToString(), 0x6A},
		{Key.KpAdd.ToString(), 0x6B},
		{Key.KpSubtract.ToString(), 0x6C},
		{Key.KpPeriod.ToString(), 0x6D},
		{Key.KpDivide.ToString(), 0x6E},
		// f-keys
		{Key.F1.ToString(), 0x70},
		{Key.F2.ToString(), 0x71},
		{Key.F3.ToString(), 0x72},
		{Key.F4.ToString(), 0x73},
		{Key.F5.ToString(), 0x74},
		{Key.F6.ToString(), 0x75},
		{Key.F7.ToString(), 0x76},
		{Key.F8.ToString(), 0x77},
		{Key.F9.ToString(), 0x78},
		{Key.F10.ToString(), 0x79},
		{Key.F11.ToString(), 0x7A},
		{Key.F12.ToString(), 0x7B},
		// numlock & scroll lock
		{Key.Numlock.ToString(), 0x90},
		{Key.Scrolllock.ToString(), 0x91},
		// OEM keys
		{Key.Quoteleft.ToString(), 0xC0},

	};
	
	Dictionary<string, int> GodotMouseToWindowMouse = new() {
		{MouseButton.Left.ToString().ToLower(), 0x01},
		{MouseButton.Right.ToString().ToLower(), 0x02},
		{MouseButton.Middle.ToString().ToLower(), 0x04},
		{MouseButton.XButton1.ToString().ToLower(), 0x05},
		{MouseButton.XButton2.ToString().ToLower(), 0x06},
	};
	
	/*{
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
	}*/
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
			if (e is InputEventMouseButton eventMouseButton){ // if input is a mouse button
				Dictionary EventDictionary = (Dictionary)((Dictionary)ActionDictionary[action])[eventMouseButton.ButtonIndex.ToString()];
				EventDictionary["clickedPrevState"] = EventDictionary["clickedState"];
				EventDictionary["clickedState"] = GetKeyboardState(GetInputEventIdentifyer(eventMouseButton)) < 0;
				bool state = (bool)EventDictionary["clickedState"];
				bool prevState = (bool)EventDictionary["clickedPrevState"];
				if (state && !prevState){
					return true;
				}
			}
			else if (e is InputEventKey eventKey){
				Dictionary EventDictionary = (Dictionary)((Dictionary)ActionDictionary[action])[eventKey.AsText()];
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
				Dictionary EventDictionary = (Dictionary)((Dictionary)ActionDictionary[action])[eventMouseButton.ButtonIndex.ToString()];
				EventDictionary["pressedPrevState"] = EventDictionary["pressedState"];
				EventDictionary["pressedState"] = GetKeyboardState(GetInputEventIdentifyer(eventMouseButton)) < 0;
				bool state = (bool)EventDictionary["pressedState"];
				if (state){ // get state and see whether or not button is being pressed
					return true;
				}
			}
			else if (e is InputEventKey eventKey){ // if input is a mouse button
				Dictionary EventDictionary = (Dictionary)((Dictionary)ActionDictionary[action])[eventKey.AsText()];
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
				Dictionary EventDictionary = (Dictionary)((Dictionary)ActionDictionary[action])[eventMouseButton.ButtonIndex.ToString()];
				EventDictionary["releasedPrevState"] = EventDictionary["releasedState"];
				EventDictionary["releasedState"] = GetKeyboardState(GetInputEventIdentifyer(eventMouseButton)) < 0;
				bool state = (bool)EventDictionary["releasedState"];
				bool prevState = (bool)EventDictionary["releasedPrevState"];
				if (!state && prevState){
					return true;
				}
			}
			else if (e is InputEventKey eventKey){
				Dictionary EventDictionary = (Dictionary)((Dictionary)ActionDictionary[action])[eventKey.AsText()];
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
			int MouseButtonIndex = 0;
			if (GodotMouseToWindowMouse.ContainsKey(eventMouseButton.ButtonIndex.ToString().ToLower())){
				MouseButtonIndex = GodotMouseToWindowMouse[eventMouseButton.ButtonIndex.ToString().ToLower()];
			}
			return MouseButtonIndex;
		}
		else if (e is InputEventKey eventKey){
			string keycodeString = OS.GetKeycodeString(eventKey.PhysicalKeycode); // get the name of the key
			// get the window keycode base on the GodotKeyToWindowKey map and return it
			int windowKeyCode = GodotKeyToWindowKey.ContainsKey(keycodeString) ? GodotKeyToWindowKey[keycodeString] : 0;
			if (windowKeyCode == 0){
				char c = (char)((int)eventKey.PhysicalKeycode);
				KeycodeHelper keycodeHelper = new KeycodeHelper { Value = VkKeyScan(c)};
				byte virtualKeycode = keycodeHelper.Low;
				byte shiftState = keycodeHelper.High;
				windowKeyCode = virtualKeycode;
			}
			return windowKeyCode;
		}
		return 0;
	}

}
