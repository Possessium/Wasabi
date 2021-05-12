// ======= Created by Lucas Guibert - https://github.com/LucasJoestar ======= //
//
// Notes : merci de me laisser te voler ton script <3
//
// ========================================================================== //

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//namespace Nowhere
//{
/// <summary>
/// Used to determine collision contacts 
/// when calculating object movement.
/// </summary>
public enum CollisionSystem
{
    Simple,
    Complex,
    Physics,
    Custom
}

[RequireComponent(typeof(Rigidbody2D))]
public class LG_Movable : MonoBehaviour
{
    #region Fields / Properties
    protected const float castMaxDistanceDetection = .001f;
    protected const int collisionSystemRecursionCeil = 3;
    public bool CanMove { get; protected set; } = true;

    // -----------------------
    

    [SerializeField] protected new BoxCollider2D collider = null;
    public BoxCollider2D MovableCollider { get { return collider; } }
    [SerializeField] protected new Rigidbody2D rigidbody = null;

    /*[SerializeField] */protected bool isAwake = true;
    /*[SerializeField] */protected bool useGravity = true;

    public bool IsAwake
    {
        get { return isAwake; }
        protected set
        {
#if UNITY_EDITOR
            if (!Application.isPlaying) return;
#endif

            if (value != isAwake)
            {
                isAwake = value;
            }
        }
    }

    public bool UseGravity
    {
        get { return useGravity; }
        protected set
        {
#if UNITY_EDITOR
            if (!Application.isPlaying) return;
#endif

            if (value != useGravity)
            {
                useGravity = value;
            }

        }
    }


    /*[SerializeField] */protected int facingSide = 1;
    [SerializeField] protected bool isGrounded = false;

    public int FacingSide { get { return facingSide; } }
    public bool IsGrounded { get { return isGrounded; } }

    /*[SerializeField, Min(0)] */protected float speed = 1;
    /*[SerializeField, Min(0)] */protected float speedCoef = 1;

    // --------------------------------------------------
    //
    // Velocity variables
    //
    // Movable class velocity is composed of 3 Vector2 :
    //  • Force, which is related to external forces, having an impact in duration (like wind)
    //  • Instant Force, also external forces by applied for one frame only (like recoil)
    //  • Movement, the velocity applied by the object itself (like walking)
    //

    /*[SerializeField] */protected Vector2 force = Vector2.zero;
    /*[SerializeField] */protected Vector2 instantForce = Vector2.zero;
    /*[SerializeField] */protected Vector2 movement = Vector2.zero;

    // -----------------------

#if UNITY_EDITOR

    /*[SerializeField] */protected Vector2 previousPosition = new Vector2();
#endif

    // -----------------------

    /*[SerializeField] */protected Vector2 groundNormal = new Vector2();
    #endregion


    #region Methods

    #region Public

    public bool IsOnMovingPlateform { get; private set; } = false;
    List<Collider2D> ignoredColliders = new List<Collider2D>();
    public void SetOnMovingPlateform(bool _state, Collider2D _plateformCollider)
    {
        IsOnMovingPlateform = isGrounded = _state;
        useGravity = !_state;
        force.y = 0;
        if (_state)
        {
            LastMovingPlateformCollider = _plateformCollider;

            isJumping = false;
            AddIgnoredCollider(_plateformCollider);

            Vector2 _p = new Vector2(rigidbody.position.x, _plateformCollider.bounds.max.y);
            _p.y += Vector2.Distance(_p, transform.position);

            SetPosition(_p);
            if(!transform.parent)
                transform.parent = _plateformCollider.transform;
        }

        else
        {
            if (transform.parent)
            {
                transform.parent = null;
            }

            RemoveIgnoredCollider(_plateformCollider);
        }
    }

    public void RefreshOnMovingPlateform()
    {
        RaycastHit2D[] _hits = new RaycastHit2D[4];

        if(collider.Cast(Vector2.down, _hits, .1f) > 0)
        {
            for (int i = 0; i < _hits.Length; i++)
            {
                if(_hits[i] && _hits[i].transform.GetComponent<WSB_MovingPlateform2>() && _hits[i].collider.bounds.max.y < collider.bounds.min.y)
                {
                    SetOnMovingPlateform(true, _hits[i].collider);
                    return;
                }
            }
        }
        SetOnMovingPlateform(false, LastMovingPlateformCollider);
    }

    public Collider2D LastMovingPlateformCollider { get; private set; }

