using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Linq;
using UnityEngine.UI;

public class WSB_GameManager : MonoBehaviour
{
    public static WSB_GameManager I { get; private set; }
    [SerializeField] bool paused = true;
    [SerializeField] Cinemachine.CinemachineBrain cinemachineBrain = null; 
    [SerializeField] WSB_TriggerCam triggerCamStart = null;
    [SerializeField] Animator endAnimator = null;
    private bool isEnded = false;
    public static bool Paused { get; private set; } = true;

    #region Pause
    [SerializeField] private GameObject pauseMenu = null;
    [SerializeField] private GameObject confirmQuit = null;
    [SerializeField] private GameObject confirmMenu = null;
    [SerializeField] private Button resumeButton = null;
    [SerializeField] private Button quitButton = null;
    [SerializeField] private Button menuButton = null;
    #endregion


    public event Action OnPause = null;
    public event Action OnResume = null;
    

    private void Awake()
    {
        I = this;
    }

    private void Start()
    {
        InputSystem.onDeviceChange += DeviceChange;
        OnPause += ShowPauseMenu;
        OnResume += HidePauseMenu;
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
        if (!_ctx.started && !isEnded)
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

    public void EndGame()
    {
        isEnded = true;
        if (Paused)
            Resume();

        endAnimator.enabled = true;
    }

    public void QuitGame() => Application.Quit();
    public void RestartGame() => SceneManager.LoadSceneAsync("Main Menus", LoadSceneMode.Single);

    public void NeedConfirmationMenu(bool _s)
    {
        if (_s)
        {
            confirmMenu.SetActive(true);
            menuButton.enabled = false;
            quitButton.enabled = false;
            resumeButton.enabled = false;
        }

        else
        {
            confirmMenu.SetActive(false);
            menuButton.enabled = true;
            quitButton.enabled = true;
            resumeButton.enabled = true;
        }
    }
    public void NeedConfirmationQuit(bool _s)
    {
        if (_s)
        {
            confirmQuit.SetActive(true);
            menuButton.enabled = false;
            quitButton.enabled = false;
            resumeButton.enabled = false;
        }

        else
        {
            confirmQuit.SetActive(false);
            menuButton.enabled = true;
            quitButton.enabled = true;
            resumeButton.enabled = true;
        }
    }

    private void ShowPauseMenu()
    {
        if(pauseMenu)
            pauseMenu.SetActive(true);
    }

    private void HidePauseMenu()
    {
        if(confirmMenu && confirmQuit && pauseMenu)
        {
            confirmQuit.SetActive(false);
            confirmMenu.SetActive(false);
            pauseMenu.SetActive(false);
        }
    }
}
