using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WSB_KeyObject : MonoBehaviour
{
    [SerializeField] private ParticleSystem confettis = null;
    [SerializeField] private Animator keyAnimator = null;
    [SerializeField] private Vector2 position = Vector2.zero;

    private static readonly int hide_Hash = Animator.StringToHash("Hide");

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere((Vector2)transform.position + position, .2f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<WSB_PlayerInteraction>() && !collision.GetComponent<WSB_PlayerInteraction>().HeldObject)
        {
            keyAnimator.SetTrigger(hide_Hash);
            collision.transform.position = transform.position + (Vector3)position;
            collision.GetComponent<WSB_PlayerInteraction>().AnimateKey();
            confettis.Play();
            Destroy(GetComponent<BoxCollider2D>());
            Destroy(this);
        }
    }
}
