using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WSB_Shrink : WSB_Power
{
    Coroutine shrinkDelay = null;
    Coroutine unshrinkDelay = null;
    [SerializeField] Vector2 offset = Vector2.zero;
    bool hasLux = false;
    WSB_Lux lux = null;

    private void Start()
    {
        lux = FindObjectOfType<WSB_Lux>();
    }


    protected override void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 2, .3f, .6f);
        if(lux)
            Gizmos.DrawWireSphere((Vector2)transform.position + offset, range * (lux.Shrinked ? 1 : .9f));
        
        else
            Gizmos.DrawWireSphere((Vector2)transform.position + offset, range);
    }

    private void Update()
    {
        if (!IsActive || !lux)
            return;

        Shrink();
    }

    void Shrink()
    {
        if (!hasLux && !lux.Shrinked && !lux.PlayerInteraction.HeldObject && Vector2.Distance(transform.position, lux.transform.position) < range)
        {
            hasLux = true;

            if (unshrinkDelay != null)
                StopCoroutine(unshrinkDelay);
            unshrinkDelay = null;

            lux.Shrink();
            shrinkDelay = StartCoroutine(ShrinkDelay());
            return;
        }

        else if (hasLux && Vector2.Distance(transform.position, lux.transform.position) > range)
        {
            if (shrinkDelay != null)
                StopCoroutine(shrinkDelay);
            shrinkDelay = null;

            hasLux = false;

            lux.Unshrink();
            unshrinkDelay = StartCoroutine(UnshrinkDelay());
            return;
        }
    }

    IEnumerator ShrinkDelay()
    {
        lux.Shrink();
        while (!lux.Shrinked)
        {
            yield return new WaitForEndOfFrame();
        }
        shrinkDelay = null;
    }

    IEnumerator UnshrinkDelay()
    {
        lux.Unshrink();
        while (lux.Shrinked)
        {
            yield return new WaitForEndOfFrame();
        }
        unshrinkDelay = null;
    }

    private void OnDisable()
    {
        if (!hasLux || !lux)
            return;

        if (shrinkDelay != null)
            StopCoroutine(shrinkDelay);
        shrinkDelay = null;

        hasLux = false;

        lux.Unshrink();
    }
}
