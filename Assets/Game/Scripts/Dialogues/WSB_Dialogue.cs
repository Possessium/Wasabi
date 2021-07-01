using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WSB_Dialogue : MonoBehaviour
{
    [SerializeField] private bool isDelay = false;
    [SerializeField] private float delay = 2;

    [SerializeField] private bool luxActivate = true;
    [SerializeField] private bool banActivate = true;
    private bool luxIn = false;
    private bool banIn = false;

    [SerializeField] private string text = "";
    [SerializeField] private TMP_Text tmpText = null;
    [SerializeField] private GameObject dialogue = null;
    [SerializeField] private float letterDelay = .01f;

    private Coroutine dialogueCoroutine = null;
    private int charPosition = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (luxActivate && !luxIn && collision.GetComponent<WSB_Lux>())
        {
            luxIn = true;
            ShowDialogue();
        }
        if (banActivate && !banIn && collision.GetComponent<WSB_Ban>())
        {
            banIn = true;
            ShowDialogue();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!isDelay && luxIn && collision.GetComponent<WSB_Lux>())
        {
            luxIn = false;
            HideDialogue();
        }
        if (!isDelay && banIn && collision.GetComponent<WSB_Ban>())
        {
            banIn = false;
            HideDialogue();
        }
    }

    private void ShowDialogue()
    {
        charPosition = tmpText.maxVisibleCharacters = 0;
        tmpText.text = text;
        dialogue.SetActive(true);
        
        if (dialogueCoroutine != null)
            StopCoroutine(dialogueCoroutine);
        dialogueCoroutine = StartCoroutine(Dialogue());

        if (isDelay)
            StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(delay);
        HideDialogue();
    }

    IEnumerator Dialogue()
    {
        while(charPosition <= tmpText.text.Length)
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

        if (isDelay)
            Destroy(this);
    }
}
