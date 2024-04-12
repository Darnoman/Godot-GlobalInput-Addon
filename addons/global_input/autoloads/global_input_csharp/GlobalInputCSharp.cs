using Godot;
using Godot.Collections;
using System;
using System.Runtime.InteropServices;

public partial class GlobalInputCSharp : Node
{
	#region Setup

		#region Input
			[StructLayout(LayoutKind.Sequential)]
			private struct MouseInput {
				public int dx;
				public int dy;
				public uint mouseData;
				public uint dwFlags;
				public uint time;
				public IntPtr dwExtraInfo;
			}
			
			[StructLayout(LayoutKind.Sequential)]
			private struct KeyboardInput {
				public ushort wVk;
				public ushort wScan;
				public uint dwFlags;
				public uint time;
				public IntPtr dwExtraInfo;
			}

			[StructLayout(LayoutKind.Sequential)]
			private struct HardwareInput {
				public uint uMsg;
				public ushort wParamL;
				public ushort wParamH;
			}

			[StructLayout(LayoutKind.Explicit)]
			private struct InputUnion {
				[FieldOffset(0)]
				public MouseInput mi;
				[FieldOffset(0)]
				public KeyboardInput ki;
				[FieldOffset(0)]
				public HardwareInput hi;
			}

			private struct Input{
				public int type;
				public InputUnion u;

				public static explicit operator Input(Variant v) {
					throw new NotImplementedException();
				}
			}

			[Flags]
			public enum InputType {
				Mouse = 0,
				Keyboard = 1,
				Hardware = 2,
			}

			[Flags]
			public enum MouseEventFlags {
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
			private struct MousePoint {
				public int X;
				public int Y;

				public MousePoint(int x, int y) {
					X = x;
					Y = y;
				}

				public override string ToString()
				{
					return $"({X}, {Y})";
				}
			}

			[Flags]
			public enum KeyEventFlags {
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

			[Flags]
			public enum MapVirtualKeyTypes : int {
				MAPVK_VK_TO_VSC = 0x00,
				MAPVK_VSC_TO_VK = 0x01,
				MAPVK_VK_TO_CHAR = 0x02,
				MAPVK_VSC_TO_VK_EX = 0x03,
				MAPVK_VK_TO_VSC_EX = 0x04
			}
		#endregion

		#region Win32 Functions
			// Imports the SendInput function from user32.dll to simulate keyboard and mouse input
			[DllImport("user32.dll", SetLastError = true)]
			private static extern uint SendInput(uint nInputs, Input[] pInputs, int cbSize);
			
			// Imports the MapVirtualKey function from user32.dll to translate a virtual-key code into a scan code or character value
			[DllImport("user32.dll")]
			private static extern uint MapVirtualKey(uint uCode, uint uMapType);
			
			// Imports the GetMessageExtraInfo function from user32.dll to retrieve extra message information
			[DllImport("user32.dll")]
			private static extern IntPtr GetMessageExtraInfo();
			
			// Imports the GetCursorPos function from user32.dll to retrieve the cursor's position
			[DllImport("user32.dll")]
			[return: MarshalAs(UnmanagedType.Bool)]
			private static extern bool GetCursorPos(out MousePoint lpMousePoint);
			
			// Imports the SetCursorPos function from user32.dll to set the cursor's position
			[DllImport("user32.dll", EntryPoint = "SetCursorPos")]
			[return: MarshalAs(UnmanagedType.Bool)]
			private static extern bool SetCursorPos(int x, int y);
			
			// Imports the mouse_event function from user32.dll to synthesize mouse motion and button clicks
			[DllImport("user32.dll")]
			private static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);
			
			// Imports the keybd_event function from user32.dll to synthesize keystrokes
			[DllImport("user32.dll", SetLastError = true)]
			private static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);
			
