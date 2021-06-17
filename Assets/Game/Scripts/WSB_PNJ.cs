using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WSB_PNJ : MonoBehaviour
{
    [SerializeField] InputActionAsset actionBan = null;
    [SerializeField] InputActionAsset actionLux = null;

    [SerializeField] WSB_Dialogue dialogue = null;

    bool banIn = false;
    bool luxIn = false;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<WSB_Ban>())
        {
            // Stocks ban entered the trigger of the PNJ
            banIn = true;

            // Bind StartDialogue method on ban's Use action
            actionBan.FindAction("Interact").performed += StartDialogue;
        }
        if (collision.GetComponent<WSB_Lux>())
        {
            // Stocks lux entered the trigger of the PNJ
            luxIn = true;

            // Bind StartDialogue method on lux's Use action
            actionLux.FindAction("Interact").performed += StartDialogue;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<WSB_Ban>())
        {
            // Stocks ban left the trigger of the PNJ
            banIn = false;

            // Unbind StartDialogue method of ban's Use action
            actionBan.FindAction("Interact").performed -= StartDialogue;
        }
        if (collision.GetComponent<WSB_Lux>())
        {
            // Stocks lux left the trigger of the PNJ
            luxIn = false;

            // Unbind StartDialogue method of lux's Use action
            actionLux.FindAction("Interact").performed -= StartDialogue;
        }

        // If both Lux and Ban aren't in the PNJ trigger disable the dialogue and tells the gamemanager
        if (!banIn && !luxIn)
        {
            dialogue.gameObject.SetActive(false);
            WSB_GameManager.SetDialogue(false);
        }
    }

    void StartDialogue(InputAction.CallbackContext obj)
    {
        // Exit if game paused or dialogue isn't set
        if (!dialogue || WSB_GameManager.Paused)
            return;

        // If the dialogue is already playing, skips to the next line or dialogue
        if (dialogue.gameObject.activeSelf)
            dialogue.Skip(obj);

        // If not, start a new dialogue
        else if(!WSB_GameManager.IsDialogue)
        {
            WSB_GameManager.SetDialogue(true);
            dialogue.gameObject.SetActive(true);
        }
    }

}
