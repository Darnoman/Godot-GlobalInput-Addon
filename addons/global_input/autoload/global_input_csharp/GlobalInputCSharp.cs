using Godot;
using System;
using System.Collections.Generic;
using SharpHook;
using System.Linq;

public partial class GlobalInputCSharp : Node
{
    #region Setup
    TaskPoolGlobalHook Hook = new();

    static string OverallEventName = "Overall";
    Dictionary<Key, SharpHook.Native.KeyCode[]> GodotKeyToSharpHookKey = new()
    {
        {Key.None, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcUndefined}},

        {Key.Escape, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcEscape}},
        {Key.F1, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcF1}},
        {Key.F2, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcF2}},
        {Key.F3, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcF3}},
        {Key.F4, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcF4}},
        {Key.F5, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcF5}},
        {Key.F6, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcF6}},
        {Key.F7, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcF7}},
        {Key.F8, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcF8}},
        {Key.F9, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcF9}},
        {Key.F10, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcF10}},
        {Key.F11, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcF11}},
        {Key.F12, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcF12}},
        {Key.F13, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcF13}},
        {Key.F14, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcF14}},
        {Key.F15, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcF15}},
        {Key.F16, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcF16}},
        {Key.F17, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcF17}},
        {Key.F18, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcF18}},
        {Key.F19, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcF19}},
        {Key.F20, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcF20}},
        {Key.F21, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcF21}},
        {Key.F22, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcF22}},
        {Key.F23, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcF23}},
        {Key.F24, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcF24}},

        {Key.Quoteleft, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcBackQuote}},
        {Key.Key0, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.Vc0}},
        {Key.Key1, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.Vc1}},
        {Key.Key2, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.Vc2}},
        {Key.Key3, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.Vc3}},
        {Key.Key4, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.Vc4}},
        {Key.Key5, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.Vc5}},
        {Key.Key6, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.Vc6}},
        {Key.Key7, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.Vc7}},
        {Key.Key8, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.Vc8}},
        {Key.Key9, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.Vc9}},
        {Key.Minus, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcMinus}},
        {Key.Equal, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcEquals}},
        {Key.Backspace, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcBackspace}},

        {Key.Tab, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcTab}},
        {Key.Capslock, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcCapsLock}},

        {Key.A, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcA}},
        {Key.B, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcB}},
        {Key.C, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcC}},
        {Key.D, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcD}},
        {Key.E, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcE}},
        {Key.F, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcF}},
        {Key.G, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcG}},
        {Key.H, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcH}},
        {Key.I, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcI}},
        {Key.J, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcJ}},
        {Key.K, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcK}},
        {Key.L, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcL}},
        {Key.M, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcM}},
        {Key.N, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcN}},
        {Key.O, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcO}},
        {Key.P, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcP}},
        {Key.Q, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcQ}},
        {Key.R, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcR}},
        {Key.S, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcS}},
        {Key.T, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcT}},
        {Key.U, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcU}},
        {Key.V, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcV}},
        {Key.W, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcW}},
        {Key.X, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcX}},
        {Key.Y, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcY}},
        {Key.Z, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcZ}},

        {Key.Bracketleft, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcOpenBracket}},
        {Key.Bracketright, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcCloseBracket}},
        {Key.Backslash, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcBackslash}},
        {Key.Semicolon, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcSemicolon}},
        {Key.Quotedbl, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcQuote}},
        {Key.Enter, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcEnter}},
        {Key.Comma, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcComma}},
        {Key.Period, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcPeriod}},
        {Key.Slash, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcSlash}},
        {Key.Space, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcSpace}},

        {Key.Print, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcPrintScreen}},
        {Key.Insert, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcInsert}},
        {Key.Home, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcHome}},
        {Key.Pageup, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcPageUp}},
        {Key.Delete, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcDelete}},
        {Key.End, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcEnd}},
        {Key.Pagedown, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcPageDown}},

        {Key.Down, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcDown}},
        {Key.Up, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcUp}},
        {Key.Right, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcRight}},
        {Key.Left, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcLeft}},

        {Key.Numlock, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcNumLock}},
        {Key.Kp0, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcNumPad0}},
        {Key.Kp1, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcNumPad1}},
        {Key.Kp2, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcNumPad2}},
        {Key.Kp3, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcNumPad3}},
        {Key.Kp4, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcNumPad4}},
        {Key.Kp5, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcNumPad5}},
        {Key.Kp6, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcNumPad6}},
        {Key.Kp7, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcNumPad7}},
        {Key.Kp8, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcNumPad8}},
        {Key.Kp9, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcNumPad9}},
        {Key.KpDivide, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcNumPadDivide}},
        {Key.KpMultiply, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcNumPadMultiply}},
        {Key.KpSubtract, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcNumPadSubtract}},
        {Key.KpAdd, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcNumPadAdd}},
        {Key.KpPeriod, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcNumPadDecimal}},
        {Key.KpEnter, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcNumPadEnter}},

        {Key.Shift, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcLeftShift, SharpHook.Native.KeyCode.VcRightShift}},
        {Key.Ctrl, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcLeftControl, SharpHook.Native.KeyCode.VcRightControl}},
        {Key.Alt, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcLeftAlt, SharpHook.Native.KeyCode.VcRightAlt}},
        {Key.Meta, new SharpHook.Native.KeyCode[] {SharpHook.Native.KeyCode.VcLeftMeta, SharpHook.Native.KeyCode.VcRightMeta}},
    };
    #endregion

    #region Setup Functions
    private void InitializeSharpHookSignals()
    {
        Hook.KeyPressed += OnHookKeyPressed;
        Hook.KeyReleased += OnHookKeyReleased;

        Hook.MousePressed += OnHookMousePressed;
        Hook.MouseReleased += OnHookMouseReleased;

        Hook.MouseWheel += OnHookMouseWheel;
    }
    #endregion

    #region Signals
    private void OnHookKeyPressed(object sender, KeyboardHookEventArgs e)
    {
        Key godotKey = MapSharpKeyToGodotKey(e.RawEvent.Keyboard.KeyCode);
        InputEventKey eventKey = new InputEventKey
        {
            Keycode = godotKey,
            PhysicalKeycode = godotKey,
            Pressed = true,
            ShiftPressed = godotKey != Key.Shift ? Input.IsKeyPressed(Key.Shift) : false,
            CtrlPressed = godotKey != Key.Ctrl ? Input.IsKeyPressed(Key.Ctrl) : false,
            AltPressed = godotKey != Key.Alt ? Input.IsKeyPressed(Key.Alt) : false,
            MetaPressed = godotKey != Key.Meta ? Input.IsKeyPressed(Key.Meta) : false,
        };
        Input.ParseInputEvent(eventKey);
        GC.Collect();
    }
    private void OnHookKeyReleased(object sender, KeyboardHookEventArgs e)
    {
        Key godotKey = MapSharpKeyToGodotKey(e.RawEvent.Keyboard.KeyCode);
        InputEventKey eventKey = new InputEventKey
        {
            Keycode = godotKey,
            PhysicalKeycode = godotKey,
            Pressed = false,
            ShiftPressed = godotKey != Key.Shift ? Input.IsKeyPressed(Key.Shift) : false,
            CtrlPressed = godotKey != Key.Ctrl ? Input.IsKeyPressed(Key.Ctrl) : false,
            AltPressed = godotKey != Key.Alt ? Input.IsKeyPressed(Key.Alt) : false,
            MetaPressed = godotKey != Key.Meta ? Input.IsKeyPressed(Key.Meta) : false,
        };
        Input.ParseInputEvent(eventKey);
        GC.Collect();
    }
    private void OnHookMousePressed(object sender, MouseHookEventArgs e)
    {
        InputEventMouseButton eventMouseButton = new InputEventMouseButton
        {
            ButtonIndex = (MouseButton)e.RawEvent.Mouse.Button,
            Position = new Vector2((float)e.RawEvent.Mouse.X, (float)e.RawEvent.Mouse.Y),
            ButtonMask = (MouseButtonMask)e.RawEvent.Mouse.Button,
            Pressed = true,
            ShiftPressed = Input.IsKeyPressed(Key.Shift),
            CtrlPressed = Input.IsKeyPressed(Key.Ctrl),
            AltPressed = Input.IsKeyPressed(Key.Alt),
            MetaPressed = Input.IsKeyPressed(Key.Meta),
        };
        Input.ParseInputEvent(eventMouseButton);
        GC.Collect();
    }
    private void OnHookMouseReleased(object sender, MouseHookEventArgs e)
    {
        InputEventMouseButton eventMouseButton = new InputEventMouseButton
        {
            ButtonIndex = (MouseButton)e.RawEvent.Mouse.Button,
            Position = new Vector2((float)e.RawEvent.Mouse.X, (float)e.RawEvent.Mouse.Y),
            ButtonMask = (MouseButtonMask)e.RawEvent.Mouse.Button,
            Pressed = false,
            ShiftPressed = Input.IsKeyPressed(Key.Shift),
            CtrlPressed = Input.IsKeyPressed(Key.Ctrl),
            AltPressed = Input.IsKeyPressed(Key.Alt),
            MetaPressed = Input.IsKeyPressed(Key.Meta),
        };
        Input.ParseInputEvent(eventMouseButton);
        GC.Collect();
    }
    private void OnHookMouseWheel(object sender, MouseWheelHookEventArgs e)
    {
        switch (e.RawEvent.Wheel.Direction)
        {
            case SharpHook.Native.MouseWheelScrollDirection.Vertical:
                if (e.RawEvent.Wheel.Rotation > 0)
                {
                    InputEventMouseButton eventMouseButton = new InputEventMouseButton
                    {
                        ButtonIndex = MouseButton.WheelUp,
                        Position = new Vector2((float)e.RawEvent.Mouse.X, (float)e.RawEvent.Mouse.Y),
                        Pressed = true,
                        ShiftPressed = Input.IsKeyPressed(Key.Shift),
                        CtrlPressed = Input.IsKeyPressed(Key.Ctrl),
                        AltPressed = Input.IsKeyPressed(Key.Alt),
                        MetaPressed = Input.IsKeyPressed(Key.Meta),
                    };
                    Input.ParseInputEvent(eventMouseButton);
                    InputEventMouseButton eventMouseButton2 = new InputEventMouseButton
                    {
                        ButtonIndex = MouseButton.WheelUp,
                        Position = new Vector2((float)e.RawEvent.Mouse.X, (float)e.RawEvent.Mouse.Y),
                        Pressed = false,
                        ShiftPressed = Input.IsKeyPressed(Key.Shift),
                        CtrlPressed = Input.IsKeyPressed(Key.Ctrl),
                        AltPressed = Input.IsKeyPressed(Key.Alt),
                        MetaPressed = Input.IsKeyPressed(Key.Meta),
                    };
                    Input.ParseInputEvent(eventMouseButton2);
                }
                else
                {
                    InputEventMouseButton eventMouseButton = new InputEventMouseButton
                    {
                        ButtonIndex = MouseButton.WheelDown,
                        Position = new Vector2((float)e.RawEvent.Mouse.X, (float)e.RawEvent.Mouse.Y),
                        Pressed = true,
                        ShiftPressed = Input.IsKeyPressed(Key.Shift),
                        CtrlPressed = Input.IsKeyPressed(Key.Ctrl),
                        AltPressed = Input.IsKeyPressed(Key.Alt),
                        MetaPressed = Input.IsKeyPressed(Key.Meta),
                    };
                    Input.ParseInputEvent(eventMouseButton);
                    InputEventMouseButton eventMouseButton2 = new InputEventMouseButton
                    {
                        ButtonIndex = MouseButton.WheelDown,
                        Position = new Vector2((float)e.RawEvent.Mouse.X, (float)e.RawEvent.Mouse.Y),
                        Pressed = false,
                        ShiftPressed = Input.IsKeyPressed(Key.Shift),
                        CtrlPressed = Input.IsKeyPressed(Key.Ctrl),
                        AltPressed = Input.IsKeyPressed(Key.Alt),
                        MetaPressed = Input.IsKeyPressed(Key.Meta),
                    };
                    Input.ParseInputEvent(eventMouseButton2);
                }
                break;
            case SharpHook.Native.MouseWheelScrollDirection.Horizontal:
                if (e.RawEvent.Wheel.Rotation > 0)
                {
                    InputEventMouseButton eventMouseButton = new InputEventMouseButton
                    {
                        ButtonIndex = MouseButton.WheelRight,
                        Position = new Vector2((float)e.RawEvent.Mouse.X, (float)e.RawEvent.Mouse.Y),
                        Pressed = true,
                        ShiftPressed = Input.IsKeyPressed(Key.Shift),
                        CtrlPressed = Input.IsKeyPressed(Key.Ctrl),
                        AltPressed = Input.IsKeyPressed(Key.Alt),
                        MetaPressed = Input.IsKeyPressed(Key.Meta),
                    };
                    Input.ParseInputEvent(eventMouseButton);
                    InputEventMouseButton eventMouseButton2 = new InputEventMouseButton
                    {
                        ButtonIndex = MouseButton.WheelRight,
                        Position = new Vector2((float)e.RawEvent.Mouse.X, (float)e.RawEvent.Mouse.Y),
                        Pressed = false,
                        ShiftPressed = Input.IsKeyPressed(Key.Shift),
                        CtrlPressed = Input.IsKeyPressed(Key.Ctrl),
                        AltPressed = Input.IsKeyPressed(Key.Alt),
                        MetaPressed = Input.IsKeyPressed(Key.Meta),
                    };
                    Input.ParseInputEvent(eventMouseButton2);
                }
                else
                {
                    InputEventMouseButton eventMouseButton = new InputEventMouseButton
                    {
                        ButtonIndex = MouseButton.WheelLeft,
                        Position = new Vector2((float)e.RawEvent.Mouse.X, (float)e.RawEvent.Mouse.Y),
                        Pressed = true,
                        ShiftPressed = Input.IsKeyPressed(Key.Shift),
                        CtrlPressed = Input.IsKeyPressed(Key.Ctrl),
                        AltPressed = Input.IsKeyPressed(Key.Alt),
                        MetaPressed = Input.IsKeyPressed(Key.Meta),
                    };
                    Input.ParseInputEvent(eventMouseButton);
                    InputEventMouseButton eventMouseButton2 = new InputEventMouseButton
                    {
                        ButtonIndex = MouseButton.WheelLeft,
                        Position = new Vector2((float)e.RawEvent.Mouse.X, (float)e.RawEvent.Mouse.Y),
                        Pressed = false,
                        ShiftPressed = Input.IsKeyPressed(Key.Shift),
                        CtrlPressed = Input.IsKeyPressed(Key.Ctrl),
                        AltPressed = Input.IsKeyPressed(Key.Alt),
                        MetaPressed = Input.IsKeyPressed(Key.Meta),
                    };
                    Input.ParseInputEvent(eventMouseButton2);
                }
                break;
            default:
                break;
        
        }
        GC.Collect();
    }
    #endregion
    
    #region Helper Functions
    private SharpHook.Native.KeyCode[] MapGodotKeyToSharpKey(Key godotKey)
    {
        if (GodotKeyToSharpHookKey.ContainsKey(godotKey)) return GodotKeyToSharpHookKey[godotKey]; // if the godot key has a Sharp key, return it
        return Array.Empty<SharpHook.Native.KeyCode>();
    }
    private Key MapSharpKeyToGodotKey(SharpHook.Native.KeyCode sharpKey)
    {
        foreach (Key godotKey in GodotKeyToSharpHookKey.Keys) // loop through each Godot key
        {
            SharpHook.Native.KeyCode[] sharpKeys = GodotKeyToSharpHookKey[godotKey];
            if (sharpKeys.Contains(sharpKey)) return godotKey;
        }
        return Key.None;
    }
    #endregion
    
    public override void _Ready()
    {
        InitializeSharpHookSignals();
        Hook.RunAsync();
    }
}
