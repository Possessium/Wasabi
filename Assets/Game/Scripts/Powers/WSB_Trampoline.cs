using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WSB_Trampoline : WSB_Power
{
    [SerializeField] float trampolineForce = 10;
    [SerializeField] BoxCollider2D bounceCollider = null;

    protected override void OnDrawGizmos()
    {
        // don't show range on that plant
    }

    [SerializeField] LayerMask layerBounce;
    RaycastHit2D[] hits = new RaycastHit2D[2];

    public override void Update()
    {
        base.Update();

        Physics2D.BoxCastNonAlloc((Vector2)transform.position + bounceCollider.offset, bounceCollider.size, 0, Vector2.zero, hits, 0, layerBounce);
        
        for (int i = 0; i < hits.Length; i++)
        {
            LG_Movable _movable;
            if (hits[i] && hits[i].transform != this.transform && hits[i].transform.TryGetComponent(out _movable))
            {
                _movable.SetPosition(_movable.transform.position + Vector3.up * .5f);
                _movable.TrampolineJump(Vector2.up * trampolineForce);
            }
        }

    }
}
