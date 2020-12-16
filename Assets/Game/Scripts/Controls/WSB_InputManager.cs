﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WSB_InputManager : MonoBehaviour
{
    public static WSB_InputManager I { get; private set; }

    [SerializeField] PlayerInput inputBan;
    [SerializeField] PlayerInput inputLux;


    private void Awake()
    {
        I = this;
    }

    public bool ChangeControllers(ControlsMode _m, bool _isBanController)
    {
        switch (_m)
        {
            // Change scheme and devices to a Keyboard use
            case ControlsMode.Keyboard:
                inputBan.SwitchCurrentControlScheme("Keyboard", Keyboard.current);
                inputLux.SwitchCurrentControlScheme("Keyboard", Keyboard.current);
                return true;

            // Change scheme and devices to a Keyboard and Gamepad use
            case ControlsMode.ControllerKeyboard:
                // If Ban will play with the only controller and there is enough controller
                if (_isBanController && Gamepad.all.Count > 0)
                {
                    inputBan.SwitchCurrentControlScheme("Controller 1", Gamepad.all[0].device);
                    inputLux.SwitchCurrentControlScheme("Keyboard", Keyboard.current);
                    return true;
                }

                // If Lux will play with the only controller and there is enough controller
                else if (Gamepad.all.Count > 0)
                {
                    inputLux.SwitchCurrentControlScheme("Controller 2", Gamepad.all[0].device);
                    inputBan.SwitchCurrentControlScheme("Keyboard", Keyboard.current);
                    return true;
                }
                return false;

            // Change scheme and devices to 2 Gamepads
            case ControlsMode.Controller:

                // If there's not enough controller return false
                if (Gamepad.all.Count < 2)
                    return false;

                inputBan.SwitchCurrentControlScheme("Controller 1", Gamepad.all[0].device);
                inputLux.SwitchCurrentControlScheme("Controller 2", Gamepad.all[1].device);
                return true;

            default:
                return false;
        }
    }
}

public enum ControlsMode
{
    Keyboard,
    ControllerKeyboard,
    Controller
}