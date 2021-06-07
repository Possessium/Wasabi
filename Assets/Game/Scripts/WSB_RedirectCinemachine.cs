using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WSB_RedirectCinemachine : MonoBehaviour
{
    [SerializeField] WSB_TriggerCam triggerCam = null;


    public void AnimationEnded() => triggerCam.AnimationEnded();
}
