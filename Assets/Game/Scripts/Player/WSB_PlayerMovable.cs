using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class WSB_PlayerMovable : LG_Movable
{
    public float XMovement = 0;
    public float YMovement = 0;

    public SO_ControllerValues ControllerValues = null;
    public GameObject Rend = null;

    public bool Turning = false;
    public bool IsRight = true;

    [SerializeField] private Animator playerAnimator = null;
    public Animator PlayerAnimator { get { return playerAnimator; } }

    private bool jumpInput = false;

    private float coyoteVar = -999;
    private float jumpOriginHeight = 0;

    private string materialName = "";
    public AK.Wwise.Event Landingsound;
    #region Animations
    [SerializeField] Transform jumpPosition = null;

    [SerializeField] GameObject jumpFX = null;
    [SerializeField] GameObject jumpInplaceFX = null;
    #endregion



    #region ANIMATION HASHES
    private static readonly int run_Hash = Animator.StringToHash("Run");
    private static readonly int jump_Hash = Animator.StringToHash("Jump");
    private static readonly int grounded_Hash = Animator.StringToHash("Grounded");
    private static readonly int rotate_Hash = Animator.StringToHash("Rotate");
    private static readonly int unWalk_hash = Animator.StringToHash("UnWalk");
    #endregion

    [SerializeField] private bool forceSpawn = true;
    [SerializeField] private Vector3 spawnPosition = Vector3.zero;

    public void FootstepSound(GameObject SwitchSound)
    {

        switch (materialName)
        {
            case "mtl_plateforme_metal":
                AkSoundEngine.SetSwitch("FOOT_TEXTUR", "Metal", SwitchSound);
                break;
            case "mtl_plateforme_semi_solid":
                AkSoundEngine.SetSwitch("FOOT_TEXTUR", "Puddle", SwitchSound);
                break;
            case "mtl_plateforme_terre":
                AkSoundEngine.SetSwitch("FOOT_TEXTUR", "Dirt", SwitchSound);
                break;

        }
    }
    private void CheckGroundTexture()
    {

        if (!IsGrounded) return;
        LayerMask mask = LayerMask.GetMask("SemiSolid", "Test");
        RaycastHit2D hit;
        hit = Physics2D.Raycast(transform.position, Vector2.up, 50, mask);
        if (hit.collider != null)
        {
            materialName = hit.collider.gameObject.GetComponent<MeshRenderer>().material.name;
            Debug.Log(materialName + "DebugRaycast");
        }
    }
    public override void Start()
    {
        base.Start();
        if (forceSpawn)
            SetPosition(spawnPosition);
    }

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
                    if (IsGrounded || (Time.time - coyoteVar < ControllerValues.JumpDelay))
                        Jump();
            }
            if (playerAnimator && CanMove)
            {

                playerAnimator.SetFloat(run_Hash, speed / movableValues.SpeedCurve.Evaluate(movableValues.SpeedCurve[movableValues.SpeedCurve.length - 1].time) * (IsRight ? 1 : -1));
                if (isJumping && isGrounded)
                {
                    AkSoundEngine.SetSwitch("FOOT_TYPE", "JUMP_start", PlayerAnimator.gameObject);
                }
                playerAnimator.SetBool(jump_Hash, isJumping);

                if (IsGrounded)
                    playerAnimator.SetBool(grounded_Hash, true);
                else
                {
                    if (IsOnMovingPlateform && !isJumping)
                        playerAnimator.SetBool(grounded_Hash, true);

                    else if (semiSolidCollider && !isJumping)
                        playerAnimator.SetBool(grounded_Hash, true);

                    else
                        playerAnimator.SetBool(grounded_Hash, false);
                }

                //PlayerAnimator.SetBool(grounded_Hash,IsGrounded ? true : (IsOnMovingPlateform && !isJumping) ? true : false);

                if (CanMove)
                {
                    if (XMovement < 0 && IsRight)
                    {
                        IsRight = false;
                        if (IsGrounded)
                        {
                            playerAnimator.SetTrigger(rotate_Hash);
                        }
                        else
                            Rend.transform.eulerAngles = new Vector3(Rend.transform.eulerAngles.x, -90, Rend.transform.eulerAngles.z);
                    }
                    else if (XMovement>0 && IsRight && isGrounded && !isJumping)
                    {
                        AkSoundEngine.SetSwitch("FOOT_TYPE", "WALK", PlayerAnimator.gameObject);
                    }
                    if (XMovement > 0 && !IsRight)
                    {
                        IsRight = true;
                        if (IsGrounded)
                        {
                            playerAnimator.SetTrigger(rotate_Hash);
                        }
                        else
                            Rend.transform.eulerAngles = new Vector3(Rend.transform.eulerAngles.x, 90, Rend.transform.eulerAngles.z);

                    }
                    else if (XMovement < 0 && !IsRight && isGrounded && !isJumping)
                    {
                        AkSoundEngine.SetSwitch("FOOT_TYPE", "WALK", PlayerAnimator.gameObject);
                    }
                }
            }
            if (isJumping)
            {
                RaycastHit2D[] _hits = new RaycastHit2D[5];
                bool _jump = true;
                if (MovableCollider.Cast(Vector2.down, _hits, .5f) > 0)
                {
                    for (int i = 0; i < _hits.Length; i++)
                    {
                        if (_hits[i] && _hits[i].transform.GetComponent<LG_Movable>() && !_hits[i].transform.GetComponent<LG_Movable>().IsGrounded)
                            _jump = false;
                    }
                }

                if (_jump)
                {
                    // Stop the jump if input is released & peak of jump icn't reached yet
                    if (!jumpInput && jumpVar < .3f)
                    {
                        jumpOriginHeight -= (ControllerValues.JumpCurve.Evaluate(.3f) - ControllerValues.JumpCurve.Evaluate(jumpVar));
                        jumpVar = .3f;
                    }
                    // Get the value corresponding to the curve set
                    else
                    {
                        jumpVar += Time.deltaTime;

                        // Stop jump if peak reached
                        if (jumpVar > ControllerValues.JumpCurve[ControllerValues.JumpCurve.length - 1].time)
                        {
                            jumpVar = ControllerValues.JumpCurve[ControllerValues.JumpCurve.length - 1].time;
                            isJumping = false;
                        }

                        MoveVertically((jumpOriginHeight + (ControllerValues.JumpCurve.Evaluate(jumpVar)) - MovableRigidbody.position.y) / Time.deltaTime);
                    }
                }
            }

            if (XMovement != 0)
                IsRight = XMovement > 0;

        }

        base.Update();
    }

    #region Input reading
    // Reads x & y movement and sets it in xMovement & yMovement
    public void Move(InputAction.CallbackContext _context)
    {
        if (_context.valueType != typeof(Vector2) /*|| !CanMove*/) return;
        XMovement = _context.ReadValue<Vector2>().x;
        YMovement = _context.ReadValue<Vector2>().y;
        PressDown = _context.ReadValue<Vector2>().y < -.8f;
    }

    // Reads jump input and sets it in jumpInput
    public void Jump(InputAction.CallbackContext _context)
    {
        if (_context.valueType == typeof(float))
            jumpInput = _context.ReadValue<float>() == 1;
    }
    #endregion

    public void Turn()
    {
        AkSoundEngine.SetSwitch("FOOT_TYPE", "SCUFF", PlayerAnimator.gameObject);
        Rend.transform.eulerAngles = new Vector3(Rend.transform.eulerAngles.x, IsRight ? 90 : -90, Rend.transform.eulerAngles.z);
    }

    public void ResetAnimations()
    {
        playerAnimator.SetTrigger(unWalk_hash);
        playerAnimator.SetBool(grounded_Hash, true);
        playerAnimator.SetBool(jump_Hash, false);
    }

    #region Jump
    // Makes the character jump
    void Jump()
    {
        
        // Checks if input was in direction of the ground
        if (PressDown)
        {
            // Cast below character to found if there is any SemiSolid plateform
            RaycastHit2D[] _hits = new RaycastHit2D[1];
            if (MovableCollider.Cast(Vector3.down, movableValues.SemisolidFilter, _hits) > 0)
            {

                // If found set collider in ignoredCollider and don't do the jump
                semiSolidCollider = _hits[0].collider;
                return;
            }
        }

        PressDown = false;

        isJumping = true;
           

        if (XMovement != 0)
        {
            if (jumpFX && jumpPosition)
            {
                ParticleSystemRenderer _fx = Instantiate(jumpFX, jumpPosition.position, Quaternion.identity).GetComponent<ParticleSystemRenderer>();
                if (_fx)
                {

                    _fx.transform.localScale = Vector3.one * (GetComponent<WSB_Ban>() ? 2 : 1);
                    _fx.flip = new Vector3(IsRight ? 0 : 1, 0, 0);
                    _fx.transform.eulerAngles = new Vector3(0, IsRight ? 0 : 180, 0);
                }
            }
        }
        else
        {
            if (jumpInplaceFX && jumpPosition)
            {

                ParticleSystemRenderer _fx = Instantiate(jumpInplaceFX, jumpPosition.position, Quaternion.identity).GetComponent<ParticleSystemRenderer>();
                if (_fx)
                {
                    _fx.transform.localScale = Vector3.one * (GetComponent<WSB_Ban>() ? 2 : 1);
                    _fx.flip = new Vector3(IsRight ? 0 : 1, 0, 0);
                    _fx.transform.eulerAngles = new Vector3(0, IsRight ? 0 : 180, 0);

                    ParticleSystemRenderer _fxChild = _fx.transform.GetChild(0).GetComponent<ParticleSystemRenderer>();
                    if (_fxChild)
                        _fxChild.flip = new Vector3(IsRight ? 1 : 0, 0, 0);
                }
            }
        }


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

        if (IsGrounded && isJumping)
        {
         
               AkSoundEngine.SetSwitch("FOOT_TYPE", "JUMP_land", PlayerAnimator.gameObject);
               Landingsound.Post(PlayerAnimator.gameObject);
          

            isJumping = false;
       
        }


    }
    #endregion
    void MyAnimationEventCallback(AnimationEvent evt)
    {
        if (evt.animatorClipInfo.weight > 0.5f)
        {
            //foreach (AK.Wwise.Event WwiseEvent in myEvents)
            //{
            //    WwiseEvent.Post(gameObject);
            //}
            // Debug.Log("eventPlayed");
        }
    }
}