using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WSB_Rune : WSB_Power
{
    [SerializeField] private Animator powerAnimatorFX = null;

    // PLAYTEST ONLY
    [SerializeField] GameObject shrinkFX = null;
    //

    private static readonly int activate_Hash = Animator.StringToHash("GoForIt");

    abstract protected override void PlayPower();

    public override void ActivatePower()
    {
        base.ActivatePower();

        if (powerAnimatorFX)
        {

            powerAnimatorFX.SetBool(activate_Hash, false);
        }

        // PLAYTEST ONLY
        if (shrinkFX)
            shrinkFX.SetActive(true);
        //
    }

    public override void DeactivatePower(WSB_PlayerMovable _p)
    {
        base.DeactivatePower(_p);

        if (powerAnimatorFX)
            powerAnimatorFX.SetBool(activate_Hash, true);

        // PLAYTEST ONLY
        if (shrinkFX)
            shrinkFX.SetActive(false);
        //
    }
}
