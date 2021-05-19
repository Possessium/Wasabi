using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WSB_BreakableObject : MonoBehaviour
{
    public UnityEvent CallBack = null;

    [SerializeField] GameObject destroyFX = null;
    private bool isQuitting = false;

    private void OnDestroy()
    {
        if (isQuitting)
            return;

        if (destroyFX)
            Instantiate(destroyFX, transform.position, Quaternion.identity);

        CallBack?.Invoke();
    }

    private void OnDisable()
    {
        isQuitting = true;
    }

    private void OnApplicationQuit()
    {
        isQuitting = true;
    }
}