    public void RemoveIgnoredCollider(Collider2D _c)
    {
        if (ignoredColliders.Contains(_c))
            ignoredColliders.Remove(_c);
    }

    public void AddIgnoredCollider(Collider2D _c)
    {
        if (!ignoredColliders.Contains(_c))
            ignoredColliders.Add(_c);
    }

    #endregion

    #region Speed
    /// <summary>
    /// Adds a speed coefficient to the object velocity.
    /// </summary>
    public void AddSpeedCoef(float _coef)
    {
        if (_coef != 0)
            speedCoef *= _coef;
    }

    /// <summary>
    /// Remove a speed coefficient from the object velocity.
    /// </summary>
    public void RemoveSpeedCoef(float _coef)
    {
        if (_coef != 0)
            speedCoef /= _coef;
    }
    #endregion

    #region Velocity
    /// <summary>
    /// Adds a force to the object velocity.
    /// Force value is decreased over time.
    /// </summary>
    public virtual void AddForce(Vector2 _force) => force += _force;

    /// <summary>
    /// Adds a force to the object velocity for this frame only.
    /// </summary>
    public virtual void AddInstantForce(Vector2 _instantForce) => instantForce += _instantForce;

    public void TrampolineJump(Vector2 _force)
    {
        force.y = 0;
        force += _force;
        semiSolidCollider = null;
        dontResetSemiSolid = false;
    }

    float 所有 = 0;

    public virtual void MoveHorizontally(float _movement)
    {
        if (_movement == 0)
        {
            if(speed != 0)
                speed = Mathf.MoveTowards(speed, 0, Time.deltaTime * (IsGrounded ? movableValues.GroundDeceleration : movableValues.AirDeceleration));
            所有 = speed;
            return;
        }

        所有 = Mathf.Min(所有 + Time.deltaTime, movableValues.SpeedCurve[movableValues.SpeedCurve.length - 1].time);
        speed = movableValues.SpeedCurve.Evaluate(所有);

        movement.x += (_movement * speed);
    }

    protected virtual void MoveVertically(float _movement) => movement.y += _movement;

    // -----------------------

    /// <summary>
    /// Get the object velocity movement for this frame.
    /// </summary>
    protected virtual Vector2 GetVelocity() => (((force + movement) * Time.deltaTime) + instantForce) * speedCoef;

    // -----------------------

    float previousXForce = 0;
    float previousXVelocity = 0;
    protected virtual void ComputeVelocityBeforeMovement()
    {
        // Slowly decrease force over time.
        if (force.x != 0)
        {
            float _maxDelta = isGrounded ?
                              movableValues.GroundDeceleration :
                              movableValues.AirDeceleration;

            // Calculs when going to opposite force direction.
            if (movement.x != 0)
            {
                if (Mathf.Sign(force.x) != Mathf.Sign(movement.x))
                {
                    _maxDelta = Mathf.Max(_maxDelta, Mathf.Abs(movement.x) * 2);
                    movement.x = Mathf.MoveTowards(movement.x, 0, Mathf.Abs(force.x) * Time.deltaTime);
                }
                else
                    movement.x = Mathf.Max(0, Mathf.Abs(movement.x) - Mathf.Abs(force.x)) * Mathf.Sign(movement.x);
            }

            // Calculs when going to opposite force direction,
            // compared to previous frame.
            float _previousXOtherVelocity = previousXVelocity - previousXForce;

            if (_previousXOtherVelocity != 0 && previousXForce != 0 && Mathf.Sign(_previousXOtherVelocity) != Mathf.Sign(previousXForce))
            {
                float _difference = Mathf.Abs(_previousXOtherVelocity);

                if (!(_previousXOtherVelocity != 0 && instantForce.x + movement.x != 0 && Mathf.Sign(_previousXOtherVelocity) != Mathf.Sign(instantForce.x + movement.x)))
                    _difference -= Mathf.Abs(instantForce.x + movement.x);

                if (_difference > 0)
                    force.x = Mathf.MoveTowards(force.x, 0, _difference);
            }

            // Reduce force
            if (_maxDelta != 0)
                force.x = Mathf.MoveTowards(force.x, 0, _maxDelta * Time.deltaTime);
        }

        // -----------------------

        previousXForce = force.x;
        previousXVelocity = force.x + instantForce.x + movement.x;

        // If going to opposite force direction, accordingly reduce force and movement.
        if (force.y != 0 && movement.y != 0 && Mathf.Sign(force.y) != Mathf.Sign(movement.y))
        {
            float _maxDelta = Mathf.Abs(movement.y);

            movement.y = Mathf.MoveTowards(movement.y, 0, Mathf.Abs(force.y));
            force.y = Mathf.MoveTowards(force.y, 0, _maxDelta * Time.deltaTime);
        }
    }
    #endregion

