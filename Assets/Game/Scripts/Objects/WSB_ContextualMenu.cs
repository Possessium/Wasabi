using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WSB_ContextualMenu : MonoBehaviour
{

    [SerializeField] GameObject toShow = null;
    int playersIn = 0;
    [SerializeField] private bool isLux = false;
    [SerializeField] private bool isBan = false;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isLux && collision.GetComponent<WSB_Lux>())
        {
            // Increase the stocked number of players in the trigger
            playersIn++;

            // Activate the thing to show
            toShow.SetActive(true);
        }

        if (isBan && collision.GetComponent<WSB_Ban>())
        {
            // Increase the stocked number of players in the trigger
            playersIn++;

            // Activate the thing to show
            toShow.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (isLux && collision.GetComponent<WSB_Lux>())
        {
            // Decrease the stocked number of players in the trigger
            playersIn--;
        }
        if (isBan && collision.GetComponent<WSB_Ban>())
        {
            // Decrease the stocked number of players in the trigger
            playersIn--;
        }

        // If there is no players left in the trigger, disable the thing to show
        if (playersIn == 0)
            toShow.SetActive(false);
    }

    public void Disable()
    {
        toShow.SetActive(false);
    }

}
