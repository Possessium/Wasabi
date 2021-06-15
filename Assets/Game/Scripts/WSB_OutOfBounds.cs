using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WSB_OutOfBounds : MonoBehaviour
{
    private void Awake()
    {
        // Gives a box collider 2D if missing
        if (!GetComponent<BoxCollider2D>())
            gameObject.AddComponent<BoxCollider2D>();

        // Set it to trigger in case it is not
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Respawn any player that would've got out of bounds
        if(collision.GetComponent<WSB_PlayerMovable>())
            WSB_CheckpointManager.I.Respawn(collision.GetComponent<WSB_PlayerMovable>());

        // Destroy any object that would fall out of bounds to avoid lag
        else
        {
            Debug.Log(collision.name + " Has been destroyed due to OUT OF BOUNDS.");
            Destroy(collision.gameObject);
        }
    }
}