    #region Physics

    protected virtual void PhysicsUpdate()
    {
        if(useGravity)
            AddGravity();
    }

    // -----------------------

    protected void AddGravity()
    {
        if (force.y > -movableValues.MaxGravity)
        {
            AddForce(new Vector2(0, Mathf.Max(Physics2D.gravity.y * Time.deltaTime, -movableValues.MaxGravity - force.y)));
        }
    }

    protected void AddGravity(float _gravityCoef, float _maxGravityCoef)
    {
        float _maxGravityValue = -movableValues.MaxGravity * _maxGravityCoef;
        if (force.y > _maxGravityValue)
        {
            AddForce(new Vector2(0, Mathf.Max(Physics2D.gravity.y * _gravityCoef * Time.deltaTime, _maxGravityValue - force.y)));
        }
    }
    #endregion

    #region Movements

    protected virtual void MovableUpdate()
    {
#if UNITY_EDITOR
        // Refresh position if object moved in editor
        if (previousPosition != rigidbody.position) RefreshPosition();
#endif

        if (!((force + instantForce + movement) == null))
        {
            Vector2 _lastPosition = rigidbody.position;

            ComputeVelocityBeforeMovement();
            ComplexCollisionsSystem();
            //CollisionSystemDelegate();
            RefreshPosition();

            Vector2 _finalMovement = rigidbody.position - _lastPosition;

            if (useGravity)
                RefreshGroundState(_finalMovement);

            OnAppliedVelocity(_finalMovement);
        }
        else
            OnAppliedVelocity(Vector2.zero);

#if UNITY_EDITOR
        previousPosition = rigidbody.position;
#endif
    }

    /// <summary>
    /// Set this object position.
    /// Use this instead of setting <see cref="Transform.position"/>.
    /// </summary>
    public void SetPosition(Vector2 _position)
    {
        if (rigidbody.position != _position)
        {
            rigidbody.position = _position;
            RefreshPosition();
        }
    }

    // -----------------------

    private void RefreshGroundState(Vector2 _movement)
    {
        if (_movement == Vector2.zero)
            return;

        // Iterate over movement hits to find if one of these
        // can be considered as ground.
        bool _isGrounded = false;

        for (int _i = 0; _i < castBufferCount; _i++)
        {
            if (castBuffer[_i].normal.y >= movableValues.GroundClimbHeight)
            {
                _isGrounded = true;
                groundNormal = castBuffer[_i].normal;
                break;
            }
        }

        // If didn't hit ground during movement,
        // try to get it with last ground normal inverse direction cast.
        //
        // Necessary when movement magnitude is inferior to default contact offset.
        if (!_isGrounded)
        {
            _isGrounded = CastCollider(groundNormal * Physics2D.defaultContactOffset * -2, out RaycastHit2D _groundHit) &&
                            (_groundHit.normal.y >= movableValues.GroundClimbHeight) &&
                            _groundHit.collider != semiSolidCollider
                            || IsOnMovingPlateform;

            if (_isGrounded)
            groundNormal = _groundHit.normal;

            else if (isGrounded)
                groundNormal = Vector2.up;
        }

        if (IsOnMovingPlateform)
            isGrounded = true;

        if (isGrounded != _isGrounded)
        {
            isGrounded = _isGrounded;
            OnSetGrounded();
        }
    }

    private void RefreshPosition()
    {
        ExtractFromCollisions();

        if ((Vector2)transform.position != rigidbody.position)
            transform.position = rigidbody.position;
    }

    // -----------------------

    /// <summary>
    /// Called after velocity has been applied.
    /// </summary>
    protected virtual void OnAppliedVelocity(Vector2 _movement) { }

    protected bool dontResetSemiSolid = false;

    /// <summary>
    /// Called when grounded value has been set.
    /// </summary>
    protected virtual void OnSetGrounded()
    {
        // Reduce horizontal force if not null when get grounded.
        if (isGrounded && !dontResetSemiSolid)
        {
            if (semiSolidCollider)
                semiSolidCollider = null;

            if (force.x != 0)
                force.x *= movableValues.OnGroundedHorizontalForceMultiplier;
        }
    }
    #endregion

