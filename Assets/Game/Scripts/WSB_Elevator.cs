using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WSB_Elevator : MonoBehaviour
{
    public static WSB_Elevator I { get; private set; }

    [SerializeField] private Animator animator = null;
    [SerializeField] private BoxCollider2D block = null;
    [SerializeField] private BoxCollider2D trigger = null;

    [SerializeField] private ParticleSystem elevatorStuckFX = null;

    [SerializeField] private WSB_SceneLoader bottomSceneLoader = null;
    [SerializeField] private WSB_SceneLoader stuckSceneLoader = null;

    private ElevatorState elevatorState = ElevatorState.Bottom;

    private static readonly int startElevator_Hash = Animator.StringToHash("Start");

    private int playersIn = 0;

    private void Awake()
    {
        I = this;
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
                bottomSceneLoader.OnScenesReady += StartElevator;
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

    void StartElevator()
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