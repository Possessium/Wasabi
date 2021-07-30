using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WSB_Shrink : WSB_Rune
{
    [SerializeField] Vector2 offset = Vector2.zero;
    [SerializeField] bool hasLux = false;
    WSB_Lux lux = null;

    private void Start()
    {
        lux = WSB_Lux.I;
    }


    protected override void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 2, .3f, .6f);
        if(lux)
            Gizmos.DrawWireSphere((Vector2)transform.position + offset, range);
        
        else
            Gizmos.DrawWireSphere((Vector2)transform.position + offset, range);
    }

    protected override void PlayPower()
    {
        if (!lux)
            return;

        if (!hasLux && !lux.PlayerInteraction.HeldObject && Vector2.Distance(transform.position, lux.transform.position) < range)
        {
            hasLux = true;

            lux.Shrink();
            
            return;
            
        }

        else if (hasLux && !lux.PlayerInteraction.HeldObject && Vector2.Distance(transform.position, lux.transform.position) > range)
        {
            hasLux = false;

            lux.Unshrink();
            
            return;
            
        }
    }

    public override void ActivatePower()
    {
        base.ActivatePower();

        WSB_SoundManager.I.ShrinkSpawn(transform);
    }

    public override void DeactivatePower(WSB_PlayerMovable _p)
    {
        base.DeactivatePower(_p);

        WSB_SoundManager.I.ShrinkDespawn(transform);

        enabled = false;
    }

    private void OnDisable()
    {
        if (!hasLux || !lux)
            return;

        hasLux = false;

        lux.Unshrink();
    }
}
