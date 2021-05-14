using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WSB_Shrink : WSB_Rune
{
    [SerializeField] Vector2 offset = Vector2.zero;
    bool hasLux = false;
    WSB_Lux lux = null;

    private void Start()
    {
        lux = WSB_Lux.I;
    }


    protected override void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 2, .3f, .6f);
        if(lux)
            Gizmos.DrawWireSphere((Vector2)transform.position + offset, range * (lux.Shrinked ? 1 : .9f));
        
        else
            Gizmos.DrawWireSphere((Vector2)transform.position + offset, range);
    }

    protected override void PlayPower()
    {
        if (!lux)
            return;

        if (!hasLux && !lux.Shrinked && !lux.PlayerInteraction.HeldObject && Vector2.Distance(transform.position, lux.transform.position) < range)
        {
            hasLux = true;

            lux.Shrink();

            return;
        }

        else if (hasLux && Vector2.Distance(transform.position, lux.transform.position) > range)
        {
            hasLux = false;

            lux.Unshrink();

            return;
        }
    }

    private void OnDisable()
    {
        if (!hasLux || !lux)
            return;

        hasLux = false;

        lux.Unshrink();
    }
}
