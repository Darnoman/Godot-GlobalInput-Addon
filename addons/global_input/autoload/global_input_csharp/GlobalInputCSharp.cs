#pragma warning disable CA1050

using Godot;
using SharpHook;
using SharpHook.Native;
using System.Collections.Generic;

public partial class GlobalInputCSharp : Node
{
    #region Setup
    readonly TaskPoolGlobalHook _hook = new();
    static readonly Dictionary<KeyCode, Key> _hKeyToGodotKey = new()
    {
        {KeyCode.VcUndefined, Key.None},

        {KeyCode.VcEscape, Key.Escape},
        {KeyCode.VcF1, Key.F1},
        {KeyCode.VcF2, Key.F2},
        {KeyCode.VcF3, Key.F3},
        {KeyCode.VcF4, Key.F4},
        {KeyCode.VcF5, Key.F5},
        {KeyCode.VcF6, Key.F6},
        {KeyCode.VcF7, Key.F7},
        {KeyCode.VcF8, Key.F8},
        {KeyCode.VcF9, Key.F9},
        {KeyCode.VcF10, Key.F10},
        {KeyCode.VcF11, Key.F11},
        {KeyCode.VcF12, Key.F12},
        {KeyCode.VcF13, Key.F13},
        {KeyCode.VcF14, Key.F14},
        {KeyCode.VcF15, Key.F15},
        {KeyCode.VcF16, Key.F16},
        {KeyCode.VcF17, Key.F17},
        {KeyCode.VcF18, Key.F18},
        {KeyCode.VcF19, Key.F19},
        {KeyCode.VcF20, Key.F20},
        {KeyCode.VcF21, Key.F21},
        {KeyCode.VcF22, Key.F22},
        {KeyCode.VcF23, Key.F23},
        {KeyCode.VcF24, Key.F24},

        {KeyCode.VcBackQuote, Key.Quoteleft},
        {KeyCode.Vc0, Key.Key0},
        {KeyCode.Vc1, Key.Key1},
        {KeyCode.Vc2, Key.Key2},
        {KeyCode.Vc3, Key.Key3},
        {KeyCode.Vc4, Key.Key4},
        {KeyCode.Vc5, Key.Key5},
        {KeyCode.Vc6, Key.Key6},
        {KeyCode.Vc7, Key.Key7},
        {KeyCode.Vc8, Key.Key8},
        {KeyCode.Vc9, Key.Key9},
        {KeyCode.VcMinus, Key.Minus},
        {KeyCode.VcEquals, Key.Equal},
        {KeyCode.VcBackspace, Key.Backspace},

        {KeyCode.VcTab, Key.Tab},
        {KeyCode.VcCapsLock, Key.Capslock},

        {KeyCode.VcA, Key.A},
        {KeyCode.VcB, Key.B},
        {KeyCode.VcC, Key.C},
        {KeyCode.VcD, Key.D},
        {KeyCode.VcE, Key.E},
        {KeyCode.VcF, Key.F},
        {KeyCode.VcG, Key.G},
        {KeyCode.VcH, Key.H},
        {KeyCode.VcI, Key.I},
        {KeyCode.VcJ, Key.J},
        {KeyCode.VcK, Key.K},
        {KeyCode.VcL, Key.L},
        {KeyCode.VcM, Key.M},
        {KeyCode.VcN, Key.N},
        {KeyCode.VcO, Key.O},
        {KeyCode.VcP, Key.P},
        {KeyCode.VcQ, Key.Q},
        {KeyCode.VcR, Key.R},
        {KeyCode.VcS, Key.S},
        {KeyCode.VcT, Key.T},
        {KeyCode.VcU, Key.U},
        {KeyCode.VcV, Key.V},
        {KeyCode.VcW, Key.W},
        {KeyCode.VcX, Key.X},
        {KeyCode.VcY, Key.Y},
        {KeyCode.VcZ, Key.Z},

        {KeyCode.VcOpenBracket, Key.Bracketleft},
        {KeyCode.VcCloseBracket, Key.Bracketright},
        {KeyCode.VcBackslash, Key.Backslash},
        {KeyCode.VcSemicolon, Key.Semicolon},
        {KeyCode.VcQuote, Key.Quotedbl},
        {KeyCode.VcEnter, Key.Enter},
        {KeyCode.VcComma, Key.Comma},
        {KeyCode.VcPeriod, Key.Period},
        {KeyCode.VcSlash, Key.Slash},
        {KeyCode.VcSpace, Key.Space},

        {KeyCode.VcPrintScreen, Key.Print},
        {KeyCode.VcInsert, Key.Insert},
        {KeyCode.VcHome, Key.Home},
        {KeyCode.VcPageUp, Key.Pageup},
        {KeyCode.VcDelete, Key.Delete},
        {KeyCode.VcEnd, Key.End},
        {KeyCode.VcPageDown, Key.Pagedown},

        {KeyCode.VcDown, Key.Down},
        {KeyCode.VcUp, Key.Up},
        {KeyCode.VcRight, Key.Right},
        {KeyCode.VcLeft, Key.Left},

        {KeyCode.VcNumLock, Key.Numlock},
        {KeyCode.VcNumPad0, Key.Kp0},
        {KeyCode.VcNumPad1, Key.Kp1},
        {KeyCode.VcNumPad2, Key.Kp2},
        {KeyCode.VcNumPad3, Key.Kp3},
        {KeyCode.VcNumPad4, Key.Kp4},
        {KeyCode.VcNumPad5, Key.Kp5},
        {KeyCode.VcNumPad6, Key.Kp6},
        {KeyCode.VcNumPad7, Key.Kp7},
        {KeyCode.VcNumPad8, Key.Kp8},
        {KeyCode.VcNumPad9, Key.Kp9},
        {KeyCode.VcNumPadDivide, Key.KpDivide},
        {KeyCode.VcNumPadMultiply, Key.KpMultiply},
        {KeyCode.VcNumPadSubtract, Key.KpSubtract},
        {KeyCode.VcNumPadAdd, Key.KpAdd},
        {KeyCode.VcNumPadDecimal, Key.KpPeriod},
        {KeyCode.VcNumPadEnter, Key.KpEnter},

        {KeyCode.VcLeftShift, Key.Shift},
        {KeyCode.VcRightShift, Key.Shift},
        {KeyCode.VcLeftControl, Key.Ctrl},
        {KeyCode.VcRightControl, Key.Ctrl},
        {KeyCode.VcLeftAlt, Key.Alt},
        {KeyCode.VcRightAlt, Key.Alt},
        {KeyCode.VcLeftMeta, Key.Meta},
        {KeyCode.VcRightMeta, Key.Meta},
    };
    #endregion

