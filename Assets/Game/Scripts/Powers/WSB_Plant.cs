using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WSB_Plant : WSB_Power
{
    private static readonly int active_Hash = Animator.StringToHash("Active");
    private static readonly int ping_Hash = Animator.StringToHash("Ping");

    abstract protected override void PlayPower();

    public override void ActivatePower()
    {
        base.ActivatePower();

        if (animator)
        {
            animator.SetBool(active_Hash, true);
            animator.SetTrigger(ping_Hash);
            AkSoundEngine.PostEvent("Play_Plant_Grow", gameObject);
        }
    }

    public override void DeactivatePower(WSB_PlayerMovable _p)
    {
        base.DeactivatePower(_p);

        if (animator)
        {
            animator.SetBool(active_Hash, false);
            animator.SetTrigger(ping_Hash);
            AkSoundEngine.PostEvent("Play_Plant_Retract", gameObject);
        }
    }
}
