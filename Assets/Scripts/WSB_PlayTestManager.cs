﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using UnityEngine.EventSystems;

public class WSB_PlayTestManager : MonoBehaviour
{
    bool singlePlayer = true;

    [SerializeField] PlayerInput inputBan = null;
    [SerializeField] PlayerInput inputLux = null;

    [SerializeField] GameObject menu = null; 
    [SerializeField] GameObject menuPause = null; 
    public static bool Paused { get; private set; } = true;
    public static bool IsDialogue { get; private set; } = false;


    public static event Action OnUpdate = null;
    public static event Action OnPause = null;
    public static event Action OnResume = null;


    private void Start()
    {
        Rigidbody2D[] _physics = FindObjectsOfType<Rigidbody2D>();
        foreach (Rigidbody2D _r in _physics)
        {
            _r.isKinematic = true;
            _r.velocity = Vector2.zero;
            _r.angularVelocity = 0;
        }
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(menu.GetComponentInChildren<UnityEngine.UI.Button>().gameObject);
    }

    private void Update()
    {
        if (!Paused)
            OnUpdate?.Invoke();
    }

    public static void SetDialogue(bool _state) => IsDialogue = _state;

    public void ChangeCharacter(InputAction.CallbackContext _ctx)
    {
        if (!_ctx.started || Paused)
            return;

        if(singlePlayer)
        {
            inputBan.enabled = !inputBan.enabled;
            inputLux.enabled = !inputLux.enabled;
        }
    }

    public void Pause(InputAction.CallbackContext _ctx)
    {
        if (_ctx.ReadValue<float>() != 1 || !_ctx.started) 
            return;
        Paused = !Paused;
        if (Paused)
        {
            OnPause?.Invoke();
            menuPause.SetActive(true);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(menuPause.GetComponentInChildren<UnityEngine.UI.Button>().gameObject);
        }
        else
            Resume();
    }

    public void Resume()
    {
        Paused = false;
        OnResume?.Invoke();
        menuPause.SetActive(false);
    }

    public void StartGame(bool _singlePlayer)
    {
        if (Gamepad.all.Count == 0 || (_singlePlayer && Gamepad.all.Count != 1) || (!_singlePlayer && Gamepad.all.Count != 2))
            return;

        singlePlayer = _singlePlayer;
        Rigidbody2D[] _physics = FindObjectsOfType<Rigidbody2D>();
        foreach (Rigidbody2D _r in _physics)
        {
            if (_r.GetComponent<WSB_Player>())
                continue;
            _r.isKinematic = false;
        }
        Paused = false;
        if(singlePlayer)
        {
            inputBan.enabled = true;
            inputLux.enabled = false;
        }
        OnResume?.Invoke();
        menu.SetActive(false);
    }

    public void ReloadScene()
    {
        OnUpdate = null;
        OnPause = null;
        OnResume = null;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Hogu_PlayTest-1");
    }

    public void QuitGame() => Application.Quit();

}

/*
 * 
 * 
 * changer de persos    DONE (singleplayer uniquement, les multi s'échangeront la manette et pis merde)
 *      enable / disable les scripts sur select  (faire en sorte que les deux players se contrôlent avec la même manette)
 * 
 * 
 * tutoriels    DONE
 *      déplacement
 *      pouvoirs
 *      changement de persos
 *      
 * 
 * reset de la salle    DONE
 * 
 * mode 1 joueur    DONE
 * 
 * mode 2 joueurs   DONE
 * 
 * 
 * 
 * 
 */
