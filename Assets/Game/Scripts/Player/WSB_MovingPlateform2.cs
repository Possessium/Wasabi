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
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.GetComponent<LG_Movable>())
        {
            if(collision.transform.GetComponent<LG_Movable>().IsOnMovingPlateform && collision.transform.parent != WSB_Ban.I.transform && collision.transform.parent != WSB_Lux.I.transform)
                collision.transform.GetComponent<LG_Movable>().SetOnMovingPlateform(false, col);
        }
    }
}