    #region Collision Calculs
    protected static int castBufferCount = 0;
    protected static RaycastHit2D[] castBuffer = new RaycastHit2D[4];
    protected static RaycastHit2D[] extraCastBuffer = new RaycastHit2D[4];

    // -----------------------

    /// <summary>
    /// Move rigidbody according to a complex collision system.
    /// When hitting something, continue movement all along hit surface.
    /// Perform movement according to ground surface angle.
    /// </summary>
    private void ComplexCollisionsSystem()
    {
        // If grounded, adjust velocity according to ground normal.
        Vector2 _velocity = GetVelocity();
        if (isGrounded)
        {
            Vector2 _x = Vector2.Perpendicular(groundNormal);
            if (_x.x < 0) _x *= -1;
            _x *= _velocity.x;

            Vector2 _y = (_velocity.y < 0 ? groundNormal : Vector2.up) * _velocity.y;

            _velocity = _x + _y;
        }

        castBufferCount = 0;
        RecursiveComplexCollisions(_velocity, groundNormal);

        // Modify force according to hit surfaces.
        if (force != null)
        {
            for (int _i = 0; _i < castBufferCount; _i++)
            {
                if (isJumping && castBuffer[_i].normal.y == -1)
                {
                    jumpVar = force.y = 0;
                    isJumping = false;
                }

                if (!(force.x != 0 && castBuffer[_i].normal.x != 0 && Mathf.Sign(force.x) != Mathf.Sign(castBuffer[_i].normal.x)))
                {
                    force.x = 0;
                    if (force.y == 0) break;
                }

                if ((force.y < 0) && (castBuffer[_i].normal.y > movableValues.GroundClimbHeight))
                {
                    force.y = 0;
                    if (force.x == 0) break;
                }

            }
        }

        // Reset instant force and movement.
        instantForce = movement = Vector2.zero;
    }


    // -------------------------------------------
    // Recursive Calculs
    // -------------------------------------------

    /// <summary>
    /// Calculates complex collisions recursively.
    /// </summary>
    private void RecursiveComplexCollisions(Vector2 _velocity, Vector2 _normal, int _recursiveCount = 0)
    {
        int _amount = CastCollider(_velocity, extraCastBuffer, out float _distance);

        // No movement mean object is stuck into something, so return.
        if (_distance == 0)
            return;

        if (_amount == 0)
        {
            rigidbody.position += _velocity;
            GroundSnap(_velocity, _normal);
            //semiSolidCollider = null;
            return;
        }


        // Move rigidbody and get extra cast velocity.
        if ((_distance -= Physics2D.defaultContactOffset) > 0)
        {
            Vector2 _normalizedVelocity = _velocity.normalized;

            rigidbody.position += _normalizedVelocity * _distance;
            _velocity = _normalizedVelocity * (_velocity.magnitude - _distance);
        }

        // If reached recursion limit, stop.
        if (_recursiveCount > collisionSystemRecursionCeil)
        {
            InsertCastInfos(extraCastBuffer, _amount);
            GroundSnap(_velocity, _normal);
            return;
        }

        // Get velocity outside normal surface, as pure value.
        float _angle = Vector2.SignedAngle(_normal, _velocity);
        _normal.Set(0, 1);
        _velocity = RotateVector(_normal, _angle) * _velocity.magnitude;

        Vector2 _hitNormal = extraCastBuffer[0].normal;
        _velocity = ClimbStep(_velocity, extraCastBuffer[0]);

        if ((Mathf.Abs(extraCastBuffer[0].normal.x) == 1) && (_velocity.x != 0))
        {
            for (int _i = 1; _i < _amount; _i++)
            {
                InsertCastInfo(extraCastBuffer[_i]);
            }
        }
        else
            InsertCastInfos(extraCastBuffer, _amount);

        if (_velocity != null)
        {
            // Reduce extra movement according to main impact normals.
            _velocity -= _hitNormal * Vector2.Dot(_velocity, _hitNormal);
            if (_velocity != null)
            {
                RecursiveComplexCollisions(_velocity, _normal, _recursiveCount + 1);
            }
        }
    }
    public Vector2 RotateVector(Vector2 _vector, float _angle)
    {
        // Equivalent :
        // Quaternion.AngleAxis(_angle, Vector3.forward) * _vector;

        float _sin = Mathf.Sin(_angle * Mathf.Deg2Rad);
        float _cos = Mathf.Cos(_angle * Mathf.Deg2Rad);

        return new Vector2()
        {
            x = (_cos * _vector.x) - (_sin * _vector.y),
            y = (_sin * _vector.x) + (_cos * _vector.y)
        };
    }

