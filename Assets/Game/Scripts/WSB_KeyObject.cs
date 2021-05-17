using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WSB_KeyObject : MonoBehaviour
{
    [SerializeField] private ParticleSystem confettis = null;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<WSB_PlayerInteraction>())
        {
            collision.GetComponent<WSB_PlayerInteraction>().AnimateKey();
            confettis.Play();
            Destroy(GetComponent<BoxCollider2D>());
            Destroy(this);
        }
    }
}
