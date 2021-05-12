using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ControllerValues", menuName = "ScriptableObjects/CreateControllerValues", order = 1)]
public class SO_ControllerValues : ScriptableObject
{
    [Tooltip("Delay for Coyote jump")]
    public float JumpDelay = .2f;

    [Tooltip("Delay for Coyote jump")]
    public float JumpBufferDelay = .2f;

    [Tooltip("Curve of the jump of the object")]
    public AnimationCurve JumpCurve = new AnimationCurve();

    [Tooltip("Layer of the object that repulse player when an object is currently grabbed")]
    public LayerMask ContactGrabLayer = 0;
    
}
