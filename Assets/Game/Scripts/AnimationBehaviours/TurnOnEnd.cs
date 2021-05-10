using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOnEnd : StateMachineBehaviour
{
    int count = 0;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        count++;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        count--;
        if(stateInfo.normalizedTime * stateInfo.length >= stateInfo.length && count < 1)
        {
            animator.transform.eulerAngles = new Vector3(animator.transform.eulerAngles.x, animator.GetComponentInParent<WSB_PlayerMovable>().IsRight ? 90 : -90,    animator.transform.eulerAngles.z);
            animator.SetBool("Turning", false);
        }

    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
