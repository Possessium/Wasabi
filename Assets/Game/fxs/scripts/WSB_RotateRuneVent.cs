using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WSB_RotateRuneVent : MonoBehaviour
{
    [SerializeField] 
        private float speed = 0f;
    [SerializeField] 
        private bool rotate = false;

    private void Update()
    {
        if (rotate)
            RotateIt();
    }

    private void RotateIt()
    {
        transform.Rotate(new Vector3(0, 1, 0) * Time.deltaTime * speed);
    }
}