using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WSB_Wind : WSB_Power
{
    [SerializeField] float windPower = 2;
    [SerializeField] LayerMask windLayer = 0;
    [SerializeField] Vector2 size = Vector2.one;

    Collider2D hit = null;
    RaycastHit2D[] checkPlayerOn = new RaycastHit2D[10];
    LG_Movable physics;
    [SerializeField] LayerMask stopWindSight = 0;

    WSB_Ban ban = null;
    WSB_Lux lux = null;


    private void Start()
    {
        ban = FindObjectOfType<WSB_Ban>();
        lux = FindObjectOfType<WSB_Lux>();
    }

    protected override void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 2, .3f, .6f);
        Gizmos.DrawWireCube(transform.position + new Vector3(0, size.y/2), size);

    }

    private void Update()
    {
        // Hold if game is in pause
        if (WSB_GameManager.Paused || !ban || !lux)
            return;

        // Find all corresponding objects in range
        Collider2D[] _hits = Physics2D.OverlapBoxAll(transform.position + new Vector3(0, size.y / 2), size, 0, windLayer);

        // Loops through found objects
        for (int i = 0; i < _hits.Length; i++)
        {
            hit = _hits[i];
            // Check if hit is Ban or Lux
            if (hit == ban.Player.MovableCollider || hit == lux.PlayerMovable.MovableCollider || hit == movable.MovableCollider)
                continue;

            // Looks if there is a wall between the power and the object and stop if yes
            Vector2 _dir = hit.transform.position - transform.position;
            RaycastHit2D _fion;
            if (_fion = Physics2D.Raycast(transform.position, _dir, Vector2.Distance(transform.position, hit.transform.position), stopWindSight))
            {
                Debug.LogError(_fion.transform.name);
                continue;
            }

            // Gets physic of hit object
            if (hit.gameObject.TryGetComponent(out physics))
            {
                // Add vertical force on the physic of the object
                physics.AddForce(Vector2.up * windPower);
            }
        }

    }
}
