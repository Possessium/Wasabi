using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WSB_ElevatorLock : MonoBehaviour
{
    private bool hasLux = false;
    private bool hasBan = false;

    [SerializeField] private GameObject elevatorLock = null;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<WSB_Ban>())
            hasBan = true;
        if (collision.GetComponent<WSB_Lux>())
            hasLux = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<WSB_Ban>())
            hasBan = false;
        if (collision.GetComponent<WSB_Lux>())
            hasLux = false;
    }

    private void Update()
    {
        if(hasLux && hasBan)
        {
            elevatorLock.SetActive(true);
            Destroy(this.gameObject);
        }
    }
}
