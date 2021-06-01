using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WSB_Movable : MonoBehaviour
{
    protected Rigidbody2D Physic = null;
    protected BoxCollider2D Collider = null;
    [SerializeField] bool canMove = true;
    [SerializeField] bool lockX = false;
    [SerializeField] bool ignoreGroundSnap = false;
    float startX = 0;

    public bool CanMove { get { return canMove; } }

    #region Unity
    private void Awake()
    {
        if (!Physic) Physic = GetComponent<Rigidbody2D>();
        if (!Physic)
        {
            Debug.LogError($"Erreur, component Rigidbody2D manquant sur {transform.name}");
            Destroy(this);
        }
        if (!Collider) Collider = GetComponent<BoxCollider2D>();
        if (!Collider)
        {
            Debug.LogError($"Erreur, component Collider2D manquant sur {transform.name}");
            Destroy(this);
        }
        if (lockX)
            startX = transform.position.x;
    }

    private void Update()
    {
        // Hold if game is on Pause
        if (WSB_GameManager.Paused)
            return;

        if (force.y < -20)
            Physic.position -= Vector2.up * .05f;

        if (lockX)
            transform.position = new Vector3(startX, transform.position.y, transform.position.z);

        if(!IsOnMovingPlateform)
            ApplyGravity();

        if ((force + instantForce + movement) != Vector2.zero)
        {
            Vector2 _pos = Physic.position;

            CalculCollision();
            RefreshPosition();

            OnAppliedVelocity(Physic.position - _pos);

            instantForce = movement = Vector2.zero;
        }
        else
            OnAppliedVelocity(Vector2.zero);

    }

    #endregion

    #region Controller

    [SerializeField] SO_ControllerValues controllerValues = null;

    [SerializeField] Vector2 force = Vector2.zero;
    [SerializeField] Vector2 instantForce = Vector2.zero;
    [SerializeField] Vector2 movement = Vector2.zero;

    [SerializeField] Vector2 groundNormal = new Vector2();

    [SerializeField] bool isGrounded = false;
    public bool IsOnMovingPlateform = false;



    void OnAppliedVelocity(Vector2 _velocity)
    {

    }

    // Refresh Physic position
    void RefreshPosition()
    {
        // Cast collider around itself
        int _amount = Collider.OverlapCollider(controllerValues.Contact, overlapBuffer);

        // Loop through colliders found
        for (int i = 0; i < _amount; i++)
        {
            // Skips this collider if tagged as ignored
            if (overlapBuffer[i] == ignoredCollider)
                continue;

            // Push out Physic position from the collider
            ColliderDistance2D _distance = Collider.Distance(overlapBuffer[i]);
            if (_distance.isOverlapped && (!overlapBuffer[i].transform.GetComponent<PlatformEffector2D>() || _distance.normal.y == -1))
                Physic.position += new Vector2(lockX ? 0 : _distance.normal.x, _distance.normal.y) * _distance.distance;
        }
    }




    #region Collision

    private static int bufferAmount = 0;
    private static readonly RaycastHit2D[] buffer = new RaycastHit2D[4];
    private static readonly RaycastHit2D[] castBuffer = new RaycastHit2D[4];
    private static readonly Collider2D[] overlapBuffer = new Collider2D[4];

    // Main collision method
    void CalculCollision()
    {
        Vector2 _velocity = GetVeloctity();
        if (isGrounded)
        {
            // Rotates _velocity such as it behaves on the normal flat surface
            Vector2 _x = Vector2.Perpendicular(groundNormal);
            if (_x.x < 0)
                _x *= -1;

            _x *= _velocity.x;
            Vector2 _y = (_velocity.y < 0 ? groundNormal : Vector2.up) * _velocity.y;

            _velocity = _x + _y;
        }

        bufferAmount = 0;
        CalculCollisionRecursively(_velocity, groundNormal);

        bool _isGrounded = false;
        for (int i = 0; i < bufferAmount; i++)
        {
            if (force != Vector2.zero)
                force -= castBuffer[i].normal * Vector2.Dot(force, castBuffer[i].normal);

            if (!_isGrounded && (castBuffer[i].normal.y > controllerValues.GroundMin))
            {
                _isGrounded = true;
                groundNormal = castBuffer[i].normal;
            }
        }

        if (!_isGrounded)
        {
            _isGrounded = (CastCollider(Vector2.down * Physics2D.defaultContactOffset * 2, out RaycastHit2D _hit) && (_hit.normal.y > controllerValues.GroundMin) && _hit.collider != ignoredCollider) || IsOnMovingPlateform;
            groundNormal = _isGrounded ? _hit.normal : Vector2.up;
        }

        if (_isGrounded)
            force.y = force.x = 0;

        if (_isGrounded != isGrounded)
            OnSetGrounded(_isGrounded);
    }

    void CalculCollisionRecursively(Vector2 _velocity, Vector2 _normal, int _iteration = 0)
    {
        int _amount = CastCollider(_velocity, out float _distance);

        if (_distance == 0)
            return;

        if (_amount == 0)
        {
            Physic.position += _velocity;
            if(!ignoreGroundSnap)
                GroundSnap(_velocity, _normal);
            return;
        }

        if ((_distance -= Physics2D.defaultContactOffset) > 0)
        {
            Vector2 _normalizedVelocity = _velocity.normalized;

            Physic.position += new Vector2(lockX ? 0 : _normalizedVelocity.x, _normalizedVelocity.y) * _distance;
            _velocity = _normalizedVelocity * (_velocity.magnitude - _distance);
        }

        InsertCastBuffer(_amount);

        if (_iteration > 3 && !ignoreGroundSnap)
        {
            GroundSnap(_velocity, _normal);
            return;
        }


        if (_normal.y != 1)
            _velocity = Quaternion.FromToRotation(_normal, Vector3.up) * _velocity;

        ClimbStep(ref _velocity, buffer[0]);

        Vector2 _hitNormal = castBuffer[0].normal;
        _velocity -= _hitNormal * Vector2.Dot(_velocity, _hitNormal);

        if (_velocity != Vector2.zero)
            CalculCollisionRecursively(_velocity, Vector2.zero, _iteration + 1);


    }
    void InsertCastBuffer(int _amount)
    {
        if (bufferAmount < buffer.Length)
        {
            for (int i = 0; i < _amount; i++)
            {
                buffer[bufferAmount + i] = castBuffer[i];
                bufferAmount++;

                if (bufferAmount == buffer.Length)
                    return;
            }
        }
        else
            buffer[bufferAmount - 1] = castBuffer[0];
    }

    bool CastCollider(Vector2 _velocity, out RaycastHit2D _hit)
    {
        bool _return = (Collider.Cast(_velocity, controllerValues.Contact, castBuffer, _velocity.magnitude) > 0) && (!castBuffer[0].transform.GetComponent<PlatformEffector2D>() && castBuffer[0].normal.y != -1);
        _hit = castBuffer[0];
        return _return;
    }

    [SerializeField] Collider2D ignoredCollider = null;

    int CastCollider(Vector2 _velocity, out float _distance)
    {
        _distance = _velocity.magnitude;
        int _amount = Collider.Cast(_velocity, controllerValues.Contact, castBuffer, _distance);

        if (_amount == 0)
            ignoredCollider = null;

        for (int i = 0; i < _amount; i++)
        {
            if ((castBuffer[i].transform.GetComponent<PlatformEffector2D>() || castBuffer[i].transform.GetComponentInChildren<PlatformEffector2D>()) && castBuffer[i].normal.y == -1 ||
                (castBuffer[i].transform.GetComponent<PlatformEffector2D>() || castBuffer[i].transform.GetComponentInChildren<PlatformEffector2D>()) && castBuffer[i] == ignoredCollider ||
                IsOnMovingPlateform && transform.position.y > castBuffer[i].collider.transform.position.y)
            {
                ignoredCollider = castBuffer[i].collider;
                _amount = i;
                break;
            }
            if (!castBuffer[i].transform.GetComponent<PlatformEffector2D>())
                ignoredCollider = null;
        }

        if (_amount > 0)
        {
            _distance = Mathf.Max(0, castBuffer[0].distance - Physics2D.defaultContactOffset);
            for (int i = 1; i < _amount; i++)
            {
                if ((castBuffer[i].distance + .001f) > castBuffer[0].distance)
                    return i;
            }
        }
        return _amount;
    }

    #endregion


    #region Physic

    void ApplyGravity() => AddForce((Physics2D.gravity * 2) * Time.deltaTime);

    void OnSetGrounded(bool _isGrounded)
    {
        isGrounded = _isGrounded;

        if (_isGrounded)
        {
            if (ignoredCollider)
                ignoredCollider = null;
        }
    }

    void ClimbStep(ref Vector2 _velocity, RaycastHit2D _hit)
    {
        if (_velocity.x == 0 || _hit.normal.y > controllerValues.GroundMin)
            return;
        Physic.position += new Vector2(0, controllerValues.MaxHeightClimb);
        _velocity.y -= controllerValues.MaxHeightClimb;

        int _amount = CastCollider(_velocity, out float _distance);

        if (_amount == 0)
        {
            Physic.position += new Vector2(lockX ? 0 : _velocity.x, _velocity.y);
            _velocity.Set(0, 0);
        }
        else if (_distance != 0 && ((_hit.collider != castBuffer[0].collider) || (_hit.normal != castBuffer[0].normal)))
        {
            Vector2 _normalized = _velocity.normalized;
            Physic.position += new Vector2(lockX ? 0 : _normalized.x, _normalized.y) * _distance;
            _velocity = _normalized * (_velocity.magnitude - _distance);

            InsertCastBuffer(_amount);
        }
        else
        {
            Physic.position -= new Vector2(0, controllerValues.MaxHeightClimb);
            _velocity.y += controllerValues.MaxHeightClimb;
            _velocity.x = 0;
        }
    }

    void GroundSnap(Vector2 _velocity, Vector2 _normal)
    {
        if (_normal.y != 1)
            _velocity = Quaternion.FromToRotation(_normal, Vector3.up) * _velocity;

        if (isGrounded && (_velocity.y < 0) && CastCollider(_normal * -1, out RaycastHit2D _snapHit))
        {
            Physic.position += new Vector2(lockX ? 0 : _normal.x, _normal.y) * -_snapHit.distance;
            InsertCastBuffer(1);
        }
    }

    #endregion


    #region AddForces
    public void StopVerticalForce() => force.y = 0;

    public void AddForce(Vector2 _force)
    {
        force += _force;
        if (force.y > 10)
            force.y = 10;
    }

    public void AddInstantForce(Vector2 _velocity) => instantForce += _velocity;

    #endregion

    public Vector2 GetVeloctity() => ((force + movement) * Time.deltaTime) + instantForce * controllerValues.SpeedCoef;

    #endregion
}
