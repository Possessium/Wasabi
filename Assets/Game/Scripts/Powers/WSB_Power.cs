using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WSB_Power : MonoBehaviour
{
    [SerializeField] Animator animator = null;
    [SerializeField] protected float range = 2;
    public bool IsActive = true;
    public WSB_PlayerMovable Owner { get; private set; } = null;
    [SerializeField] protected LG_Movable movable = null; 

    private void Start()
    {
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
        Owner = null;

        movable.MovableCollider.size *= 2;

        if (animator)
            animator.SetTrigger("Grow");
    }

    public void DeactivatePower(WSB_PlayerMovable _p)
    {
        IsActive = false;
        Owner = _p;

        movable.MovableCollider.size /= 2;

        if (animator)
            animator.SetTrigger("Shrink");
    }

    internal void Lock(bool v)
    {
        movable.CanMove = v;
    }
}
