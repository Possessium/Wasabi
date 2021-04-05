using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WSB_Carnivore : WSB_Power
{
    [SerializeField] float eatDelay = 3;
    [SerializeField] LayerMask eatLayer = 0;
    bool isEating = false;

    public override void Start()
    {
        base.Start();

        // Start the main coroutine
        StartCoroutine(Check());
    }

    IEnumerator Check()
    {
        // Will loop until the object is disabled or destroyed
        while(true)
        {
            while(!isActive || !isActive)
            {
                yield return new WaitForSeconds(.5f);
            }

            // Buffer if the plant is eating something
            if (isEating)
            {
                yield return new WaitForEndOfFrame();
            }

            else
            {
                // Search for element around range to eat
                Collider2D[] _hits = Physics2D.OverlapCircleAll(transform.position, range, eatLayer);
                // If found any, eat them
                if (_hits.Length > 0)
                    StartCoroutine(Eat(_hits[0]));
                else
                    yield return new WaitForSeconds(2);
            }
        }
    }

    IEnumerator Eat(Collider2D _hit)
    {
        isEating = true;

        // Destroy next item
        Destroy(_hit.gameObject);

        // Wait for given delay
        yield return new WaitForSeconds(eatDelay);

        isEating = false;
    }
}
