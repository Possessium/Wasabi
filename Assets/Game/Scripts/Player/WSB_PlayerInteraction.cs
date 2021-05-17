using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WSB_PlayerInteraction : MonoBehaviour
{
    [SerializeField] private WSB_PlayerMovable movable = null;

    [SerializeField] private ContactFilter2D grabContactFilter = new ContactFilter2D();

    [SerializeField] private Animator playerAnimator = null;
    [SerializeField] private Transform playerHands = null;

    private bool isLeverRight = false;

    private LG_Movable grabbedObject = null;
        public bool HeldObject { get { return grabbedObject; } }

    #region FX
    [SerializeField] ParticleSystem sweat = null;


    #endregion


    #region ANIMATION HASHES
    private static readonly int pick_Hash = Animator.StringToHash("Pick");
    private static readonly int key_Hash = Animator.StringToHash("Key");
    private static readonly int lever_Hash = Animator.StringToHash("Lever");
    private static readonly int grab_Hash = Animator.StringToHash("Grab");
    #endregion


    private void Start()
    {
        if (!playerAnimator)
            TryGetComponent(out playerAnimator);
    }

    private void Update()
    {
        bool _isBlocked = false;


        if (grabbedObject && ((!movable.IsRight && movable.XMovement <= 0) || (movable.IsRight && movable.XMovement >= 0)))
        {
            Collider2D[] _hits = Physics2D.OverlapBoxAll((Vector2)grabbedObject.transform.position + grabbedObject.MovableCollider.offset, grabbedObject.MovableCollider.size * 1.1f, 0, movable.ControllerValues.ContactGrabLayer);

            for (int i = 0; i < _hits.Length; i++)
            {
                if (_hits[i].transform != this.transform && _hits[i].transform != grabbedObject.transform)
                {
                    _isBlocked = true;
                    break;
                }

            }
            _hits = Physics2D.OverlapBoxAll((Vector2)grabbedObject.transform.position + grabbedObject.MovableCollider.offset, grabbedObject.MovableCollider.size, 0, movable.ControllerValues.ContactGrabLayer);
            for (int i = 0; i < _hits.Length; i++)
            {
                if (_hits[i].transform != this.transform && _hits[i].transform != grabbedObject.transform)
                    movable.MovableRigidbody.position += (movable.IsRight ? Vector2.left : Vector2.right) * .1f;
            }
        }

        if (!_isBlocked)
            movable.MoveHorizontally(movable.XMovement);

        if (grabbedObject && !playerHands)
        {
            grabbedObject.transform.position = transform.position + Vector3.up * (GetComponent<WSB_Ban>() ? 2 : 1) + (movable.IsRight ? Vector3.right : Vector3.left) * 1.5f;
        }
    }


    public void AnimationFinished(bool _s) => movable.CanMove = _s;

    #region Lever
    public void Lever(InputAction.CallbackContext _ctx)
    {
        if (!_ctx.started)
            return;

        Collider2D _hit = Physics2D.OverlapBox(movable.MovableRigidbody.position + Vector2.up, Vector2.one, 0, movable.ControllerValues.LeverLayer);
        if (_hit)
        {
            WSB_Lever _lever = _hit.GetComponent<WSB_Lever>();
            
            if (!_lever.CanPress)
                return;

            _lever.Interact();
            AnimateLever(_lever.Position);
        }
    }

    public void AnimateKey()
    {
        playerAnimator.SetTrigger(key_Hash);
        movable.IsRight = true;
        movable.Rend.transform.eulerAngles = new Vector3(movable.Rend.transform.eulerAngles.x, 90, movable.Rend.transform.eulerAngles.z);
        movable.CanMove = false;
        movable.StopMoving();
    }

    public void AnimateLever(Vector2 _pos)
    {
        if (playerAnimator)
        {
            movable.Rend.transform.eulerAngles = new Vector3(movable.Rend.transform.eulerAngles.x, isLeverRight ? 90 : -90, movable.Rend.transform.eulerAngles.z);
            movable.IsRight = isLeverRight = !isLeverRight;
            movable.SetPosition(_pos);
            playerAnimator.SetTrigger(lever_Hash);
            movable.CanMove = false;
        }
    }
    #endregion

    #region Power
    public void GrabObject(InputAction.CallbackContext _context)
    {
        // Exit if not the beggining of the input
        if (!_context.started || (GetComponent<WSB_Lux>() && GetComponent<WSB_Lux>().Shrinked))
            return;

        if (playerAnimator)
            playerAnimator.SetTrigger(pick_Hash);
    }

    void DropGrabbedObject()
    {
        if (!grabbedObject)
            return;

        grabbedObject.transform.parent = transform.parent;

        grabbedObject.MovableCollider.enabled = true;
        grabbedObject.enabled = true;
        grabbedObject.GetComponent<WSB_Power>().enabled = true;
        grabbedObject.GetComponent<WSB_Power>().ActivatePower();
        grabbedObject.transform.parent = null;
        grabbedObject.transform.eulerAngles = Vector3.zero;
        grabbedObject.SetPosition(new Vector3(grabbedObject.MovableRigidbody.position.x, grabbedObject.MovableRigidbody.position.y, 2));
        grabbedObject.RefreshOnMovingPlateform();
        grabbedObject = null;
        
        if (sweat)
            sweat.Stop();
    }

    public void DropObject()
    {
        if (playerAnimator)
            playerAnimator.SetBool(grab_Hash, false);

        DropGrabbedObject();
    }
     
    public void TryGrab()
    {
        RaycastHit2D[] _hit = new RaycastHit2D[1];

        // Cast on facing direction to check if there is an object
        if (movable.MovableCollider.Cast(movable.IsRight ? Vector2.right : Vector2.left, grabContactFilter, _hit, .5f) > 0)
        {
            if (_hit[0].transform.GetComponent<LG_Movable>() && !_hit[0].transform.GetComponent<LG_Movable>().CanMove)
                return;
            // Search for WSB_Pot component
            if (_hit[0].transform.GetComponent<WSB_Power>())

                // Sets grabbedObject var
                _hit[0].transform.TryGetComponent(out grabbedObject);

            grabbedObject.enabled = false;
            grabbedObject.GetComponent<WSB_Power>().DeactivatePower(movable);

            if (playerHands)
            {
                grabbedObject.transform.parent = playerHands;
                grabbedObject.transform.position = playerHands.transform.position + ((movable.IsRight ? grabbedObject.transform.right : -grabbedObject.transform.right) * (GetComponent<WSB_Ban>() ? 1.2f : .85f));
            }

            grabbedObject.MovableCollider.enabled = false;

            if (playerAnimator)
                playerAnimator.SetBool(grab_Hash, true);

            if (sweat)
                sweat.Play();
        }
    }
    #endregion
}
