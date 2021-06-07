using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class WSB_TriggerCam : MonoBehaviour
{
    [SerializeField] private Vector2 targetPosition = Vector2.zero;

    public bool MoveToDestination = false;

    [SerializeField] private Animator animator = null;
    [SerializeField] private bool stopPlayers = false;

    [SerializeField] private int nextZoom = 0;
    [SerializeField] private bool changeZoom = false;
    [SerializeField] private bool changePos = false;
    [SerializeField] private bool playOnStart = false;
    [SerializeField] private bool isElevator = false;

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
        if(MoveToDestination)
        {
            WSB_CameraManager.I.CamLux.SetCam(new Vector3(
                changePos ? transform.position.x + targetPosition.x : WSB_CameraManager.I.GetDynamicMiddlePosition().x,
                changePos ? transform.position.y + targetPosition.y : WSB_CameraManager.I.GetDynamicMiddlePosition().y,
                changeZoom ? nextZoom : WSB_CameraManager.I.CamLux.Cam.orthographicSize),
                TriggerCinemachine);
        }
    }

    public void TriggerCinemachine()
    {
        WSB_CameraManager.I.ToggleSplit(false);
        WSB_CameraManager.I.IsActive = false;

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
        {
            WSB_CameraManager.I.ChangeZoom(nextZoom);
            nextZoom = 19;
        }

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
        if (!MoveToDestination && col.GetComponent<WSB_PlayerMovable>() && !isElevator)
        {
            if (stopPlayers)
                StopPlayers();

            if (changeZoom)
                WSB_CameraManager.I.ChangeZoom(nextZoom);

            WSB_CameraManager.I.ToggleSplit(false);
            WSB_CameraManager.I.IsActive = false;
            MoveToDestination = true;
        }
    }
}
