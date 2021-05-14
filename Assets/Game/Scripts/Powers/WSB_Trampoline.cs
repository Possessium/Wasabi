﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WSB_Trampoline : WSB_Plant
{
    [SerializeField] float trampolineForce = 10;
    [SerializeField] BoxCollider2D bounceCollider = null;

    protected override void OnDrawGizmos()
    {
        // don't show range on that plant
    }

    [SerializeField] ContactFilter2D bounceFilter;
    RaycastHit2D[] hits = new RaycastHit2D[2];

    protected override void PlayPower()
    {
        System.Array.Clear(hits, 0, 2);

        bounceCollider.Cast(Vector2.up, bounceFilter, hits, .5f);

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