    // -------------------------------------------
    // Buffer Utilities
    // -------------------------------------------

    /// <summary>
    /// Inserts a RaycastHit information into the <see cref="castBuffer"/> buffer.
    /// </summary>
    protected void InsertCastInfo(RaycastHit2D _hit)
    {
        // Add new hit if there is enough space, or replace the last one.
        if (castBufferCount < castBuffer.Length)
        {
            castBuffer[castBufferCount] = _hit;
            castBufferCount++;
        }
        else
            castBuffer[castBufferCount - 1] = _hit;
    }

    /// <summary>
    /// Inserts an array of RaycastHit informations into the <see cref="castBuffer"/> buffer.
    /// </summary>
    protected void InsertCastInfos(RaycastHit2D[] _hits, int _amount)
    {
        // Add as many hits as possible while there is enough space,
        // or replace the last one if the buffer is already full.
        if (castBufferCount < castBuffer.Length)
        {
            for (int _i = 0; _i < _amount; _i++)
            {
                castBuffer[castBufferCount + _i] = _hits[_i];
                castBufferCount++;

                if (castBufferCount == castBuffer.Length)
                    return;
            }
        }
        else
            castBuffer[castBufferCount - 1] = _hits[0];
    }

    // -------------------------------------------
    // Special Movements
    // -------------------------------------------

    //[SerializeField] float groundClimbHeight = .7f;

    /// <summary>
    /// Make the object climb a surface, if possible.
    /// Climb cast infos are automatically added to the <see cref="castBuffer"/> buffer.
    /// </summary>
    protected Vector2 ClimbStep(Vector2 _velocity, RaycastHit2D _stepHit)
    {
        // If climbing is not necessary, return.
        if (!((_stepHit.normal.y == 0) && (_velocity.y <= 0) && (_velocity.x != 0)))
            return _velocity;


        // Heighten the rigidbody position and add opposite velocity,
        // then cast collider and get hit informations.
        rigidbody.position += new Vector2(0, movableValues.MaxHeightClimb);
        _velocity.y -= movableValues.MaxHeightClimb;

        int _amount = CastCollider(_velocity, extraCastBuffer, out float _climbDistance);

        if (_amount == 0)
        {
            rigidbody.position += _velocity;
            _velocity.Set(0, 0);
        }
        // If hit something, check if it's a different surface than the one trying to climb
        // and that the rigidbody is not stuck in ; when so, distance is equal to zero.
        else if ((_climbDistance != 0) && ((_stepHit.collider != extraCastBuffer[0].collider) || (_stepHit.normal != extraCastBuffer[0].normal)))
        {
            Vector2 _normalized = _velocity.normalized;
            rigidbody.position += _normalized * _climbDistance;
            _velocity = _normalized * (_velocity.magnitude - _climbDistance);

            InsertCastInfos(extraCastBuffer, _amount);
        }
        // If not, climb is failed so just reset position and velocity.
        else
        {
            rigidbody.position -= new Vector2(0, movableValues.MaxHeightClimb);
            _velocity.y += movableValues.MaxHeightClimb;
            _velocity.x = 0;
        }

        return _velocity;
    }

    // -----------------------

    /// <summary>
    /// Snap the object to the ground only if already grounded
    /// and movement is going down.
    /// Used for slopes & steps movements.
    /// Ground cast info is automatically added to the <see cref="castBuffer"/> buffer.
    /// </summary>
    protected bool GroundSnap(Vector2 _velocity, Vector2 _normal)
    {
        // Get going down velocity.
        _velocity = _normal * Vector2.Dot(_velocity, _normal);

        // If object was grounded and going down, try to snap to ground (slope & steps).
        if (isGrounded && (_velocity.y < 0))
            return GroundSnap(_normal);

        return false;
    }

    //[SerializeField] float groundSnapHeight = .5f;

    /// <summary>
    /// Snap the object to the ground.
    /// Used for slopes & steps movements.
    /// Ground cast info is automatically added to the <see cref="castBuffer"/> buffer.
    /// </summary>
    protected bool GroundSnap(Vector2 _normal)
    {
        if (CastCollider(_normal * -movableValues.GroundSnapHeight, out RaycastHit2D _snapHit))
        {
            rigidbody.position += _normal * -_snapHit.distance;
            InsertCastInfo(_snapHit);
            return true;
        }

        return false;
    }
    #endregion

