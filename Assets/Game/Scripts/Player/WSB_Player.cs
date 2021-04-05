using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
//using Nowhere;

//[RequireComponent(typeof(Rigidbody2D)), RequireComponent(typeof(BoxCollider2D))]
public class WSB_Player : LG_Movable
{

    float jumpOriginHeight = 0;
    public bool Turning = false;
    public override void Update()
    {
        if (WSB_GameManager.Paused)
            return;

        if(CanMove)
        {
            bool _isBlocked = false;

            if (grabbedObject && ((!IsRight && xMovement <= 0) || (IsRight && xMovement >= 0)) )
            {
                Collider2D[] _hits = Physics2D.OverlapBoxAll((Vector2)grabbedObject.transform.position + grabbedObject.MovableCollider.offset, grabbedObject.MovableCollider.size * 1.1f, 0, controllerValues.ContactGrabLayer);

                for (int i = 0; i < _hits.Length; i++)
                {
                    if (_hits[i].transform != this.transform && _hits[i].transform != grabbedObject.transform)
                    {
                        _isBlocked = true;
                        break;
                    }

                }
                _hits = Physics2D.OverlapBoxAll((Vector2)grabbedObject.transform.position + grabbedObject.MovableCollider.offset, grabbedObject.MovableCollider.size, 0, controllerValues.ContactGrabLayer);
                for (int i = 0; i < _hits.Length; i++)
                {
                    if (_hits[i].transform != this.transform && _hits[i].transform != grabbedObject.transform)
                        rigidbody.position += (IsRight ? Vector2.left : Vector2.right) * .1f;
                }
            }

            if(!_isBlocked)
                MoveHorizontally(xMovement);

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

                playerAnimator.SetBool("Jump", isJumping);

                playerAnimator.SetBool("Grounded", IsGrounded ? true : (IsOnMovingPlateform && !isJumping) ? true : false);

                if (CanMove)
                {
                    if(xMovement < 0 /*&& rend.transform.rotation.y > 0*/ && IsRight)
                    {
                        IsRight = false;
                        if (isGrounded)
                        {
                            playerAnimator.SetBool("Turning", true);
                            playerAnimator.SetTrigger("Rotate");
                        }
                        else
                            rend.transform.eulerAngles = new Vector3(rend.transform.eulerAngles.x, -90, rend.transform.eulerAngles.z);
                    }

                    if (xMovement > 0 /*&& rend.transform.rotation.y < 0*/ && !IsRight)
                    {
                        IsRight = true;
                        if (isGrounded)
                        {
                            playerAnimator.SetBool("Turning", true);
                            playerAnimator.SetTrigger("Rotate");
                        }
                        else
                            rend.transform.eulerAngles = new Vector3(rend.transform.eulerAngles.x, 90, rend.transform.eulerAngles.z);
                    }

                    playerAnimator.SetFloat("Run", speed / movableValues.SpeedCurve.Evaluate(movableValues.SpeedCurve[movableValues.SpeedCurve.length - 1].time) * (IsRight ? 1 : -1));
                }
            }
            if (isJumping)
            {
                RaycastHit2D[] _hits = new RaycastHit2D[5];
                bool _jump = true;
                if(collider.Cast(Vector2.down, _hits, .5f) > 0)
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

                        MoveVertically((jumpOriginHeight + (controllerValues.JumpCurve.Evaluate(jumpVar)) - rigidbody.position.y) / Time.deltaTime);
                    }
                }
            }

            if (xMovement != 0)
                IsRight = xMovement > 0;

            if (grabbedObject)
                grabbedObject.transform.position = transform.position + Vector3.up * 1.5f + (IsRight ? Vector3.right : Vector3.left) * 1.5f;
        }
            

        base.Update();
    }

    bool canAnimateLever = false;
    bool canAnimateButton = false;
    bool isLeverRight = false;

    public void ToggleLever(bool _s, bool _r = false)
    {
        canAnimateLever = _s;
        isLeverRight = _r;
    }
    public void ToggleButton(bool _s)
    {
        canAnimateButton = _s;
    }

    public void AnimateLever(Vector2 _pos)
    {
        if (canAnimateLever && playerAnimator)
        {
            rend.transform.eulerAngles = new Vector3(rend.transform.eulerAngles.x, isLeverRight ? 90 : -90, rend.transform.eulerAngles.z);
            IsRight = isLeverRight = !isLeverRight;
            SetPosition(_pos);
            playerAnimator.SetTrigger("Lever");
            CanMove = false;
        }
    }

    public void AnimateButton(Vector2 _pos)
    {
        if (canAnimateButton && playerAnimator)
        {
            SetPosition(_pos);
            playerAnimator.SetTrigger("Button");
            CanMove = false;
        }
    }

    public void AnimationFinished() => CanMove = true;

    /*[SerializeField] */
    float xMovement = 0;
    /*[SerializeField] */float yMovement = 0;
    /*[SerializeField] */bool jumpInput = false;
    [SerializeField] SO_ControllerValues controllerValues = null;
    [SerializeField] GameObject rend = null;
    [SerializeField] protected Animator playerAnimator = null;
    /*[SerializeField]*/ public bool IsRight { get; protected set; } = true;

    // Reads x & y movement and sets it in xMovement & yMovement
    public void Move(InputAction.CallbackContext _context)
    {
        if (_context.valueType != typeof(Vector2) || !CanMove) return;
        xMovement = _context.ReadValue<Vector2>().x;
        yMovement = _context.ReadValue<Vector2>().y;
    }

    // Reads jump input and sets it in jumpInput
    public void Jump(InputAction.CallbackContext _context)
    {
        if (_context.valueType == typeof(float))
            jumpInput = _context.ReadValue<float>() == 1;
    }


    /*[SerializeField] */LG_Movable grabbedObject = null;
    [SerializeField] ContactFilter2D grabContactFilter = new ContactFilter2D();
    // Reads grab input and try to grab object
    public void GrabObject(InputAction.CallbackContext _context)
    {
        // Exit if not the beggining of the input
        if (!_context.started || (GetComponent<WSB_Lux>() && GetComponent<WSB_Lux>().Shrinked))
            return;


        // Drop object if already got one
        if (grabbedObject)
        {
            DropGrabbedObject();
            return;
        }

        RaycastHit2D[] _hit = new RaycastHit2D[1];

        // Cast on facing direction to check if there is an object
        if (collider.Cast(IsRight ? Vector2.right : Vector2.left, grabContactFilter, _hit, .5f) > 0)
        {
            if (_hit[0].transform.GetComponent<WSB_Movable>() && !_hit[0].transform.GetComponent<WSB_Movable>().CanMove)
                return;
            // Search for WSB_Pot component
            if (_hit[0].transform.GetComponent<WSB_Power>())

            // Sets grabbedObject var
            _hit[0].transform.TryGetComponent(out grabbedObject);

            grabbedObject.enabled = false;
            grabbedObject.GetComponent<WSB_Power>().DeactivatePower(this);
            grabbedObject.transform.parent = transform;
            grabbedObject.transform.position = transform.position + Vector3.up * 1.5f + (IsRight ? Vector3.right : Vector3.left) * 1.5f;

            grabbedObject.MovableCollider.enabled = false;

            if (playerAnimator)
                playerAnimator.SetBool("Grab", true);
        }
    }

    void DropGrabbedObject()
    {
        if (!grabbedObject)
            return;

        grabbedObject.transform.parent = transform.parent;

        grabbedObject.MovableCollider.enabled = true;
        grabbedObject.enabled = true;
        grabbedObject.GetComponent<WSB_Power>().ActivatePower();
        grabbedObject.transform.position = transform.position * 1.5f + (IsRight ? Vector3.right : Vector3.left) * 1.5f;
        grabbedObject.RefreshOnMovingPlateform();
        grabbedObject = null;
        if (playerAnimator)
            playerAnimator.SetBool("Grab", false);
    }

    //// Virtual method
    //public virtual void UseSpell(string _s)
    //{
    //    // Stops if grabbedObject
    //    if (grabbedObject)
    //        return;
    //}

    //// Virtual method
    //public virtual void StopSpell()
    //{
    //    // Stops if grabbedObject
    //    if (grabbedObject)
    //        return;
    //}
    float coyoteVar = -999;

    // Makes the character jump
    void Jump()
    {
        // Checks if input was in direction of the ground
        if (yMovement < 0)
        {
            // Cast below character to found if there is any SemiSolid plateform
            RaycastHit2D[] _hits = new RaycastHit2D[1];
            if (collider.Cast(Vector3.down, movableValues.SemisolidFilter, _hits) > 0)
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
    protected void StopJump() => isJumping = false;

    protected override void OnSetGrounded()
    {
        base.OnSetGrounded();
        if(IsGrounded)
            isJumping = false;
    }
}
