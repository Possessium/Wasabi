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

    private List<GameObject> instantiatedParticles = new List<GameObject>();

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position + spawnPosition, .2f);
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
        instantiatedParticles.Add(_p);
        yield return new WaitForSeconds(lifeTime);
        instantiatedParticles.Remove(_p);
        Destroy(_p.gameObject);
    }

    private void OnEnable()
    {
        StartCoroutine(Delay());
    }

    private void OnDisable()
    {
        foreach (GameObject _go in instantiatedParticles)
        {
            Destroy(_go);
        }
        instantiatedParticles.Clear();
    }
}