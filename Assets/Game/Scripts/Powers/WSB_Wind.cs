using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WSB_Wind : WSB_Rune
{
    [SerializeField] float windPower = 2;
    [SerializeField] LayerMask windLayer = 0;
    [SerializeField] Vector2 size = Vector2.one;
    [SerializeField] private GameObject poufAigretteFX = null;
    [SerializeField] LayerMask stopWindSight = 0;

    WSB_Ban ban = null;
    WSB_Lux lux = null;


    private void Start()
    {
        ban = WSB_Ban.I;
        lux = WSB_Lux.I;
    }

    protected override void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 2, .3f, .6f);
        Gizmos.DrawWireCube(transform.position + new Vector3(0, size.y/2), size);
    }

    protected override void PlayPower()
    {
        // Hold if game is in pause
        if (WSB_GameManager.Paused || !ban || !lux)
            return;

        // Find all corresponding objects in range
        Collider2D[] _hits = Physics2D.OverlapBoxAll(transform.position + new Vector3(0, size.y / 2), size, 0, windLayer);

        Collider2D _hit = null;

        
        // Loops through found objects
        for (int i = 0; i < _hits.Length; i++)
        {
            _hit = _hits[i];
            // Check if hit is Ban or Lux
            if (_hit == ban.Player.MovableCollider || _hit == lux.PlayerMovable.MovableCollider || _hit == movable.MovableCollider)
                continue;

            // Looks if there is a wall between the power and the object and stop if yes
            Vector2 _dir = _hit.transform.position - transform.position;
            RaycastHit2D _fion;
            if (_fion = Physics2D.Raycast(transform.position, _dir, Vector2.Distance(transform.position, _hit.transform.position), stopWindSight))
            {
                Debug.LogError(_fion.transform.name);
                continue;
            }

            LG_Movable _physics;
            // Gets physic of hit object
            if (_hit.gameObject.TryGetComponent(out _physics))
            {
                // Add vertical force on the physic of the object
                _physics.AddForce(Vector2.up * windPower);
            }
        }
    }

    public override void ActivatePower()
    {
        base.ActivatePower();

        WSB_SoundManager.I.WindActive(transform);
    }

    public override void DeactivatePower(WSB_PlayerMovable _p)
    {
        base.DeactivatePower(_p);

        if (poufAigretteFX)
            Instantiate(poufAigretteFX, transform.position, Quaternion.identity);

        WSB_SoundManager.I.WindDisable(transform);        
    }
}
