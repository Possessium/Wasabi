using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WSB_BreakableObject : MonoBehaviour
{
    public UnityEvent CallBack = null;

    [SerializeField] GameObject destroyFX = null;
    [SerializeField] private Vector2 position = Vector2.zero;
    private bool isQuitting = false;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere((Vector2)transform.position + position, .2f);
    }

    private void OnDestroy()
    {
        if (isQuitting)
            return;

        if (destroyFX)
            Instantiate(destroyFX, (Vector2)transform.position + position, Quaternion.identity);

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