    #region Collider Operations

    protected Collider2D semiSolidCollider = null;

    private static RaycastHit2D[] singleCastBuffer = new RaycastHit2D[1];

    protected bool CastCollider(Vector2 _movement, out RaycastHit2D _hit)
    {
        bool _result = collider.Cast(_movement, movableValues.Contact, singleCastBuffer, _movement.magnitude) > 0;
        _hit = singleCastBuffer[0];

        if (_result)
            _result = _hit.collider != semiSolidCollider;

        return _result;
    }

    protected int CastCollider(Vector2 _movement, RaycastHit2D[] _hitBuffer, out float _distance)
    {
        _distance = _movement.magnitude;

        int _hitAmount = collider.Cast(_movement, movableValues.Contact, _hitBuffer, _distance);

        if (_hitAmount == 0 && !dontResetSemiSolid)
        {
            semiSolidCollider = null;
        }

        for (int i = 0; i < _hitAmount; i++)
        {
            if ((_hitBuffer[i].transform.gameObject.layer == Mathf.Log(movableValues.SemisolidFilter.layerMask.value, 2)) && _hitBuffer[i].normal.y != 1 ||
                (_hitBuffer[i].transform.gameObject.layer == Mathf.Log(movableValues.SemisolidFilter.layerMask.value, 2)) && _hitBuffer[i] == semiSolidCollider ||
                IsOnMovingPlateform && transform.position.y > _hitBuffer[i].collider.transform.position.y)
            {
                semiSolidCollider = _hitBuffer[i].collider;
                _hitAmount = i;
                break;
            }
            if (_hitBuffer[i].transform.gameObject.layer != Mathf.Log(movableValues.SemisolidFilter.layerMask.value, 2))
            {
                semiSolidCollider = null;
            }
        }

        if (_hitAmount > 0)
        {
            // Hits are already sorted by distance, so simply get closest one.
            _distance = Mathf.Max(0, _hitBuffer[0].distance - Physics2D.defaultContactOffset);

            // Retains only closest hits by ignoring those with too distants.
            for (int _i = 1; _i < _hitAmount; _i++)
            {
                if ((_hitBuffer[_i].distance + castMaxDistanceDetection) > _hitBuffer[0].distance) return _i;
            }
        }
        return _hitAmount;
    }

    // -----------------------

    protected static Collider2D[] overlapBuffer = new Collider2D[6];

    private void ExtractFromCollisions()
    {
        int _count = collider.OverlapCollider(movableValues.Contact, overlapBuffer);
        ColliderDistance2D _distance;

        for (int _i = 0; _i < _count; _i++)
        {
            if (ignoredColliders.Contains(overlapBuffer[_i]))
                continue;

            // If overlap, extract from collision.
            _distance = collider.Distance(overlapBuffer[_i]);

            if ((
                   _distance.isValid
                && _distance.isOverlapped
                && overlapBuffer.Length > _i
                && overlapBuffer[_i].transform.gameObject.layer != Mathf.Log(movableValues.SemisolidFilter.layerMask.value, 2)
               )
            ||
               (
                   _distance.normal.y == -1
                && _distance.pointB.y < transform.position.y + .01f
                && !dontResetSemiSolid
                && !transform.parent
               ))
            {
                rigidbody.position += _distance.normal * _distance.distance * 2;
            }
               
        }
    }
    #endregion
     
    protected float jumpVar = 0;
    /*[SerializeField]*/ protected bool isJumping = false;
    [SerializeField] protected SO_MovableValues movableValues;

    #region Monobehaviour
    public virtual void Start()
    {
#if UNITY_EDITOR
        previousPosition = transform.position;
#endif

        //// Initialize object contact filter.
        //movableValues.Contact.layerMask = Physics2D.GetLayerCollisionMask(gameObject.layer);
        //movableValues.Contact.useLayerMask = true;

        groundNormal = Vector2.up;
        //CollisionSystem = collisionSystem;
    }

    public virtual void Update()
    {
        if (WSB_GameManager.Paused)
            return;

        if (!GetComponent<WSB_Player>() && force.y > 5)
            force.y = 5;

        PhysicsUpdate();
        MovableUpdate();
    }
    #endregion

    #endregion
}
//}
