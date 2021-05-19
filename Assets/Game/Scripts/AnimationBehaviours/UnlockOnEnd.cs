using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockOnEnd : StateMachineBehaviour
{
    WSB_PlayerInteraction playerInteraction = null;
    bool isFound = false;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!isFound)
            isFound = playerInteraction = animator.GetComponentInParent<WSB_PlayerInteraction>();

        if (!isFound)
            return;

        playerInteraction.AnimationFinished(false);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!isFound)
            isFound = playerInteraction = animator.GetComponentInParent<WSB_PlayerInteraction>();

        if (!isFound)
            return;

        playerInteraction.AnimationFinished(true);
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
