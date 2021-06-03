using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using PathCreation.Examples;

public class WSB_Elevator : MonoBehaviour
{
    public static WSB_Elevator I { get; private set; }

    [SerializeField] private Animator animator = null;
    [SerializeField] private BoxCollider2D block = null;
    [SerializeField] private BoxCollider2D trigger = null;

    [SerializeField] private ParticleSystem elevatorStuckFX = null;

    [SerializeField] private WSB_SceneLoader bottomSceneLoader = null;
    [SerializeField] private WSB_SceneLoader stuckSceneLoader = null;

    [SerializeField] private LineRenderer lineRendererLeft = null;
    [SerializeField] private LineRenderer lineRendererRight = null;
    [SerializeField] private Transform anchorLeft = null;
    [SerializeField] private Transform anchorRight = null;

    [SerializeField] private WSB_TriggerCam triggerCamToStuck = null;

    private ElevatorState elevatorState = ElevatorState.Bottom;

    private static readonly int startElevator_Hash = Animator.StringToHash("Start");

    private int playersIn = 0;

    private void Awake()
    {
        I = this;
    }

    private void Update()
    {
        lineRendererLeft.SetPosition(0, new Vector3(anchorLeft.position.x, anchorLeft.position.y, -24.5f));
        lineRendererRight.SetPosition(0, new Vector3(anchorRight.position.x, anchorRight.position.y, -24.5f));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.GetComponent<WSB_PlayerInteraction>())
            return;

        playersIn++;
        if (playersIn == 2)
            ActivateElevator();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.GetComponent<WSB_PlayerInteraction>())
            return;

        playersIn--;

        if (playersIn < 0)
            playersIn = 0;
    }


    private void ActivateElevator()
    {

        playersIn = 0;
        switch (elevatorState)
        {
            case ElevatorState.Bottom:
                bottomSceneLoader.OnScenesReady += TriggerCinemachine;
                triggerCamToStuck.MoveToDestination = true;
                bottomSceneLoader.NextScene();

                elevatorState = ElevatorState.Stuck;

                bottomSceneLoader.enabled = false;
                stuckSceneLoader.enabled = true;
                break;
            case ElevatorState.Stuck:
                stuckSceneLoader.OnScenesReady += StartElevator;
                stuckSceneLoader.NextScene();

                elevatorState = ElevatorState.Top;

                stuckSceneLoader.enabled = false;
                break;
        }
        block.enabled = true;
        trigger.enabled = false;
    }

    void TriggerCinemachine() => triggerCamToStuck.TriggerCinemachine();

    public void StartElevator()
    {
        animator.SetTrigger(startElevator_Hash);
        switch (elevatorState)
        {
            case ElevatorState.Stuck:
                bottomSceneLoader.OnScenesReady -= StartElevator;
                break;
            case ElevatorState.Top:
                stuckSceneLoader.OnScenesReady -= StartElevator;
                break;
        }
    }

    public void ActivateTrigger()
    {
        switch (elevatorState)
        {
            case ElevatorState.Bottom:
                break;
            case ElevatorState.Stuck:
                elevatorStuckFX.Stop();
                trigger.enabled = true;
                break;
            case ElevatorState.Top:
                break;
        }
    }

    public void DisableTrigger()
    {
        switch (elevatorState)
        {
            case ElevatorState.Bottom:
                break;
            case ElevatorState.Stuck:
                elevatorStuckFX.Play();
                trigger.enabled = false;
                break;
            case ElevatorState.Top:
                break;
            default:
                break;
        }
    }

}

public enum ElevatorState
{
    Bottom,
    Stuck,
    Top
}