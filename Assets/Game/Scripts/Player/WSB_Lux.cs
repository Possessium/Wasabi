using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WSB_Lux : MonoBehaviour
{
    [SerializeField] ContactFilter2D shrinkLayer;
    [SerializeField] float shrinkSpeed = 10;
    [SerializeField] GameObject render = null;
    public bool Shrinked { get; private set; } = false;
    Coroutine shrink = null;
    Coroutine unshrink = null;

    [SerializeField] Vector2 startSize = Vector2.zero;
    [SerializeField] Vector3 startRenderSize = Vector3.zero;

    [SerializeField] WSB_PlayerMovable playerMovable = null;
    public WSB_PlayerMovable PlayerMovable { get { return playerMovable; } }
    [SerializeField] WSB_PlayerInteraction playerInteraction = null;
    public WSB_PlayerInteraction PlayerInteraction { get { return playerInteraction; } }

    // Set default calues to charges and adds custom update in game global update
    private void Start()
    {

        startSize = playerMovable.MovableCollider.size;
        startRenderSize = render.transform.localScale;
    }

    
    public void Shrink()
    {
        playerMovable.StopJump();
        if(shrink == null)
        {
            playerMovable.RemoveSpeedCoef(2);

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
            if (playerMovable.MovableCollider.Cast(Vector2.up, shrinkLayer, _hits, 1.5f, true) > 0)
                return;

            playerMovable.AddSpeedCoef(2);

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
        while (playerMovable.MovableCollider.size != startSize / 2)
        {
            playerMovable.MovableCollider.size = Vector2.MoveTowards(playerMovable.MovableCollider.size, startSize / 2, Time.deltaTime * shrinkSpeed);
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
        BoxCollider2D _startCollider = playerMovable.MovableCollider;
        // Increase size back to the stocked start size
        while (playerMovable.MovableCollider.size != startSize || render.transform.localScale != startRenderSize)
        {
            // Checks above behind if there is a roof, loops until there isn't anymore
            if(_startCollider.Cast(Vector2.up, shrinkLayer, _hits, .5f, true) == 0)
                playerMovable.MovableCollider.size = Vector2.MoveTowards(playerMovable.MovableCollider.size, startSize, Time.deltaTime * shrinkSpeed);

            render.transform.localScale = Vector3.MoveTowards(render.transform.localScale, startRenderSize, Time.deltaTime * shrinkSpeed);
            render.transform.localPosition = Vector3.MoveTowards(render.transform.localPosition, Vector3.zero, Time.deltaTime * shrinkSpeed);
            yield return new WaitForEndOfFrame();
        }
        Shrinked = false;
        unshrink = null;
    }

}
