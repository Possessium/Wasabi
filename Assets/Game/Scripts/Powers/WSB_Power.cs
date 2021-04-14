using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WSB_Power : LG_Movable
{
    [SerializeField] Animator animator = null;
    [SerializeField] protected float range = 2;
    public bool IsActive = true;
    [SerializeField] WSB_Player owner = null;

    public override void Start()
    {
        base.Start();

        if (!animator)
            TryGetComponent(out animator);

    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 2, .3f, .6f);
        Gizmos.DrawWireSphere((Vector2)transform.position, range);
    }


    public void ActivatePower()
    {
        IsActive = true;
        owner = null;

        collider.size = collider.size * 2;

        if (animator)
            animator.SetTrigger("Grow");
    }

    public void DeactivatePower(WSB_Player _p)
    {
        IsActive = false;
        owner = _p;

        collider.size = collider.size / 2;

        if (animator)
            animator.SetTrigger("Shrink");
    }
}
