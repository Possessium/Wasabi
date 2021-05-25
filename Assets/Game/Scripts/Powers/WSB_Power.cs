using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WSB_Power : MonoBehaviour
{
    [SerializeField] Animator animator = null;
    [SerializeField] protected float range = 2;

    [SerializeField] ParticleSystem insectFX = null;
    [SerializeField] ParticleSystem dropFX = null;

    public bool IsActive = true;
    public WSB_PlayerMovable Owner { get; private set; } = null;
    [SerializeField] protected LG_Movable movable = null;

    private static readonly int grow_Hash = Animator.StringToHash("Grow");
    private static readonly int shrink_Hash = Animator.StringToHash("Shrink");

    private void Start()
    {
        if (!animator)
            TryGetComponent(out animator);
    }

    private void Update()
    {
        if (IsActive)
        {
            PlayPower();
            transform.position = new Vector3(transform.position.x, transform.position.y, 2);
        }

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

        if (dropFX)
            dropFX.Play();

        if (animator)
            animator.SetTrigger(grow_Hash);
    }

    public virtual void DeactivatePower(WSB_PlayerMovable _p)
    {
        IsActive = false;
        Owner = _p;

        movable.MovableCollider.size /= 2;

        if (animator)
            animator.SetTrigger(shrink_Hash);

        if (insectFX)
            insectFX.Play();
    }

    abstract protected void PlayPower();
    internal void Lock(bool v)
    {
        movable.CanMove = v;
    }
}
