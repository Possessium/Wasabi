using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WSB_Wind : WSB_Power
{
    [SerializeField] float windPower = 2;
    [SerializeField] LayerMask windLayer = 0;

    Collider2D hit = null;
    RaycastHit2D[] checkPlayerOn = new RaycastHit2D[10];
    LG_Movable physics;
    [SerializeField] LayerMask stopWindSight = 0;

    public override void Update()
    {
        base.Update();

        // Hold if game is in pause
        if (WSB_GameManager.Paused)
            return;

        // Find all corresponding objects in range
        Collider2D[] _hits = Physics2D.OverlapCircleAll(transform.position, range, windLayer);

        // Loops through found objects
        for (int i = 0; i < _hits.Length; i++)
        {
            hit = _hits[i];
            // Check if hit is Ban or Lux
            if (hit == WSB_Ban.I.MovableCollider || hit == WSB_Lux.I.MovableCollider || hit == collider)
                continue;

            // Check if Ban is on top of the moving object
            checkPlayerOn = new RaycastHit2D[10];
            WSB_Ban.I.MovableCollider.Cast(Vector2.down, checkPlayerOn, 1);
            if (System.Array.Find(checkPlayerOn, r => r.collider == hit))
                continue;

            // Check if Lux is on top of the moving object
            checkPlayerOn = new RaycastHit2D[10];
            WSB_Lux.I.MovableCollider.Cast(Vector2.down, checkPlayerOn, 1);
            if (System.Array.Find(checkPlayerOn, r => r.collider == hit))
                continue;

            // Looks if there is a wall between the power and the object and stop if yes
            Vector2 _dir = hit.transform.position - transform.position;
            if (Physics2D.Raycast(transform.position, _dir.normalized, Vector2.Distance(transform.position, hit.transform.position), stopWindSight))
                continue;

            // Gets physic of hit object
            hit.gameObject.TryGetComponent(out physics);

            if (physics)
            {
                // Add vertical force on the physic of the object
                physics.AddForce(Vector2.up * windPower);
            }
        }

    }
}
