using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WSB_Shrink : WSB_Power
{
    [SerializeField] LayerMask layerShrink = 0;

    Coroutine shrinkDelay = null;
    Coroutine unshrinkDelay = null;
    [SerializeField] Vector2 offset = Vector2.zero;
    bool hasLux = false;

    public override void Start()
    {
        base.Start();
    }

    protected override void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 2, .3f, .6f);
        if(WSB_Lux.I)
            Gizmos.DrawWireSphere((Vector2)transform.position + offset, range * (WSB_Lux.I.Shrinked ? 1 : .9f));
        
        else
            Gizmos.DrawWireSphere((Vector2)transform.position + offset, range);
    }

    public override void Update()
    {
        base.Update();

        if (!IsActive)
            return;

            Shrink();
    }

    void Shrink()
    {
        if (!WSB_Lux.I.Shrinked && !WSB_Lux.I.HeldObject && Vector2.Distance(transform.position, WSB_Lux.I.transform.position) < range)
        {
            hasLux = true;
            
            if (unshrinkDelay != null)
                StopCoroutine(unshrinkDelay);
            unshrinkDelay = null;
            
            WSB_Lux.I.Shrink();
            shrinkDelay = StartCoroutine(ShrinkDelay());
            return;
        }

        else if (hasLux && Vector2.Distance(transform.position, WSB_Lux.I.transform.position) > range)
        {
            if (shrinkDelay != null)
                StopCoroutine(shrinkDelay);
            shrinkDelay = null;

            hasLux = false;

            WSB_Lux.I.Unshrink();
            unshrinkDelay = StartCoroutine(UnshrinkDelay());
            return;
        }
    }

    IEnumerator ShrinkDelay()
    {
        while(!WSB_Lux.I.Shrinked)
        {
            WSB_Lux.I.Shrink();
            yield return new WaitForEndOfFrame();
        }
        shrinkDelay = null;
    }

    IEnumerator UnshrinkDelay()
    {
        while(WSB_Lux.I.Shrinked)
        {
            WSB_Lux.I.Unshrink();
            yield return new WaitForEndOfFrame();
        }
        unshrinkDelay = null;
    }
    public override void DeactivatePower(WSB_Player _p)
    {
        base.DeactivatePower(_p);

        if (!hasLux)
            return;

        if (shrinkDelay != null)
            StopCoroutine(shrinkDelay);
        shrinkDelay = null;

        hasLux = false;

        //Debug.LogError("in");
        WSB_Lux.I.Unshrink();
        //Debug.LogError("inner");
    }

    private void OnDisable()
    {
        if (!hasLux)
            return;

        if (shrinkDelay != null)
            StopCoroutine(shrinkDelay);
        shrinkDelay = null;

        hasLux = false;

        Debug.LogError("in");
        WSB_Lux.I.Unshrink();
        Debug.LogError("inner");
    }
}
