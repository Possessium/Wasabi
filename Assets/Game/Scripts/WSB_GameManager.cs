using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Linq;

public class WSB_GameManager : MonoBehaviour
{
    public static WSB_GameManager I { get; private set; }
    [SerializeField] bool paused = true;
    [SerializeField] Cinemachine.CinemachineBrain cinemachineBrain = null; 
    [SerializeField] WSB_TriggerCam triggerCamStart = null; 
    public static bool Paused { get; private set; } = true;


    public static event Action OnPause = null;
    public static event Action OnResume = null;

    private void Awake()
    {
        I = this;
    }

    private void Start()
    {
        InputSystem.onDeviceChange += DeviceChange;
    }

    private void DeviceChange(InputDevice arg1, InputDeviceChange arg2)
    {
        if(arg2 == InputDeviceChange.Disconnected)
        {
            Paused = true;

            OnPause?.Invoke();
        }
    }


    private void Update()
    {
        Paused = paused;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<WSB_PlayerMovable>())
            WSB_CheckpointManager.I.Respawn(collision.GetComponent<WSB_PlayerMovable>());
    }

    public void Pause(InputAction.CallbackContext _ctx)
    {
        // If input isn't start exit
        if (!_ctx.started)
            return;

        // Inverse pause state
        Paused = !Paused;

        // If the game is paused, invoke event, show menu and select the first item in the menu
        if (Paused)
        {
            OnPause?.Invoke();
        }
        else
            Resume();
    }

    public void Resume()
    {
        // Set Pause state to false, invoke resume event, hide pause menu
        Paused = false;
        OnResume?.Invoke();
    }

    public void StartGame()
    {
        // Set Pause to false
        Paused = false;

        cinemachineBrain.enabled = true;

        triggerCamStart.TriggerCinemachine();

        OnResume?.Invoke();
    }

    public void QuitGame() => Application.Quit();

}
