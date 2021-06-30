using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WSB_Dialogue : MonoBehaviour
{
    private int playersIn = 0;

    [SerializeField] private TMP_Text tmpText = null;
    [SerializeField] private GameObject dialogue = null;
    [SerializeField] private float letterDelay = .25f;

    private Coroutine dialogueCoroutine = null;
    private int charPosition = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<WSB_PlayerInteraction>())
        {
            playersIn++;
            if (playersIn == 1)
                ShowDialogue();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<WSB_PlayerInteraction>())
        {
            playersIn--;
            if (playersIn == 0)
                HideDialogue();
        }
    }

    private void ShowDialogue()
    {
        charPosition = tmpText.maxVisibleCharacters = 0;
        dialogue.SetActive(true);
        
        if (dialogueCoroutine != null)
            StopCoroutine(dialogueCoroutine);
        dialogueCoroutine = StartCoroutine(Dialogue());
    }

    IEnumerator Dialogue()
    {
        while(charPosition < tmpText.text.Length)
        {
            tmpText.maxVisibleCharacters = charPosition++;
            yield return new WaitForSeconds(letterDelay);
        }

        dialogueCoroutine = null;
    }

    private void HideDialogue()
    {
        if (dialogueCoroutine != null)
            StopCoroutine(dialogueCoroutine);

        dialogueCoroutine = null;

        charPosition = tmpText.maxVisibleCharacters = 0;
        dialogue.SetActive(false);
    }
}
