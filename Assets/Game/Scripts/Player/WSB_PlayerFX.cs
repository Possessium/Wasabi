using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WSB_PlayerFX : MonoBehaviour
{
    [SerializeField] private GameObject stepFX = null;

    [SerializeField] private WSB_PlayerMovable playerMovable = null;

    public void PlayStepFX()
    {
        if (playerMovable.IsGrounded && playerMovable.CanMove && stepFX)
        {
            //AkSoundEngine.SetSwitch("FOOT_TYPE", "WALK", SwitchSound);
            ParticleSystem _particle = Instantiate(stepFX, transform.position, Quaternion.identity).GetComponent<ParticleSystem>();
            _particle.transform.position += Vector3.up * .1f;
            _particle.transform.localEulerAngles = new Vector3(0, playerMovable.IsRight ? 0 : 180, 0);
            _particle.GetComponent<ParticleSystemRenderer>().flip = new Vector3(playerMovable.IsRight ? 0 : 1, 0, 0);
            _particle.transform.localScale = playerMovable.transform.localScale;
        }
    }

}
