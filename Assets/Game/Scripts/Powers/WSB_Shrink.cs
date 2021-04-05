using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WSB_Shrink : WSB_Power
{
    [SerializeField] LayerMask layerShrink = 0;

    Coroutine shrinkDelay = null;
    Coroutine unshrinkDelay = null;
    [SerializeField] Vector2 offset = Vector2.zero;

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

        if (!isActive)
            return;

        Collider2D[] _hits = Physics2D.OverlapCircleAll((Vector2)transform.position + offset, range * (WSB_Lux.I.Shrinked ? 1 : .9f), layerShrink);

        if(!WSB_Lux.I.Shrinked)
        {
            for (int i = 0; i < _hits.Length; i++)
            {
                if (_hits[i].transform != WSB_Lux.I.transform)
                    continue;

                if (unshrinkDelay != null)
                    StopCoroutine(unshrinkDelay);
                unshrinkDelay = null;

                shrinkDelay = StartCoroutine(ShrinkDelay());
                break;
            }
        }
        else if(_hits.Length == 0)
        {
            if (shrinkDelay != null)
                StopCoroutine(shrinkDelay);
            shrinkDelay = null;

            unshrinkDelay = StartCoroutine(UnshrinkDelay());
        }
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.GetComponent<WSB_Lux>() && isActive)
    //    {
    //        if(unshrinkDelay != null)
    //            StopCoroutine(unshrinkDelay);
    //        unshrinkDelay = null;

    //        shrinkDelay = StartCoroutine(ShrinkDelay());
    //    }
    //}

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if(collision.GetComponent<WSB_Lux>() && isActive)
    //    {
    //        if(shrinkDelay != null)
    //            StopCoroutine(shrinkDelay);
    //        shrinkDelay = null;

    //        unshrinkDelay = StartCoroutine(UnshrinkDelay());
    //    }
    //}

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
}
