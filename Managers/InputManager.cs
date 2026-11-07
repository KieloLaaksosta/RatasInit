using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace RatasInit.Managers;

public static class InputManager
{
    public static bool IsHeld(InputCode code) => _heldInputCodes.Contains(code);
    public static bool IsPressed(InputCode code) => _pressedInputCodes.Contains(code);
    public static bool IsReleased(InputCode code) => _releasedInputCodes.Contains(code);

    public static KeyBindings KeyBindings { get; set; }
    public static Action<Input> OnAny { get; set; }

    private static readonly HashSet<InputCode> _heldInputCodes = [];
    private static readonly HashSet<InputCode> _pressedInputCodes = [];
    private static readonly HashSet<InputCode> _releasedInputCodes = [];
    private static readonly HashSet<InputCode> _prevHeldInputCodes = [];

    private static int _prevScroll;

    public static void Update()
    {
        _prevHeldInputCodes.Clear();
        foreach (var code in _heldInputCodes)
        {
            _prevHeldInputCodes.Add(code);
        }

        _heldInputCodes.Clear();
        _pressedInputCodes.Clear();
        _releasedInputCodes.Clear();

        var mState = Mouse.GetState();
        if (mState.LeftButton == ButtonState.Pressed) AddInputToSet(new Input(MouseButtons.Left), _heldInputCodes);
        if (mState.RightButton == ButtonState.Pressed) AddInputToSet(new Input(MouseButtons.Right), _heldInputCodes);
        if (mState.MiddleButton == ButtonState.Pressed) AddInputToSet(new Input(MouseButtons.Middle), _heldInputCodes);

        if (mState.ScrollWheelValue < _prevScroll) AddInputToSet(new Input(MouseButtons.ScrollDown), _heldInputCodes);
        else if (mState.ScrollWheelValue > _prevScroll) AddInputToSet(new Input(MouseButtons.ScrollUp), _heldInputCodes);
        _prevScroll = mState.ScrollWheelValue;

        var kState = Keyboard.GetState();
        foreach (var key in kState.GetPressedKeys())
        {
            AddInputToSet(new Input(key), _heldInputCodes);
        }

        var padState = GamePad.GetState(0, GamePadDeadZone.None);
        if (padState.IsConnected && KeyBindings != null)
        {
            if (padState.Triggers.Left > KeyBindings.TriggersTreshold)
                AddInputToSet(new Input(AdditionalGamePadButtons.LeftTrigger), _heldInputCodes);
            if (padState.Triggers.Right > KeyBindings.TriggersTreshold)
                AddInputToSet(new Input(AdditionalGamePadButtons.RightTrigger), _heldInputCodes);

            var buttons = padState.Buttons;
            if (buttons.A == ButtonState.Pressed) AddInputToSet(new Input(Buttons.A), _heldInputCodes);
            if (buttons.B == ButtonState.Pressed) AddInputToSet(new Input(Buttons.B), _heldInputCodes);
            if (buttons.X == ButtonState.Pressed) AddInputToSet(new Input(Buttons.X), _heldInputCodes);
            if (buttons.Y == ButtonState.Pressed) AddInputToSet(new Input(Buttons.Y), _heldInputCodes);
            if (buttons.Back == ButtonState.Pressed) AddInputToSet(new Input(Buttons.Back), _heldInputCodes);
            if (buttons.Start == ButtonState.Pressed) AddInputToSet(new Input(Buttons.Start), _heldInputCodes);
            if (buttons.LeftShoulder == ButtonState.Pressed) AddInputToSet(new Input(Buttons.LeftShoulder), _heldInputCodes);
            if (buttons.RightShoulder == ButtonState.Pressed) AddInputToSet(new Input(Buttons.RightShoulder), _heldInputCodes);
            if (buttons.LeftStick == ButtonState.Pressed) AddInputToSet(new Input(Buttons.LeftStick), _heldInputCodes);
            if (buttons.RightStick == ButtonState.Pressed) AddInputToSet(new Input(Buttons.RightStick), _heldInputCodes);
            if (buttons.BigButton == ButtonState.Pressed) AddInputToSet(new Input(Buttons.BigButton), _heldInputCodes);

            var dPad = padState.DPad;
            if (dPad.Up == ButtonState.Pressed) AddInputToSet(new Input(AdditionalGamePadButtons.DPadUp), _heldInputCodes);
            if (dPad.Right == ButtonState.Pressed) AddInputToSet(new Input(AdditionalGamePadButtons.DPadRight), _heldInputCodes);
            if (dPad.Left == ButtonState.Pressed) AddInputToSet(new Input(AdditionalGamePadButtons.DPadLeft), _heldInputCodes);
            if (dPad.Down == ButtonState.Pressed) AddInputToSet(new Input(AdditionalGamePadButtons.DPadDown), _heldInputCodes);

            CheckThumbstickCode(padState.ThumbSticks.Left, AdditionalGamePadButtons.LeftStickRight, AdditionalGamePadButtons.LeftStickLeft, AdditionalGamePadButtons.LeftStickUp, AdditionalGamePadButtons.LeftStickDown, _heldInputCodes);
            CheckThumbstickCode(padState.ThumbSticks.Right, AdditionalGamePadButtons.RightStickRight, AdditionalGamePadButtons.RightStickLeft, AdditionalGamePadButtons.RightStickUp, AdditionalGamePadButtons.RightStickDown, _heldInputCodes);
        }

        foreach (var code in _heldInputCodes)
        {
            if (!_prevHeldInputCodes.Contains(code))
                _pressedInputCodes.Add(code);
        }

        foreach (var code in _prevHeldInputCodes)
        {
            if (!_heldInputCodes.Contains(code))
                _releasedInputCodes.Add(code);
        }
    }

    private static void AddInputToSet(Input input, HashSet<InputCode> targetSet)
    {
        foreach (var code in GetInputCodes(input))
        {
            targetSet.Add(code);
        }
    }

    private static List<InputCode> GetInputCodes(Input input)
    {
        if (KeyBindings?.Bindings != null && KeyBindings.Bindings.TryGetValue(input, out var codes))
            return codes;
        return [];
    }

    private static void CheckThumbstickCode(Vector2 stick, AdditionalGamePadButtons right, AdditionalGamePadButtons left, AdditionalGamePadButtons up, AdditionalGamePadButtons down, HashSet<InputCode> targetSet)
    {
        float threshold = KeyBindings?.SticksThreshold ?? 0.5f;
        if (MathF.Abs(stick.X) > MathF.Abs(stick.Y))
        {
            if (stick.X > threshold) AddInputToSet(new Input(right), targetSet);
            else if (stick.X < -threshold) AddInputToSet(new Input(left), targetSet);
        }
        else
        {
            if (stick.Y > threshold) AddInputToSet(new Input(up), targetSet);
            else if (stick.Y < -threshold) AddInputToSet(new Input(down), targetSet);
        }
    }
}