    #region Setup Functions
    void InitializeSharpHookSignals()
    {
        _hook.KeyPressed += OnHookKeyPressed;
        _hook.KeyReleased += OnHookKeyReleased;

        _hook.MousePressed += OnHookMousePressed;
        _hook.MouseReleased += OnHookMouseReleased;

        _hook.MouseWheel += OnHookMouseWheel;
    }
    #endregion

    #region Signals
    void OnHookKeyPressed(object _, KeyboardHookEventArgs args)
    {
        InputEventKey e = GetInputEventKey(args.Data.KeyCode, true);
        Input.ParseInputEvent(e);
    }

    void OnHookKeyReleased(object _, KeyboardHookEventArgs args)
    {
        InputEventKey e = GetInputEventKey(args.Data.KeyCode, false);
        Input.ParseInputEvent(e);
    }

    void OnHookMousePressed(object _, MouseHookEventArgs args)
    {
        InputEventMouseButton e = GetInputEventMouseButton(args.Data, true);
        Input.ParseInputEvent(e);
    }

    void OnHookMouseReleased(object _, MouseHookEventArgs args)
    {
        InputEventMouseButton e = GetInputEventMouseButton(args.Data, false);
        Input.ParseInputEvent(e);
    }

    void OnHookMouseWheel(object _, MouseWheelHookEventArgs args)
    {
        InputEventMouseButton e = GetInputEventMouseButton(args.RawEvent.Mouse, true);

        if (args.Data.Direction == MouseWheelScrollDirection.Vertical)
        {
            e.ButtonIndex = args.Data.Rotation > 0 ? Godot.MouseButton.WheelUp : Godot.MouseButton.WheelDown;
        }
        else
        {
            e.ButtonIndex = args.Data.Rotation > 0 ? Godot.MouseButton.WheelRight : Godot.MouseButton.WheelLeft;
        }

        Input.ParseInputEvent(e);
        e.Pressed = false;
        Input.ParseInputEvent(e);
    }
    #endregion

    #region Helper Functions
    static InputEventKey GetInputEventKey(KeyCode hKey, bool pressed)
    {
        Key godotKey = ToGodotKey(hKey);
        return new InputEventKey()
        {
            Keycode = godotKey,
            PhysicalKeycode = godotKey,
            Pressed = pressed,
            ShiftPressed = godotKey != Key.Shift && Input.IsKeyPressed(Key.Shift),
            CtrlPressed = godotKey != Key.Ctrl && Input.IsKeyPressed(Key.Ctrl),
            AltPressed = godotKey != Key.Alt && Input.IsKeyPressed(Key.Alt),
            MetaPressed = godotKey != Key.Meta && Input.IsKeyPressed(Key.Meta),
        };
    }

    static InputEventMouseButton GetInputEventMouseButton(MouseEventData mouse, bool pressed)
    {
        return new InputEventMouseButton()
        {
            ButtonIndex = (Godot.MouseButton)mouse.Button,
            Position = new Vector2(mouse.X, mouse.Y),
            ButtonMask = (MouseButtonMask)mouse.Button,
            Pressed = pressed,
            ShiftPressed = Input.IsKeyPressed(Key.Shift),
            CtrlPressed = Input.IsKeyPressed(Key.Ctrl),
            AltPressed = Input.IsKeyPressed(Key.Alt),
            MetaPressed = Input.IsKeyPressed(Key.Meta),
        };
    }

    static Key ToGodotKey(KeyCode hKey)
    {
        if (_hKeyToGodotKey.TryGetValue(hKey, out Key key))
        {
            return key;
        }

        return Key.None;
    }
    #endregion

    public override void _Ready()
    {
        InitializeSharpHookSignals();
        _hook.RunAsync();
    }
}
