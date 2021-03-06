using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WSB_MovingPlateform2 : MonoBehaviour
{
    [SerializeField] Collider2D col = null;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.GetComponent<LG_Movable>() && collision.bounds.center.y - .25f > col.bounds.max.y)
        {
            collision.transform.GetComponent<LG_Movable>().SetOnMovingPlateform(true, col);
            collision.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.GetComponent<LG_Movable>())
        {
            collision.transform.GetComponent<LG_Movable>().SetOnMovingPlateform(false, col);
            collision.transform.SetParent(null);
        }
    }
}
