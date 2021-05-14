using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WSB_RedirectAnimations : MonoBehaviour
{
    WSB_PlayerInteraction playerInteraction = null;
    bool isFound = false;


    public void TryGrab()
    {
        if (!isFound)
            isFound = playerInteraction = GetComponentInParent<WSB_PlayerInteraction>();

        if (!isFound)
            return;

        playerInteraction.TryGrab();
    }

    public void DropObject()
    {
        if (!isFound)
            isFound = playerInteraction = GetComponentInParent<WSB_PlayerInteraction>();

        if (!isFound)
            return;

        playerInteraction.DropObject();
    }
}
