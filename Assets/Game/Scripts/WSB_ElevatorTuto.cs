using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WSB_ElevatorTuto : MonoBehaviour
{
    [SerializeField] private Animator animator = null;

    private static readonly int animate_Hash = Animator.StringToHash("Animate");

    private int count = 0;

    public void RegisterSocle()
    {
        count++;
        if (count >= 2)
            TriggerElevator(true);
    }

    private void TriggerElevator(bool _s)
    {
        if (!animator)
            return;

        if(_s)
        {
            animator.SetTrigger(animate_Hash);
            animator.speed = 1;
        }
        else
        {
            animator.speed = 0;
        }
    }

    public void RemoveSocle()
    {
        count--;
        TriggerElevator(false);
    }
}
