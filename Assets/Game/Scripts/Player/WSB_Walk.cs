using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WSB_Walk : MonoBehaviour
{
    [SerializeField] private bool walk = true;

    bool hasLux = false;
    bool hasBan = false;

    private static readonly int walk_Hash = Animator.StringToHash("Walk");
    private static readonly int unwalk_Hash = Animator.StringToHash("UnWalk");
    private static readonly int isWalking_Hash = Animator.StringToHash("IsWalking");

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out WSB_PlayerMovable _movable))
        {
            if(!hasBan && _movable.GetComponent<WSB_Ban>())
            {
                hasBan = true;

                _movable.PlayerAnimator.SetTrigger(walk ? walk_Hash : unwalk_Hash);
                _movable.PlayerAnimator.SetBool(isWalking_Hash, walk);
                
                if (walk)
                    _movable.AddSpeedCoef(.5f);
                else
                    _movable.RemoveSpeedCoef(.5f);
                
                if (hasLux)
                    Destroy(this.gameObject);
            }

            else if (!hasLux && _movable.GetComponent<WSB_Lux>())
            {
                hasLux = true;

                _movable.PlayerAnimator.SetTrigger(walk ? walk_Hash : unwalk_Hash);
                _movable.PlayerAnimator.SetBool(isWalking_Hash, walk);

                if (walk)
                    _movable.AddSpeedCoef(.5f);
                else
                    _movable.RemoveSpeedCoef(.5f);

                if (hasBan)
                    Destroy(this.gameObject);
            }
        }
    }
}
