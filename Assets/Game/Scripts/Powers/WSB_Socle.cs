using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WSB_Socle : MonoBehaviour
{
    [SerializeField] private bool isEndSocle = false;

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

    private void Start()
    {
        if(isEndSocle && WSB_Elevator.I)
        {
            onActivate.AddListener(WSB_Elevator.I.ActivateTrigger);
            onDeactivate.AddListener(WSB_Elevator.I.DisableTrigger);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (currentHeldPower)
            return;

        WSB_Power _buffer = null;

        if(collision.TryGetComponent(out _buffer))
        {
            switch (soclePower)
            {
                case Power.Shrink:
                    if (_buffer is WSB_Shrink)
                        ActivateSocle(_buffer);
                    break;
                case Power.Wind:
                    if (_buffer is WSB_Wind)
                        ActivateSocle(_buffer);
                    break;
                case Power.Dragon:
                    if (_buffer is WSB_Carnivore)
                        ActivateSocle(_buffer);
                    break;
                case Power.Trampoline:
                    if (_buffer is WSB_Trampoline)
                        ActivateSocle(_buffer);
                    break;
            }
        }
    }

    float zPowerPosition = 0;

    void ActivateSocle(WSB_Power _power)
    {
        onActivate?.Invoke();
        zPowerPosition = _power.transform.position.z;
        _power.transform.position = new Vector3(transform.position.x + position.x, transform.position.y + position.y, transform.position.z);
        currentHeldPower = _power;
        currentHeldPower.Lock(true);
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!currentHeldPower)
            return;

        if (currentHeldPower == collision.GetComponent<WSB_Power>())
        {
            currentHeldPower.transform.position = new Vector3(currentHeldPower.transform.position.x, currentHeldPower.transform.position.y, zPowerPosition);
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
