using System;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework.Input;

namespace RatasInit.Managers;

public enum InputCode
{
}

public enum InputType : byte
{
    Keyboard,
    Mouse,
    GamePadAdditionalButton,
    GamePadButton
}

public enum MouseButtons
{
    Left,
    Middle,
    Right,
    ScrollDown,
    ScrollUp,
}

public enum AdditionalGamePadButtons
{
    LeftStickUp,
    LeftStickDown,
    LeftStickLeft,
    LeftStickRight,
    RightStickUp,
    RightStickDown,
    RightStickLeft,
    RightStickRight,
    LeftTrigger,
    RightTrigger,
    DPadUp,
    DPadLeft,
    DPadRight,
    DPadDown,
}

[StructLayout(LayoutKind.Explicit)]
public readonly struct Input
{
    [FieldOffset(0)]
    public readonly InputType type;
    [FieldOffset(1)]
    public readonly Keys key;
    [FieldOffset(1)]
    public readonly MouseButtons mouseButton;
    [FieldOffset(1)]
    public readonly AdditionalGamePadButtons additionalGamePadButton;
    [FieldOffset(1)]
    public readonly Buttons gamePadButton;

    public Input(Keys key)
    {
        type = InputType.Keyboard;
        this.key = key;
    }

    public Input(MouseButtons button)
    {
        type = InputType.Mouse;
        mouseButton = button;
    }

    public Input(AdditionalGamePadButtons button)
    {
        type = InputType.GamePadAdditionalButton;
        additionalGamePadButton = button;
    }

    public Input(Buttons button)
    {
        type = InputType.GamePadButton;
        gamePadButton = button;
    }

    public static bool operator ==(Input input1, Input input2)
    {
        if (input1.type != input2.type) return false;
        return input1.type switch
        {
            InputType.Keyboard => input1.key == input2.key,
            InputType.Mouse => input1.mouseButton == input2.mouseButton,
            InputType.GamePadAdditionalButton => input1.additionalGamePadButton == input2.additionalGamePadButton,
            InputType.GamePadButton => input1.gamePadButton == input2.gamePadButton,
            _ => false
        };
    }

    public static bool operator !=(Input input1, Input input2) => !(input1 == input2);

    public override bool Equals(object obj)
    {
        if (obj is Input other)
            return this == other;
        return false;
    }

    public override int GetHashCode() => type switch
    {
        InputType.Keyboard => HashCode.Combine(type, key),
        InputType.Mouse => HashCode.Combine(type, mouseButton),
        InputType.GamePadAdditionalButton => HashCode.Combine(type, additionalGamePadButton),
        InputType.GamePadButton => HashCode.Combine(type, gamePadButton),
        _ => (int)type
    };
}
