using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WSB_ElevatorCam : MonoBehaviour
{

    private short playersIn = 0;

    public bool CanBeActivated = false;

    [SerializeField] private Animator elevatorAnimator = null;

    private static readonly int cam_Hash = Animator.StringToHash("Cam");


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (CanBeActivated && collision.GetComponent<WSB_PlayerMovable>())
        {
            playersIn++;
            if(playersIn == 2)
                ToggleCam();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(CanBeActivated && collision.GetComponent<WSB_PlayerMovable>())
        {
            playersIn--;

            if (playersIn < 0)
                playersIn = 0;
        }
    }

    private void ToggleCam() => elevatorAnimator.SetTrigger(cam_Hash);

    public void Activate(bool _s)
    {
        CanBeActivated = _s;
        if (_s)
            playersIn = 0;
    }
}
