using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WSB_WindParticle : MonoBehaviour
{
    [SerializeField] private float minDelay = .5f;
    [SerializeField] private float maxDelay = 1.5f;
    [SerializeField] private GameObject particle = null;
    [SerializeField] private Vector3 spawnPosition = Vector3.zero;
    [SerializeField] private float lifeTime = 5f;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position + spawnPosition, .2f);
    }

    void Start()
    {
        StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
        while (true)
        {
            if (particle)
                StartCoroutine(DestroyParticle(Instantiate(particle, transform.position + spawnPosition, Quaternion.identity)));             
            yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));
        }
    }

    IEnumerator DestroyParticle(GameObject _p)
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(_p.gameObject);
    }
}