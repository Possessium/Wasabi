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
    }

    bool keyboard = true;

    private void Update()
    {
        if (Keyboard.current.tabKey.wasPressedThisFrame)
            SwitchControls();
    }

    void SwitchControls()
    {
        if (keyboard && Gamepad.all.Count >= 2)
        {
            keyboard = false;
            inputBan.SwitchCurrentControlScheme("Gamepad 2", Gamepad.all[0].device);
            inputLux.SwitchCurrentControlScheme("Gamepad", Gamepad.all[1].device);
        }

        else if (!keyboard)
        {
            keyboard = true;
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
