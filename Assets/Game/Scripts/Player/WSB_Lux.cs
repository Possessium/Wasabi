using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WSB_Lux : WSB_Player
{
    public static WSB_Lux I { get; private set; }

    [SerializeField] ContactFilter2D shrinkLayer;
    [SerializeField] float shrinkSpeed = 10;
    [SerializeField] GameObject render = null;
    public bool Shrinked { get; private set; } = false;
    Coroutine shrink = null;
    Coroutine unshrink = null;

    [SerializeField] Vector2 startSize = Vector2.zero;
    [SerializeField] Vector3 startRenderSize = Vector3.zero;

    // Populate the Instance of this script
    private void Awake()
    {
        I = this;
    }

    // Set default calues to charges and adds custom update in game global update
    public override void Start()
    {
        base.Start();

        WSB_GameManager.OnUpdate += MyUpdate;

        startSize = collider.size;
        startRenderSize = render.transform.localScale;
    }


    public override void Update()
    {
        // Has to be here and empty to override Unity update and use MyUpdate below
    }
    //  |
    //  |
    //  V
    // Update called on bound event
    void MyUpdate()
    {
        base.Update();
    }

    
    public void Shrink()
    {
        StopJump();
        if(shrink == null)
        {
            RemoveSpeedCoef(2);

            if(unshrink != null)
            {
                StopCoroutine(unshrink);
                unshrink = null;
            }
            shrink = StartCoroutine(ShrinkCoroutine());
        }
    }

    public void Unshrink()
    {
        if(unshrink == null)
        {
            RaycastHit2D[] _hits = new RaycastHit2D[1];
            if (collider.Cast(Vector2.up, shrinkLayer, _hits, 1.5f, true) > 0)
                return;

            AddSpeedCoef(2);

            if (shrink != null)
            {
                StopCoroutine(shrink);
                shrink = null;
            }
            unshrink = StartCoroutine(UnshrinkCoroutine());
        }
    }

    IEnumerator ShrinkCoroutine()
    {
        // Reduce size to half of the start's
        while (collider.size != startSize / 2)
        {
            collider.size = Vector2.MoveTowards(collider.size, startSize / 2, Time.deltaTime * shrinkSpeed);
            render.transform.localScale = Vector3.MoveTowards(render.transform.localScale, startRenderSize / 2, Time.deltaTime * shrinkSpeed);
            render.transform.localPosition = Vector3.MoveTowards(render.transform.localPosition, new Vector3(0, .6f, 0), Time.deltaTime * shrinkSpeed);
            yield return new WaitForEndOfFrame();
        }
        Shrinked = true;
        shrink = null;
    }

    IEnumerator UnshrinkCoroutine()
    {
        RaycastHit2D[] _hits = new RaycastHit2D[1];
        // Increase size back to the stocked start size
        while (collider.size != startSize || render.transform.localScale != startRenderSize)
        {
            // Checks above behind if there is a roof, loops until there isn't anymore
            if(collider.Cast(Vector2.up, shrinkLayer, _hits, 1.5f, true) == 0)
                collider.size = Vector2.MoveTowards(collider.size, startSize, Time.deltaTime * shrinkSpeed);

            render.transform.localScale = Vector3.MoveTowards(render.transform.localScale, startRenderSize, Time.deltaTime * shrinkSpeed);
            render.transform.localPosition = Vector3.MoveTowards(render.transform.localPosition, Vector3.zero, Time.deltaTime * shrinkSpeed);
            yield return new WaitForEndOfFrame();
        }
        Shrinked = false;
        unshrink = null;
    }

}
