using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WSB_Elevator : MonoBehaviour
{
    public static WSB_Elevator I { get; private set; }

    [SerializeField] private Animator animator = null;
    [SerializeField] private BoxCollider2D block = null;
    [SerializeField] private BoxCollider2D trigger = null;
    [SerializeField] private ParticleSystem elevatorFX = null;

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
    }




    private void ActivateElevator()
    {
        animator.SetTrigger(startElevator_Hash);
        block.enabled = true;
    }

    public void ActivateTrigger()
    {
        elevatorFX.Stop();
        trigger.enabled = true;
    }

}
