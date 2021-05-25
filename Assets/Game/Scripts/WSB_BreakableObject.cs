using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WSB_BreakableObject : MonoBehaviour
{
    public UnityEvent CallBack = null;

    [SerializeField] GameObject destroyFX = null;

    private void OnDestroy()
    {
        if (destroyFX)
            Instantiate(destroyFX, transform.position, Quaternion.identity);

        CallBack?.Invoke();
    }
}
