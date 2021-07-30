using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WSB_Lux : MonoBehaviour
{
    public static WSB_Lux I { get; private set; }

    [SerializeField] ContactFilter2D shrinkLayer;
    public bool Shrinked { get; private set; } = false;

    [SerializeField] WSB_PlayerMovable playerMovable = null;
    public WSB_PlayerMovable PlayerMovable { get { return playerMovable; } }
    [SerializeField] WSB_PlayerInteraction playerInteraction = null;
    public WSB_PlayerInteraction PlayerInteraction { get { return playerInteraction; } }

    [SerializeField] private Animator shrinkAnimator = null;
    private static readonly int shrink_Hash = Animator.StringToHash("Shrinked");


    private void Awake()
    {
        I = this;
    }


    public void Shrink()
    {
        playerMovable.RemoveSpeedCoef(1.5f);
        shrinkAnimator.SetBool(shrink_Hash, true);

        WSB_SoundManager.I.ShrinkLux();

        Shrinked = true;
    }

    public void Unshrink()
    {
        playerMovable.AddSpeedCoef(1.5f);
        shrinkAnimator.SetBool(shrink_Hash, false);

        WSB_SoundManager.I.UnshrinkLux();

        Shrinked = false;
    }
}
