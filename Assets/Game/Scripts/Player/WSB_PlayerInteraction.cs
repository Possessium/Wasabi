using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WSB_PlayerInteraction : MonoBehaviour
{
    [SerializeField] WSB_PlayerMovable movable = null;
    [SerializeField] Animator playerAnimator = null;

    [SerializeField] bool canAnimateLever = false;
    bool canAnimateButton = false;
    bool isLeverRight = false;

    private void Start()
    {
        if (!playerAnimator)
            TryGetComponent(out playerAnimator);
    }

    private void Update()
    {
        bool _isBlocked = false;


        if (grabbedObject && ((!movable.IsRight && movable.xMovement <= 0) || (movable.IsRight && movable.xMovement >= 0)))
        {
            Collider2D[] _hits = Physics2D.OverlapBoxAll((Vector2)grabbedObject.transform.position + grabbedObject.MovableCollider.offset, grabbedObject.MovableCollider.size * 1.1f, 0, movable.controllerValues.ContactGrabLayer);

            for (int i = 0; i < _hits.Length; i++)
            {
                if (_hits[i].transform != this.transform && _hits[i].transform != grabbedObject.transform)
                {
                    _isBlocked = true;
                    break;
                }

            }
            _hits = Physics2D.OverlapBoxAll((Vector2)grabbedObject.transform.position + grabbedObject.MovableCollider.offset, grabbedObject.MovableCollider.size, 0, movable.controllerValues.ContactGrabLayer);
            for (int i = 0; i < _hits.Length; i++)
            {
                if (_hits[i].transform != this.transform && _hits[i].transform != grabbedObject.transform)
                    movable.MovableRigidbody.position += (movable.IsRight ? Vector2.left : Vector2.right) * .1f;
            }
        }

        if (!_isBlocked)
            movable.MoveHorizontally(movable.xMovement);

        if (grabbedObject && !playerHands)
        {
            grabbedObject.transform.position = transform.position + Vector3.up * 1.5f + (movable.IsRight ? Vector3.right : Vector3.left) * 1.5f;
        }
    }

    public void ToggleLever(bool _s, bool _r = false)
    {
        canAnimateLever = _s;
        isLeverRight = _r;
    }

    public void Lever(InputAction.CallbackContext _ctx)
    {
        if (!_ctx.started)
            return;

        Collider2D _hit = Physics2D.OverlapBox(movable.MovableRigidbody.position + Vector2.up, Vector2.one, 0, movable.controllerValues.LeverLayer);
        if (_hit)
        {
            WSB_Lever _lever = _hit.GetComponent<WSB_Lever>();
            _lever.Interact();
            AnimateLever(_lever.Position);
        }
    }

    public void AnimateLever(Vector2 _pos)
    {
        if (playerAnimator)
        {
            movable.rend.transform.eulerAngles = new Vector3(movable.rend.transform.eulerAngles.x, isLeverRight ? 90 : -90, movable.rend.transform.eulerAngles.z);
            movable.IsRight = isLeverRight = !isLeverRight;
            movable.SetPosition(_pos);
            playerAnimator.SetTrigger("Lever");
            movable.CanMove = false;
        }
    }

    public void AnimationFinished(bool _s) => movable.CanMove = _s;

    #region ANIMATION HASHES
    private static readonly int pick_Hash = Animator.StringToHash("Pick"); //LA
    #endregion

    /*[SerializeField] */
    LG_Movable grabbedObject = null;
    public bool HeldObject { get { return grabbedObject; } }

    [SerializeField] Transform playerHands = null;
    [SerializeField] ContactFilter2D grabContactFilter = new ContactFilter2D();
    // Reads grab input and try to grab object
    public void GrabObject(InputAction.CallbackContext _context)
    {
        // Exit if not the beggining of the input
        if (!_context.started || (GetComponent<WSB_Lux>() && GetComponent<WSB_Lux>().Shrinked))
            return;

        if (playerAnimator)
            playerAnimator.SetTrigger(pick_Hash);

        if (GetComponent<WSB_Ban>())
        {
            if (grabbedObject)
                DropGrabbedObject();
            else
                TryGrab();
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
        //grabbedObject.transform.position = transform.position * 1.5f + (IsRight ? Vector3.right : Vector3.left) * 1.5f;
        grabbedObject.transform.parent = null;
        grabbedObject.transform.eulerAngles = Vector3.zero;
        grabbedObject.RefreshOnMovingPlateform();
        grabbedObject = null;
    }

    public void DropObject()
    {
        if (playerAnimator)
            playerAnimator.SetBool("Grab", false);

        DropGrabbedObject();
    }

    public void TryGrab()
    {
        RaycastHit2D[] _hit = new RaycastHit2D[1];

        // Cast on facing direction to check if there is an object
        if (movable.MovableCollider.Cast(movable.IsRight ? Vector2.right : Vector2.left, grabContactFilter, _hit, .5f) > 0)
        {
            if (_hit[0].transform.GetComponent<WSB_Movable>() && !_hit[0].transform.GetComponent<WSB_Movable>().CanMove)
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
                grabbedObject.transform.position = playerHands.transform.position + (movable.IsRight ? grabbedObject.transform.right : -grabbedObject.transform.right);
            }

            grabbedObject.MovableCollider.enabled = false;

            if (playerAnimator)
                playerAnimator.SetBool("Grab", true);
        }
    }










}
