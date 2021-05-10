using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class WSB_PlayerMovable : LG_Movable
{
    float jumpOriginHeight = 0;
    public bool Turning = false;
    public override void Update()
    {
        if (WSB_GameManager.Paused)
            return;

        if (CanMove)
        {

            if (IsGrounded)
            {
                jumpVar = force.y = 0;
                jumpOriginHeight = transform.position.y;
                if (jumpInput && CanMove)
                    if (IsGrounded || (Time.time - coyoteVar < controllerValues.JumpDelay))
                        Jump();
            }
            if (playerAnimator)
            {
                if (Keyboard.current.yKey.isPressed)
                {
                    playerAnimator.SetTrigger("Key");
                    IsRight = true;
                    rend.transform.eulerAngles = new Vector3(rend.transform.eulerAngles.x, 90, rend.transform.eulerAngles.z);
                    CanMove = false;
                }

                playerAnimator.SetFloat("Run", speed / movableValues.SpeedCurve.Evaluate(movableValues.SpeedCurve[movableValues.SpeedCurve.length - 1].time) * (IsRight ? 1 : -1));

                playerAnimator.SetBool("Jump", isJumping);

                playerAnimator.SetBool("Grounded",IsGrounded ? true : (IsOnMovingPlateform && !isJumping) ? true : false);

                if (CanMove)
                {
                    if (xMovement < 0 && IsRight)
                    {
                        IsRight = false;
                        if (IsGrounded)
                        {
                            playerAnimator.SetBool("Turning", true);
                            playerAnimator.SetTrigger("Rotate");
                        }
                        else
                            rend.transform.eulerAngles = new Vector3(rend.transform.eulerAngles.x, -90, rend.transform.eulerAngles.z);
                    }

                    if (xMovement > 0 && !IsRight)
                    {
                        IsRight = true;
                        if (IsGrounded)
                        {
                            playerAnimator.SetBool("Turning", true);
                            playerAnimator.SetTrigger("Rotate");
                        }
                        else
                            rend.transform.eulerAngles = new Vector3(rend.transform.eulerAngles.x, 90, rend.transform.eulerAngles.z);
                    }

                    if(playerAnimator.GetBool("Turning"))
                    {
                        rend.transform.eulerAngles = new Vector3(rend.transform.eulerAngles.x, IsRight ? -90 : 90, rend.transform.eulerAngles.z);
                    }
                }
            }
            if (isJumping)
            {
                RaycastHit2D[] _hits = new RaycastHit2D[5];
                bool _jump = true;
                if(MovableCollider.Cast(Vector2.down, _hits, .5f) > 0)
                {
                    for (int i = 0; i < _hits.Length; i++)
                    {
                        if (_hits[i] && _hits[i].transform.GetComponent<LG_Movable>() && !_hits[i].transform.GetComponent<LG_Movable>().IsGrounded)
                            _jump = false;
                    }
                }

                if(_jump)
                {
                    // Stop the jump if input is released & peak of jump icn't reached yet
                    if (!jumpInput && jumpVar < .3f)
                    {
                        jumpOriginHeight -= (controllerValues.JumpCurve.Evaluate(.3f) - controllerValues.JumpCurve.Evaluate(jumpVar));
                        jumpVar = .3f;
                    }
                    // Get the value corresponding to the curve set
                    else
                    {
                        jumpVar += Time.deltaTime;

                        // Stop jump if peak reached
                        if (jumpVar > controllerValues.JumpCurve[controllerValues.JumpCurve.length - 1].time)
                        {
                            jumpVar = controllerValues.JumpCurve[controllerValues.JumpCurve.length - 1].time;
                            isJumping = false;
                        }

                        MoveVertically((jumpOriginHeight + (controllerValues.JumpCurve.Evaluate(jumpVar)) - MovableRigidbody.position.y) / Time.deltaTime);
                    }
                }
            }

            if (xMovement != 0)
                IsRight = xMovement > 0;

        }

        base.Update();
    }

    
    /*[SerializeField] */
    public float xMovement = 0;
    /*[SerializeField] */public float yMovement = 0;
    /*[SerializeField] */bool jumpInput = false;
    bool down = false;
    public SO_ControllerValues controllerValues = null;
    public GameObject rend = null;
    [SerializeField] protected Animator playerAnimator = null;
    /*[SerializeField]*/ public bool IsRight = true;
    // Reads x & y movement and sets it in xMovement & yMovement
    public void Move(InputAction.CallbackContext _context)
    {
        if (_context.valueType != typeof(Vector2) || !CanMove) return;
        xMovement = _context.ReadValue<Vector2>().x;
        yMovement = _context.ReadValue<Vector2>().y;
        down = _context.ReadValue<Vector2>().y < 0;
    }

    // Reads jump input and sets it in jumpInput
    public void Jump(InputAction.CallbackContext _context)
    {
        if (_context.valueType == typeof(float))
            jumpInput = _context.ReadValue<float>() == 1;
    }

    
    float coyoteVar = -999;

    // Makes the character jump
    void Jump()
    {
        // Checks if input was in direction of the ground
        if (down)
        {
            // Cast below character to found if there is any SemiSolid plateform
            RaycastHit2D[] _hits = new RaycastHit2D[1];
            if (MovableCollider.Cast(Vector3.down, movableValues.SemisolidFilter, _hits) > 0)
            {

                // If found set collider in ignoredCollider and don't do the jump
                semiSolidCollider = _hits[0].collider;
                dontResetSemiSolid = true;
                return;
            }
        }

        dontResetSemiSolid = false;

        isJumping = true;

        jumpVar = force.y = 0;

        // Set originHeight for jump curve calculs
        jumpOriginHeight = transform.position.y;

        // Reset coyoteVar to unobtainable number
        coyoteVar = -999;
    }
    public void StopJump() => isJumping = false;

    protected override void OnSetGrounded()
    {
        base.OnSetGrounded();

        if(IsGrounded)
            isJumping = false;
    }
}
