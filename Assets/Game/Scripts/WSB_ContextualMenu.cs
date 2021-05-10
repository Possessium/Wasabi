using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WSB_ContextualMenu : MonoBehaviour
{

    [SerializeField] GameObject toShow = null;
    int playersIn = 0;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<WSB_PlayerMovable>())
        {
            // Increase the stocked number of players in the trigger
            playersIn++;

            // Activate the thing to show
            toShow.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<WSB_PlayerMovable>())
        {
            // Decrease the stocked number of players in the trigger
            playersIn--;

            // If there is no players left in the trigger, disable the thing to show
            if(playersIn == 0)
                toShow.SetActive(false);
        }
    }

}
