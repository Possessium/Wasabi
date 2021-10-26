using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WSB_Carnivore : WSB_Plant
{
    [SerializeField] float eatDelay = 3;
    [SerializeField] LayerMask eatLayer = 0;
    [SerializeField] private Animator fxFire = null;
    bool isEating = false;


    private static readonly int fire_Hash = Animator.StringToHash("Fire");
    private static readonly int goForIt_Hash = Animator.StringToHash("GoForIt");

    IEnumerator Eat(GameObject _hit)
    {
        isEating = true;
        fxFire.SetBool(goForIt_Hash, true);

        if (animator)
            animator.SetTrigger(fire_Hash);

        WSB_SoundManager.I.DragonEat(transform);

        yield return new WaitForSeconds(1.1f);

        Destroy(_hit.gameObject);

        yield return new WaitForSeconds(1.1f);

        fxFire.SetBool(goForIt_Hash, false);

        // Wait for given delay
        yield return new WaitForSeconds(eatDelay);

        isEating = false;
    }

    protected override void PlayPower()
    {
        if (!IsActive || isEating)
            return;

        Collider2D[] _hits = Physics2D.OverlapCircleAll(transform.position, range, eatLayer);
        // If found any, eat them
        if (_hits.Length > 0)
        {
            if (_hits[0].transform.position.x < transform.position.x)
                transform.eulerAngles = new Vector3(0, 0, 0);
            else
                transform.eulerAngles = new Vector3(0, 180, 0);

            StartCoroutine(Eat(_hits[0].gameObject));
        }
    }

    public override void ActivatePower()
    {
        base.ActivatePower();

        WSB_SoundManager.I.SpawnDragon(transform);
    }

    public override void DeactivatePower(WSB_PlayerMovable _p)
    {
        base.DeactivatePower(_p);

        WSB_SoundManager.I.DespawnDragon(transform);
    }
}
