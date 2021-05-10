using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WSB_Carnivore : WSB_Power
{
    [SerializeField] float eatDelay = 3;
    [SerializeField] LayerMask eatLayer = 0;
    bool isEating = false;

    private void Update()
    {
        if (!IsActive || isEating)
            return;

        Collider2D[] _hits = Physics2D.OverlapCircleAll(transform.position, range, eatLayer);
        // If found any, eat them
        if (_hits.Length > 0)
        {
            Destroy(_hits[0]);
            StartCoroutine(Eat());
        }
    }

    IEnumerator Eat()
    {
        isEating = true;

        // Wait for given delay
        yield return new WaitForSeconds(eatDelay);

        isEating = false;
    }
}
