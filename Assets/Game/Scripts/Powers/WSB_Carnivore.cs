using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WSB_Carnivore : WSB_Plant
{
    [SerializeField] float eatDelay = 3;
    [SerializeField] LayerMask eatLayer = 0;
    bool isEating = false;


    private static readonly int fire_Hash = Animator.StringToHash("Fire");

    IEnumerator Eat(GameObject _hit)
    {
        isEating = true;

        if (animator)
            animator.SetTrigger(fire_Hash);

        yield return new WaitForSeconds(.5f);

        Destroy(_hit.gameObject);
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
}
