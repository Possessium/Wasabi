using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class WSB_TriggerCam : MonoBehaviour
{
    [SerializeField] private Vector2 targetPosition = Vector2.zero;

    private bool moveToDestination = false;

    [SerializeField] private Animator animator = null;
    [SerializeField] private bool stopPlayers = false;

    [SerializeField] private int nextZoom = 0;
    [SerializeField] private bool changeZoom = false;
    [SerializeField] private bool changePos = false;
    [SerializeField] private bool playOnStart = false;

    private void Start()
    {
        if (playOnStart)
            TriggerCinemachine();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(new Vector3(transform.position.x + targetPosition.x, transform.position.y + targetPosition.y, transform.position.z), .25f);
    }

    private void Update()
    {
        if(moveToDestination)
        {
            WSB_CameraManager.I.CamLux.SetCam(new Vector3(
                changePos ? transform.position.x + targetPosition.x : WSB_CameraManager.I.CamLux.transform.position.x,
                changePos ? transform.position.y + targetPosition.y : WSB_CameraManager.I.CamLux.transform.position.y,
                changeZoom ? nextZoom : WSB_CameraManager.I.CamLux.Cam.orthographicSize),
                TriggerCinemachine);
        }

        /* Cheat codes */

        if (Keyboard.current.numpad9Key.isPressed)
        {
            if (animator)
                animator.enabled = false;
            AnimationEnded();
            Camera.main.GetComponent<Cinemachine.CinemachineBrain>().enabled = false;
            WSB_CameraManager.I.ChangeZoom(12);
        }

    }

    public void TriggerCinemachine()
    {
        if (stopPlayers)
            StopPlayers();

        if (animator)
            animator.enabled = true;

        else
            AnimationEnded();
    }

    private void StopPlayers()
    {
        WSB_Lux.I.PlayerMovable.StopMoving();
        WSB_Lux.I.PlayerMovable.ResetAnimations();
        WSB_Ban.I.Player.StopMoving();
        WSB_Ban.I.Player.ResetAnimations();
    }

    public void AnimationEnded()
    {
        WSB_CameraManager.I.IsActive = true;
        if (changeZoom)
            WSB_CameraManager.I.ChangeZoom(nextZoom);

        if (stopPlayers)
        {
            WSB_Lux.I.PlayerMovable.CanMove = true;
            WSB_Ban.I.Player.CanMove = true;
        }
        Destroy(this);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        // If any player enters this trigger, send the trigger information to the camera manager
        if (!moveToDestination && col.GetComponent<WSB_PlayerMovable>())
        {
            if (stopPlayers)
                StopPlayers();

            WSB_CameraManager.I.IsActive = false;
            moveToDestination = true;
        }
    }
}
