using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WSB_Checkpoint : MonoBehaviour
{
    [SerializeField] Vector2 position = Vector2.zero;
    public Vector2 Position { get { return position; } }

    private void OnDrawGizmos()
    {
        // Only to set properly the spawnpoint of the players
        Gizmos.color = new Color(2, .3f, 0, .8f);
        Gizmos.DrawSphere(position, .3f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If a player enter the trigger, tells the manager this is the new checkpoint
        if(collision.GetComponent<WSB_PlayerMovable>())
        {
            WSB_CheckpointManager.I.SetNewCheckpoint(this, collision.GetComponent<WSB_Ban>());
        }
    }
}
