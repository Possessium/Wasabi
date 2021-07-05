using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

public class WSB_InputManager : MonoBehaviour
{
    public static WSB_InputManager I { get; private set; }

    [SerializeField] PlayerInput inputBan = null;
    [SerializeField] PlayerInput inputLux = null;


    private void Awake()
    {
        I = this;
    }

    private void Start()
    {
        InputSystem.onDeviceChange += InputSystem_onDeviceChange;
        inputBan.SwitchCurrentControlScheme("Keyboard&Mouse", Keyboard.current);
        inputLux.SwitchCurrentControlScheme("Keyboard&Mouse", Keyboard.current);

        SwitchControls();
    }

    public bool IsKeyboard { get; private set; } = true;

    private void Update()
    {
        if (IsKeyboard && Gamepad.all.Count > 1)
        {
            if (GamepadAnyButtonPressed(Gamepad.all[0]) || GamepadAnyButtonPressed(Gamepad.all[1]))
                SwitchControls();
        }

        else if (!IsKeyboard && Keyboard.current.wasUpdatedThisFrame)
            SwitchControls();
    }

    bool GamepadAnyButtonPressed(Gamepad _g)
    {
        return
            _g.buttonEast.wasPressedThisFrame ||
            _g.buttonNorth.wasPressedThisFrame ||
            _g.buttonSouth.wasPressedThisFrame ||
            _g.buttonWest.wasPressedThisFrame ||

            _g.aButton.wasPressedThisFrame ||
            _g.bButton.wasPressedThisFrame ||
            _g.xButton.wasPressedThisFrame ||
            _g.yButton.wasPressedThisFrame ||

            _g.circleButton.wasPressedThisFrame ||
            _g.crossButton.wasPressedThisFrame ||
            _g.squareButton.wasPressedThisFrame ||
            _g.triangleButton.wasPressedThisFrame ||

            _g.leftStickButton.wasPressedThisFrame ||
            _g.rightStickButton.wasPressedThisFrame ||

            _g.selectButton.wasPressedThisFrame ||
            _g.startButton.wasPressedThisFrame ||

            _g.leftTrigger.wasPressedThisFrame ||
            _g.rightTrigger.wasPressedThisFrame ||
            _g.leftShoulder.wasPressedThisFrame ||
            _g.rightShoulder.wasPressedThisFrame ||

            _g.dpad.left.wasPressedThisFrame ||
            _g.dpad.right.wasPressedThisFrame ||
            _g.dpad.up.wasPressedThisFrame ||
            _g.dpad.down.wasPressedThisFrame
            ;
    }

    void SwitchControls()
    {
        if (IsKeyboard && Gamepad.all.Count >= 2)
        {
            IsKeyboard = false;
            inputBan.SwitchCurrentControlScheme("Gamepad 2", Gamepad.all[0].device);
            inputLux.SwitchCurrentControlScheme("Gamepad", Gamepad.all[1].device);
        }

        else if (!IsKeyboard)
        {
            IsKeyboard = true;
            inputBan.SwitchCurrentControlScheme("Keyboard&Mouse", Keyboard.current);
            inputLux.SwitchCurrentControlScheme("Keyboard&Mouse", Keyboard.current);
        }
    }

    private void InputSystem_onDeviceChange(InputDevice arg1, InputDeviceChange arg2)
    {
        if (arg2 == InputDeviceChange.Disconnected)
            SwitchControls();
    }
}
