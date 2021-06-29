using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WSB_EndGame : MonoBehaviour
{
    private int passed = 0;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<WSB_PlayerMovable>())
        {
            collision.GetComponent<WSB_PlayerMovable>().EndGame();
            passed++;
            if (passed >= 2)
                WSB_GameManager.I.EndGame();
        }
    }
}
