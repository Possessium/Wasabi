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
    //Coroutine shrink = null;
    //Coroutine unshrink = null;

    [SerializeField] Vector2 startSize = Vector2.zero;
    [SerializeField] Vector3 startRenderSize = Vector3.zero;

    [SerializeField] WSB_PlayerMovable playerMovable = null;
    public WSB_PlayerMovable PlayerMovable { get { return playerMovable; } }
    [SerializeField] WSB_PlayerInteraction playerInteraction = null;
    public WSB_PlayerInteraction PlayerInteraction { get { return playerInteraction; } }

    [SerializeField] private Animator shrinkAnimator = null;
    private static readonly int shrink_Hash = Animator.StringToHash("Shrink");
    private static readonly int unshrink_Hash = Animator.StringToHash("Unshrink");


    // Set default calues to charges and adds custom update in game global update
    private void Start()
    {

        startSize = playerMovable.MovableCollider.size;
        startRenderSize = render.transform.localScale;
    }

    
    public void Shrink()
    {
        playerMovable.StopJump();
        playerMovable.RemoveSpeedCoef(1.5f);
        shrinkAnimator.ResetTrigger(unshrink_Hash);
        shrinkAnimator.SetTrigger(shrink_Hash);
        
        Shrinked = true;
    }

    public void Unshrink()
    {
        RaycastHit2D[] _hits = new RaycastHit2D[1];
        if (playerMovable.MovableCollider.Cast(Vector2.up, shrinkLayer, _hits, 1.5f, true) > 0)
            return;

        playerMovable.AddSpeedCoef(1.5f);
        shrinkAnimator.ResetTrigger(shrink_Hash);
        shrinkAnimator.SetTrigger(unshrink_Hash);
        
        Shrinked = false;
    }
}
