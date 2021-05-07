using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WSB_Socle : MonoBehaviour
{
    [SerializeField] Power soclePower = Power.Shrink;
    WSB_Power currentHeldPower = null;

    [SerializeField] Vector2 position = Vector2.zero;

    [SerializeField] UnityEngine.Events.UnityEvent onActivate = null;
    [SerializeField] UnityEngine.Events.UnityEvent onDeactivate = null;


    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(255, 120, 0, 1);
        Gizmos.DrawSphere(new Vector3(transform.position.x + position.x, transform.position.y + position.y, -5), .1f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (currentHeldPower)
            return;

        WSB_Power _buffer = null;

        if(collision.TryGetComponent(out _buffer))
        {
            if ((collision.GetComponent<WSB_Shrink>() && soclePower == Power.Shrink) ||
                (collision.GetComponent<WSB_Wind>() && soclePower == Power.Wind) ||
                (collision.GetComponent<WSB_Carnivore>() && soclePower == Power.Dragon) ||
                (collision.GetComponent<WSB_Trampoline>() && soclePower == Power.Trampoline))
            {
                onActivate?.Invoke();
                collision.transform.position = new Vector3(transform.position.x + position.x, transform.position.y + position.y, collision.transform.position.z);
                currentHeldPower = _buffer;
                currentHeldPower.Lock(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!currentHeldPower)
            return;

        if (currentHeldPower == collision.GetComponent<WSB_Power>())
        {
            if(!currentHeldPower.Owner)
                currentHeldPower.Lock(false);
            currentHeldPower = null;
            onDeactivate?.Invoke();
        }
    }
}

public enum Power
{
    Shrink,
    Wind,
    Dragon,
    Trampoline
}
