using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ControllerValues", menuName = "ScriptableObjects/CreateControllerValues", order = 1)]
public class SO_ControllerValues : ScriptableObject
{
    [Tooltip("Contact filter to know what must collide with it")]
    public ContactFilter2D Contact = new ContactFilter2D();

    [Tooltip("Minimum y normal for object to stay on it")]
    public float GroundMin = .5f;

    [Tooltip("Max height for object to climb without jumping")]
    public float MaxHeightClimb = .5f;

    [Tooltip("Speed coeficient for object movement")]
    public float SpeedCoef = 1;

    [Tooltip("Climb speed when on a ladder")]
    public float ClimbSpeed = 5;

    [Tooltip("Deceleration value used when stopping")]
    public float Deceleration = 10;

    [Tooltip("Delay for Coyote jump")]
    public float JumpDelay = .2f;

    [Tooltip("Delay for Coyote jump")]
    public float JumpBufferDelay = .2f;

    [Tooltip("Curve of the jump of the object")]
    public AnimationCurve JumpCurve = new AnimationCurve();

    [Tooltip("Layer of the object that repulse player when an object is currently grabbed")]
    public LayerMask ContactGrabLayer = 0;
    
    [Tooltip("Curve of the acceleration of the object")]
    public AnimationCurve SpeedCurve = new AnimationCurve();

}
