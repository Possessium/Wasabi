﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class WSB_MainMenu : MonoBehaviour
{
    [SerializeField] MenuMode currentMode = MenuMode.Title;

    [SerializeField] Animator menuAnimator = null;

    private static readonly int activate_Hash = Animator.StringToHash("Activate");
    private static readonly int reset_Hash = Animator.StringToHash("Reset");
    private static readonly int back_Hash = Animator.StringToHash("Back");
    private static readonly int creditsTransition_Hash = Animator.StringToHash("CreditsTransition");

    bool canSkipTitle = true;

    private void Update()
    {
        if(currentMode == MenuMode.Title && canSkipTitle)
        {
            if (Keyboard.current.anyKey.wasPressedThisFrame || MouseClicked(Mouse.current))
                Next();

            if(Gamepad.all.Count == 1)
            {
                if(GamepadAnyButtonPressed(Gamepad.all[0]))
                    Next();
            }
            else if (Gamepad.all.Count == 2)
            {
                if (GamepadAnyButtonPressed(Gamepad.all[0]) || GamepadAnyButtonPressed(Gamepad.all[1]))
                    Next();
            }
        }
        else if(currentMode != MenuMode.Title && currentMode != MenuMode.Play)
        {
            if (Keyboard.current.escapeKey.wasPressedThisFrame)
                Back();


            if (Gamepad.all.Count == 1)
            {
                if (Gamepad.all[0].buttonWest.wasPressedThisFrame)
                    Back();
            }
            else if (Gamepad.all.Count == 2)
            {
                if (Gamepad.all[0].buttonWest.wasPressedThisFrame || Gamepad.all[1].buttonWest.wasPressedThisFrame)
                    Back();
            }
        }
    }

    private bool MouseClicked(Mouse _m)
    {
        return
            _m.backButton.wasPressedThisFrame ||
            _m.forwardButton.wasPressedThisFrame ||
            _m.leftButton.wasPressedThisFrame ||
            _m.middleButton.wasPressedThisFrame ||
            _m.rightButton.wasPressedThisFrame
            ;
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

    IEnumerator DelayTitle()
    {
        yield return new WaitForSeconds(.5f);
        canSkipTitle = true;
    }
    public void PlayGame()
    {
        currentMode = MenuMode.Play;
        WSB_SoundManager.I.ButtonStart();

        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Loading", UnityEngine.SceneManagement.LoadSceneMode.Additive);
        UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync("Main Menus");
    }

    [SerializeField] private GameObject menuGO = null;
    [SerializeField] private GameObject playGO = null;
    [SerializeField] private GameObject creditGO = null;
    [SerializeField] private EventSystem eventSystem = null;

    [SerializeField] private UnityEngine.UI.Button menuButton = null;
    [SerializeField] private UnityEngine.UI.Button tutoButton = null;
    [SerializeField] private UnityEngine.UI.Button creditsButton = null;
    public void Next()
    {
        PlayClickSound(true);
        switch (currentMode)
        {
            case MenuMode.Title:
                eventSystem.SetSelectedGameObject(menuGO);
                menuButton.Select();
                currentMode = MenuMode.Main;
                break;
            case MenuMode.Main:
                eventSystem.SetSelectedGameObject(playGO);
                tutoButton.Select();
                currentMode = MenuMode.Tuto;
                break;
            case MenuMode.Tuto:
                currentMode = MenuMode.Play;
                break;
        }
        menuAnimator.SetTrigger(activate_Hash);
    }

    public void Credits()
    {
        eventSystem.SetSelectedGameObject(creditGO);
        creditsButton.Select();
        PlayClickSound(true);
        menuAnimator.SetTrigger(creditsTransition_Hash);
        currentMode = MenuMode.Credits;
    }
    public void Back()
    {
        PlayClickSound(false);
        switch (currentMode)
        {
            case MenuMode.Main:
                eventSystem.SetSelectedGameObject(null);
                currentMode = MenuMode.Title;

                canSkipTitle = false;
                StartCoroutine(DelayTitle());
                break;
            case MenuMode.Tuto:
            case MenuMode.Credits:
                eventSystem.SetSelectedGameObject(menuGO);
                menuButton.Select();
                currentMode = MenuMode.Main;
                break;
        }
        menuAnimator.SetTrigger(back_Hash);
    }

    public void ChangeMusicVolume(float f) => WSB_SoundManager.I.ChangeMusicVolume(f);
    public void ChangeSoundVolume(float f) => WSB_SoundManager.I.ChangeSoundVolume(f);

    private void PlayClickSound(bool _1)
    {
        if (_1)
            WSB_SoundManager.I.Button1();
        else
            WSB_SoundManager.I.Button2();
    }

    public void Quit() => Application.Quit();
}

public enum MenuMode
{
    Title,
    Main,
    Tuto,
    Play,
    Credits
}