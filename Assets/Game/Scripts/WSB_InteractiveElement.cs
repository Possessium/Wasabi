using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WSB_InteractiveElement : MonoBehaviour
{
    [SerializeField] List<Animator> animators = new List<Animator>();
    [SerializeField] bool isOneTimeUse = false;
    [SerializeField] bool isAnimationPlayOnTrigger = false;

    private static readonly int on_Hash = Animator.StringToHash("On");
    private static readonly int activate_Hash = Animator.StringToHash("Activate");

    bool on = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isAnimationPlayOnTrigger)
            ActivateAnimators();
    }

    public void ActivateAnimators()
    {
        on = !on;
        for (int i = 0; i < animators.Count; i++)
        {
            animators[i].SetBool(on_Hash, on);
            animators[i].SetTrigger(activate_Hash);
        }
        if (isOneTimeUse)
            Destroy(this);
    }
}
