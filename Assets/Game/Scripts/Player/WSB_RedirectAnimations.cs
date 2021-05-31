using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WSB_RedirectAnimations : MonoBehaviour
{
    WSB_PlayerInteraction playerInteraction = null;
    WSB_PlayerMovable playerMovable = null;
    bool isInteractionFound = false;
    bool isMovableFound = false;


    public void TryGrab()
    {
        if (!isInteractionFound)
            isInteractionFound = playerInteraction = GetComponentInParent<WSB_PlayerInteraction>();

        if (!isInteractionFound)
            return;

        playerInteraction.TryGrab();
    }

    public void DropObject()
    {
        if (!isInteractionFound)
            isInteractionFound = playerInteraction = GetComponentInParent<WSB_PlayerInteraction>();

        if (!isInteractionFound)
            return;

        playerInteraction.DropObject();
    }
    public void StopMoving()
    {
        if (!isMovableFound)
            isMovableFound = playerMovable = GetComponentInParent<WSB_PlayerMovable>();

        if (!isMovableFound)
            return;

        playerMovable.StopMoving();
    }

    public void AnimateLever()
    {
        if (!isInteractionFound)
            isInteractionFound = playerInteraction = GetComponentInParent<WSB_PlayerInteraction>();

        if (!isInteractionFound)
            return;

        playerInteraction.ToggleLever();
    }
}
