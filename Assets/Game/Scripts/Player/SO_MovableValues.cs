using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MovableValues", menuName = "ScriptableObjects/CreateMovableValues", order = 1)]
public class SO_MovableValues : ScriptableObject
{
    [Tooltip("Contact filter to know what must collide with it")]
    public ContactFilter2D Contact = new ContactFilter2D();

    [Tooltip("Contact filter for the semisolids")]
    public ContactFilter2D SemisolidFilter = new ContactFilter2D();

    [Tooltip("Height for object to climb")]
    public float GroundClimbHeight = .7f;

    [Tooltip("Minimum y normal for object to snap on it")]
    public float GroundSnapHeight = .5f;

    [Tooltip("Max height for object to climb without jumping")]
    public float MaxHeightClimb = .5f;

    [Tooltip("Speed coeficient for object movement")]
    public float SpeedCoef = 1;

    [Tooltip("Deceleration value used when stopping in air")]
    public float AirDeceleration = 5;

    [Tooltip("Deceleration value used when stopping on ground")]
    public float GroundDeceleration = 12.5f;

    [Tooltip("Deceleration value used when stopping on ground")]
    public float MaxGravity = 25;

    [Tooltip("Deceleration value used when stopping on ground")]
    public float OnGroundedHorizontalForceMultiplier = .35f;

    [Tooltip("Curve of the acceleration of the object")]
    public AnimationCurve SpeedCurve = new AnimationCurve();
}
