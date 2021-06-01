using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class WSB_TriggerCam : MonoBehaviour
{
    [SerializeField] private Vector2 targetPosition = Vector2.zero;

    private bool moveToDestination = false;

    [SerializeField] private PlayableDirector playableDirector = null;
    [SerializeField] private bool stopPlayers = false;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(new Vector3(transform.position.x + targetPosition.x, transform.position.y + targetPosition.y, transform.position.z), .25f);
    }

    private void Update()
    {
        if(moveToDestination)
        {
            WSB_CameraManager.I.CamLux.SetCam(new Vector3(transform.position.x + targetPosition.x, transform.position.y + targetPosition.y, transform.position.z), TriggerCinemachine);
        }
    }

    void TriggerCinemachine()
    {
        playableDirector.Play();
        playableDirector.stopped += PlayableDirector_stopped;
    }

    private void PlayableDirector_stopped(PlayableDirector obj)
    {
        WSB_CameraManager.I.IsActive = true;
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
            if(stopPlayers)
            {
                WSB_Lux.I.PlayerMovable.StopMoving();
                WSB_Ban.I.Player.StopMoving();
            }
            WSB_CameraManager.I.IsActive = false;
            moveToDestination = true;
        }
    }
}
