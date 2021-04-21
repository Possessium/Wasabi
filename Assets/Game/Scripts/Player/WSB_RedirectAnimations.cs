using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WSB_RedirectAnimations : MonoBehaviour
{
    public void TryGrab()
    {
        if(GetComponentInParent<WSB_Player>())
            GetComponentInParent<WSB_Player>().TryGrab();
    }

    public void DropObject()
    {
        if (GetComponentInParent<WSB_Player>())
            GetComponentInParent<WSB_Player>().DropObject();
    }
}
