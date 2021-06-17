using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WSB_TriggerCam : MonoBehaviour
{
    [SerializeField] BoxCollider2D trigger = null;
        public BoxCollider2D Trigger { get { return trigger; } }

    #region Trigger values
    [SerializeField, Min(.1f)] Vector2 triggerSize = Vector2.one;

    [Header("Camera mode"), SerializeField] CamType changeTypeTo = CamType.None;
        public CamType Type { get { return changeTypeTo; } }
    [Header("Camera position"), SerializeField] Vector3 changePositionTo = Vector3.zero;
        public Vector3 Position { get { return changePositionTo; } }
    [Header("Split angle for SplitFixe"), SerializeField] float changeAngleTo = 0;
        public float Angle { get { return changeAngleTo; } }
    [Header("Camera Zoom"), SerializeField] float changeZoomTo = 0;
        public float Zoom { get { return changeZoomTo; } }
    //[Header("Camera FOV PERSPECTIVE ONLY"), SerializeField] float changeFOVTo = 0;
    //    public float FOV { get { return changeFOVTo; } }
    #endregion

    public int PlayersIn { get; private set; } = 0;

    private void OnDrawGizmos()
    {
        // For easy use
        // Color selected based on trigger type
        // Draw the trigger in that color
        // Draw a line to the position the camera will go
        switch (Type)
        {
            case CamType.Fixe:
                Gizmos.color = new Color(1, 0, 0, .4f);
                break;
            case CamType.Dynamic:
                Gizmos.color = new Color(0, 1, 0, .4f);
                break;
            case CamType.SplitFixe:
                Gizmos.color = new Color(1, 1, 0, .4f);
                break;
            case CamType.SplitDynamic:
                Gizmos.color = new Color(0, 1, 1, .4f);
                break;
            case CamType.None:
                Gizmos.color = new Color(.01f, .01f, .01f, .4f);
                break;
        }
        Gizmos.DrawCube(transform.position, new Vector3(triggerSize.x, triggerSize.y, 1));
        if(Type == CamType.Fixe)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(Position, .25f);
            Gizmos.DrawLine(Position, transform.position);
        }
    }

    private void Awake()
    {
        // Gets needed components if not set.
        // Throw errors if not found them destroy itself
        if (!trigger) trigger = GetComponent<BoxCollider2D>();
        if(!trigger)
        {
            Debug.LogError($"BoxCollider introuvable sur {transform.name}");
            Destroy(this);
        }
    }

    private void Start()
    {
        // Setup trigger
        trigger.isTrigger = true;
        trigger.size = triggerSize;
        //if (WSB_CameraManager.I.IsOrtho)
            changePositionTo = new Vector3(changePositionTo.x, changePositionTo.y, changeZoomTo);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        // If any player enters this trigger, send the trigger information to the camera manager
        if (col.GetComponent<WSB_PlayerMovable>())
        {
            PlayersIn++;
            WSB_CameraManager.I.TriggerEntered(this);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.GetComponent<WSB_PlayerMovable>())
        {
            PlayersIn--;
            if(PlayersIn == 0)
                WSB_CameraManager.I.TriggerExit(this);
        }
    }
}