			// Imports the GetKeyState function from user32.dll to determine whether a key is up or down at the time the function is called
			[DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
			private static extern short GetKeyState(int nVirtKey);
			
			// Imports the VkKeyScan function from user32.dll to translate a character to the corresponding virtual-key code and shift state
			[DllImport("user32.dll")]
			private static extern short VkKeyScan(char ch);
		#endregion

		#region Godot Specific Dictionaries

			/// <summary>
			/// Maps Godot key codes to Windows key codes
			/// </summary>
			public Dictionary<string, int> GodotKeyToWindowKey = new() {
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
				{"Meta", 0x5B},
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
    			{Key.F13.ToString(), 0x7C},
				{Key.F14.ToString(), 0x7D},
				{Key.F15.ToString(), 0x7E},
				{Key.F16.ToString(), 0x7F},
				{Key.F17.ToString(), 0x80},
				{Key.F18.ToString(), 0x81},
				{Key.F19.ToString(), 0x82},
				{Key.F20.ToString(), 0x83},
				{Key.F21.ToString(), 0x84},
				{Key.F22.ToString(), 0x85},
				{Key.F23.ToString(), 0x86},
				{Key.F24.ToString(), 0x87},
				// numlock & scroll lock
				{Key.Numlock.ToString(), 0x90},
				{Key.Scrolllock.ToString(), 0x91},
				// OEM keys
				{Key.Quoteleft.ToString(), 0xC0},
			};

			/// <summary>
			/// Maps Godot mouse buttons to Window's virtual keycodes
			/// </summary>
			public Dictionary<string, int> GodotMouseToWindowMouse = new() {
				{MouseButton.Left.ToString().ToLower(), 0x01},
				{MouseButton.Right.ToString().ToLower(), 0x02},
				{MouseButton.Middle.ToString().ToLower(), 0x04},
				{MouseButton.Xbutton1.ToString().ToLower(), 0x05},
				{MouseButton.Xbutton2.ToString().ToLower(), 0x06},
			};
	
		#endregion
	#endregion

	#region General Mouse and Keyboard Functions
		// GetMousePointPosition function returns the current position of the mouse as a MousePoint object.
		private MousePoint GetMousePointPosition() {
        var gotPoint = GetCursorPos(out MousePoint currentMousePoint);
        if (!gotPoint) {
				currentMousePoint = new MousePoint(0, 0);
			}
			return currentMousePoint;
		}

		// Retrieves the current mouse position and returns it as a Vector2.
		public Vector2 GetMousePosition() {
			MousePoint mousePointPosition = GetMousePointPosition();
			return new Vector2(mousePointPosition.X, mousePointPosition.Y);
		}

		// Sets the mouse position to the specified coordinates.
		public void SetMousePosition(Vector2I newPosition) {
			SetCursorPos(newPosition.X, newPosition.Y);
		}

		public int GetMouseAndKeyState(int keycode) {
			return GetKeyState(keycode);
		}

		private Input[] CreateKeyboardInput(KeyEventFlags flag, int keycode, int scancode = 0) {
			Input[] inputs = new Input[] {
				new Input {
					type = (int)InputType.Keyboard,
					u = new InputUnion {
						ki = new KeyboardInput {
							wVk = (ushort)keycode,
							wScan = (ushort)scancode,
							dwFlags = (uint)(flag),
							dwExtraInfo = GetMessageExtraInfo()
						}
					}
				}
			};
			return inputs;
		}
		private Input[] CreateMouseInput(MouseEventFlags flags, Vector2? mouseOffset = null) {
			if (mouseOffset == null) {
				mouseOffset = Vector2.Zero;
			}
			Input[] inputs = new Input[] {
				new Input {
					type = (int)InputType.Mouse,
					u = new InputUnion {
						mi = new MouseInput {
							dwFlags = (uint)flags,
							dx = (int)((Vector2)mouseOffset).X,
							dy = (int)((Vector2)mouseOffset).Y,
							dwExtraInfo = GetMessageExtraInfo(),
						}
					}
				}
			};
			return inputs;
		}
		
		/// <summary>
		/// Sends input based on the specified input type and flags.
		/// </summary>
		/// <param name="type">The type of input to send.</param>
		/// <param name="flags">The flags associated with the input.</param>
		/// <param name="parameters">Optional parameters for the input (default is null).
		/// Keyboard: (KeyEventFlags flag, int keycode, int scancode = 0)
		/// Mouse: (MouseEventFlags flags, Vector2? mouseOffset = null)</param>
		public int SendInput(int type, Array<Variant> parameters) {
			if (type == (int)InputType.Keyboard) {
				KeyEventFlags flag = (KeyEventFlags)((int)parameters[0]);
				int keycode = (int)parameters[1];
				int scancode = 0;
				if (parameters.Count > 2) {
					scancode = (int)parameters[2];
					flag = flag | KeyEventFlags.Scancode;
				}
				GD.Print(keycode," ", scancode);
				Input[] inputs = CreateKeyboardInput(flag, keycode, scancode);
				return (int)SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(Input)));
			}
			else if (type == (int)InputType.Mouse) {
				MouseEventFlags mouseFlags = (MouseEventFlags)((int)parameters[0]);
				Vector2? mouseOffset = (Vector2?)parameters[1];
				Input[] inputs = CreateMouseInput(mouseFlags, mouseOffset);
				return (int)SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(Input)));
			}
			return 0;
		}
	
	#endregion

	public Dictionary ActionDictionary = new Dictionary();

	/// <summary>
	/// Initialize the action dictionary with actions from the InputMap and their corresponding input events.
	/// </summary>
	private void InitializeActionDictionary() {
		foreach(string action in InputMap.GetActions()) { // get all actions within the InputMap
			ActionDictionary.Add(action, new Dictionary<string, Dictionary<string, bool>>()); // add them to the ActionDictionary
			foreach(InputEvent e in InputMap.ActionGetEvents(action)) {
				switch (e) {
					case InputEventMouseButton eventMouseButton when !((Dictionary)ActionDictionary[action]).ContainsKey(eventMouseButton.ButtonIndex.ToString()):
						((Dictionary)ActionDictionary[action]).Add(eventMouseButton.ButtonIndex.ToString(), new Dictionary<string, bool>
						{
							{"pressedState", false},
							{"pressedPrevState", false},
							{"justPressedState", false},
							{"justPressedPrevState", false},
							{"justReleasedState", false},
							{"justReleasedPrevState", false},
						});
						break;

					case InputEventKey eventKey when !((Dictionary)ActionDictionary[action]).ContainsKey(eventKey.AsText()):
						((Dictionary)ActionDictionary[action]).Add(eventKey.AsText(), new Dictionary<string, bool>
						{
							{"pressedState", false},
							{"pressedPrevState", false},
							{"justPressedState", false},
							{"justPressedPrevState", false},
							{"justReleasedState", false},
							{"justReleasedPrevState", false},
						});
						break;
				}
			}
		}
	}

    public override void _Ready()
    {
        InitializeActionDictionary();
    }

	/// <summary>
	/// Checks if the specified action has just been pressed.
	/// </summary>
	/// <param name="action">The name of the action to check.</param>
	/// <returns>True if the action has just been pressed, false otherwise.</returns>
	public bool IsActionJustPressed(StringName action) {
			foreach(InputEvent e in InputMap.ActionGetEvents(action)) {
				string eventType = null;
				string eventString = null;
				KeyModifierMask? eventModifierMask = null; 
	
				switch (e) {
					case InputEventMouseButton eventMouseButton:
						eventType = "MouseButton";
						eventString = eventMouseButton.ButtonIndex.ToString();
						eventModifierMask = eventMouseButton.GetModifiersMask(); 
						break;
					case InputEventKey eventKey:
						eventType = "Key";
						eventString = eventKey.AsText();
						eventModifierMask = eventKey.GetModifiersMask(); 
						break;
				}
				if (eventType == null) return false; // exits if event is neither a mouse button nor a key
	
				Dictionary EventDictionary = (Dictionary)((Dictionary)ActionDictionary[action])[eventString];
	
				bool eventModifierState = IsEventModifierPressed((KeyModifierMask) eventModifierMask);
				
				EventDictionary["justPressedPrevState"] = EventDictionary["justPressedState"];
				EventDictionary["justPressedState"] = GetMouseAndKeyState(GetInputEventIdentifier(e)) < 0 && eventModifierState;
				bool state = (bool)EventDictionary["justPressedState"];
				bool prevState = (bool)EventDictionary["justPressedPrevState"];
				if (state && !prevState) {

					return true;
				}
			}
			return false;
	}

	/// <summary>
	/// Check if the specified action is currently pressed.
	/// </summary>
	/// <param name="action">The name of the action to check.</param>
	/// <returns>True if the action is currently pressed, false otherwise.</returns>
	public bool IsActionPressed(StringName action) {
		foreach(InputEvent e in InputMap.ActionGetEvents(action)) {
			string eventType = null;
			string eventString = null;
			KeyModifierMask? eventModifierMask = null; 

			switch (e) {
				case InputEventMouseButton eventMouseButton:
					eventType = "MouseButton";
					eventString = eventMouseButton.ButtonIndex.ToString();
					eventModifierMask = eventMouseButton.GetModifiersMask(); 
					break;
				case InputEventKey eventKey:
					eventType = "Key";
					eventString = eventKey.AsText();
					eventModifierMask = eventKey.GetModifiersMask(); 
					break;
			}
			if (eventType == null) return false; // exits if event is neither a mouse button nor a key

			Dictionary EventDictionary = (Dictionary)((Dictionary)ActionDictionary[action])[eventString];

			bool eventModifierState = IsEventModifierPressed((KeyModifierMask) eventModifierMask);

			EventDictionary["pressedPrevState"] = EventDictionary["pressedState"];
			EventDictionary["pressedState"] = GetMouseAndKeyState(GetInputEventIdentifier(e)) < 0 && eventModifierState;
			bool state = (bool)EventDictionary["pressedState"];
			bool prevState = (bool)EventDictionary["pressedPrevState"];

			if (state && prevState) {
				return true;
			}
		}
		return false;
	}

	/// <summary>
	/// Checks if the given action has just been released.
	/// </summary>
	/// <param name="action">The name of the action to be checked.</param>
	/// <returns>True if the action has just been released, otherwise returns false.</returns>
	public bool IsActionJustReleased(StringName action) {
		foreach(InputEvent e in InputMap.ActionGetEvents(action)) {
				string eventType = null;
				string eventString = null;
				KeyModifierMask? eventModifierMask = null; 
	
				switch (e) {
					case InputEventMouseButton eventMouseButton:
						eventType = "MouseButton";
						eventString = eventMouseButton.ButtonIndex.ToString();
						eventModifierMask = eventMouseButton.GetModifiersMask(); 
						break;
					case InputEventKey eventKey:
						eventType = "Key";
						eventString = eventKey.AsText();
						eventModifierMask = eventKey.GetModifiersMask(); 
						break;
				}
				if (eventType == null) return false; // exits if event is neither a mouse button nor a key
	
				Dictionary EventDictionary = (Dictionary)((Dictionary)ActionDictionary[action])[eventString];
	
				bool eventModifierState = IsEventModifierPressed((KeyModifierMask) eventModifierMask);

				EventDictionary["justReleasedPrevState"] = EventDictionary["justReleasedState"];
				EventDictionary["justReleasedState"] = GetMouseAndKeyState(GetInputEventIdentifier(e)) < 0 && eventModifierState;
				bool state = (bool)EventDictionary["justReleasedState"];
				bool prevState = (bool)EventDictionary["justReleasedPrevState"];
				if (!state && prevState) {
					return true;
				}
			}
			return false;
	}

	/// <summary>
	/// Checks if the specified key is currently pressed.
	/// </summary>
	/// <param name="key">The key to check.</param>
	/// <returns>True if the specified key is pressed, false otherwise.</returns>
	public bool IsKeyPressed(Key key) {
		int windowKeyCode = GodotKeyToWindowKey.ContainsKey(key.ToString()) ? GodotKeyToWindowKey[key.ToString()] : 0;
		if (windowKeyCode == 0) {
			char c = (char) key;
			KeycodeHelper keycodeHelper = new KeycodeHelper { Value = VkKeyScan(c) };
			windowKeyCode = keycodeHelper.Low;
		}
		int keyState = GetMouseAndKeyState(windowKeyCode);
		if (keyState < 0) {
			return true;
		}
		return false;
	}

	/// <summary>
	/// GetVector takes four StringName parameters and returns a Vector2 object.
	/// </summary>
	/// <param name="negativeX"></param>
	/// <param name="positiveX"></param>
	/// <param name="negativeY"></param>
	/// <param name="positiveY"></param>
	/// <returns></returns>
	public Vector2 GetVector(StringName negativeX, StringName positiveX, StringName negativeY, StringName positiveY){
        Vector2 inputVector = new Vector2
        {
            X = (IsActionPressed(positiveX) ? 1 : 0) - (IsActionPressed(negativeX) ? 1 : 0),
            Y = (IsActionPressed(positiveY) ? 1 : 0) - (IsActionPressed(negativeY) ? 1 : 0)
        };
        return inputVector;
	}

	/// <summary>
	/// Checks if any input action is currently pressed.
	/// </summary>
	/// <returns>Boolean indicating if any input action is pressed.</returns>
	public bool IsAnythingPressed(){
		foreach(string action in InputMap.GetActions()){
			if (IsActionPressed(action)){
				return true;
			}
		}
		return false;
	}


	/// <summary>
	/// A method to check if the event modifier key is pressed, and return the state of the event modifier.
	/// </summary>
	/// <param name="eventModifierMask">The KeyModifierMask to check for pressed state.</param>
	/// <returns>A boolean indicating whether the event modifier key is pressed or not.</returns>
	public bool IsEventModifierPressed(KeyModifierMask eventModifierMask) {
		bool eventModifierState = true;
		if (eventModifierMask > 0) {
			string eventModifierString = eventModifierMask.ToString().Replace("Mask", "");
			string[] eventModifierStringArray = eventModifierString.Split(", ");
			foreach(string eventModifier in eventModifierStringArray) {
				int eventModifierKeyCode = GodotKeyToWindowKey[eventModifier];
				if(GetMouseAndKeyState(eventModifierKeyCode) >= 0) { // if any of the modifiers are unpressed
					eventModifierState = false;
				}
			}
		}
		return eventModifierState;
	}

	/// <summary>
	/// GetInputEventIdentifier method takes an InputEvent as input and returns an integer output based on the type of input event and its properties.
	/// </summary>
	/// <param name="e">The input InputEvent</param>
	/// <returns>An integer representing the input event identifier</returns>
	public int GetInputEventIdentifier(InputEvent e) {
		switch (e)
		{
			case InputEventMouseButton eventMouseButton when GodotMouseToWindowMouse.ContainsKey(eventMouseButton.ButtonIndex.ToString().ToLower()):
				return GodotMouseToWindowMouse[eventMouseButton.ButtonIndex.ToString().ToLower()];
			case InputEventKey eventKey:
				string keycodeString = OS.GetKeycodeString(eventKey.PhysicalKeycode).Replace(" ", "");
				if (keycodeString == "")
				{
					keycodeString = eventKey.AsText();
				}
				int windowKeyCode = GodotKeyToWindowKey.ContainsKey(keycodeString) ? GodotKeyToWindowKey[keycodeString] : 0;
				if (windowKeyCode == 0)
				{
					char c = (char)((int) eventKey.PhysicalKeycode);
					KeycodeHelper keycodeHelper = new KeycodeHelper { Value = VkKeyScan(c)};
					byte virtualKeycode = keycodeHelper.Low;
					byte shiftState = keycodeHelper.High;
					windowKeyCode = virtualKeycode;
				}
				return windowKeyCode;
			default:
				return 0;
		}
	}
	
	/// <summary>
	/// Maps a virtual-key code or a scan code to a character value.
	/// </summary>
	/// <param name="code">The virtual-key code or scan code for a key.</param>
	/// <param name="mapType">The type of mapping to be performed.</param>
	/// <returns>The translated character value.</returns>
	public int MapVirtualKey(int code, int mapType) {
		return (int)MapVirtualKey((uint)code, (uint)mapType);
	}
}
