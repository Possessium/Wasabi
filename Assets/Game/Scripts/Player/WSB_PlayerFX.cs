using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WSB_PlayerFX : MonoBehaviour
{
    [SerializeField] private GameObject stepFX = null;

    [SerializeField] private WSB_PlayerMovable playerMovable = null;
    [SerializeField] private LayerMask groundLayer = 0;

    public void PlayStepFX()
    {
        if (playerMovable.IsGrounded && playerMovable.CanMove && stepFX)
        {
            ParticleSystem _particle = Instantiate(stepFX, transform.position, Quaternion.identity).GetComponent<ParticleSystem>();
            _particle.transform.position += Vector3.up * .1f;
            _particle.transform.localEulerAngles = new Vector3(0, playerMovable.IsRight ? 0 : 180, 0);
            _particle.GetComponent<ParticleSystemRenderer>().flip = new Vector3(playerMovable.IsRight ? 0 : 1, 0, 0);
            _particle.transform.localScale = playerMovable.transform.localScale;

            string _s = Physics2D.Raycast(transform.position, Vector2.down, 2, groundLayer).transform?.tag;

            if (_s == "METAL" && playerMovable.PlayerAnimator.GetBool("IsWalking"))
                WSB_SoundManager.I.Walk(playerMovable.GetComponent<WSB_Ban>(), GroundType.Metal);
            else if (_s == "METAL" && !playerMovable.PlayerAnimator.GetBool("IsWalking"))
                WSB_SoundManager.I.Run(playerMovable.GetComponent<WSB_Ban>(), GroundType.Metal);

            else if (_s == "GRASS" && playerMovable.PlayerAnimator.GetBool("IsWalking"))
                WSB_SoundManager.I.Walk(playerMovable.GetComponent<WSB_Ban>(), GroundType.Grass);
            else if (_s == "GRASS" && !playerMovable.PlayerAnimator.GetBool("IsWalking"))
                WSB_SoundManager.I.Run(playerMovable.GetComponent<WSB_Ban>(), GroundType.Grass);

            else if (_s == "WOOD" && playerMovable.PlayerAnimator.GetBool("IsWalking"))
                WSB_SoundManager.I.Walk(playerMovable.GetComponent<WSB_Ban>(), GroundType.Wood);
            else if (_s == "WOOD" && !playerMovable.PlayerAnimator.GetBool("IsWalking"))
                WSB_SoundManager.I.Run(playerMovable.GetComponent<WSB_Ban>(), GroundType.Wood);
        }
    }

}
