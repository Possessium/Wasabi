using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WSB_Carnivore : WSB_Plant
{
    [SerializeField] float eatDelay = 3;
    [SerializeField] LayerMask eatLayer = 0;
    bool isEating = false;

    IEnumerator Eat()
    {
        isEating = true;

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
            Destroy(_hits[0].gameObject);
            StartCoroutine(Eat());
        }
    }
}
