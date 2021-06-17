using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WSB_Cable : MonoBehaviour
{
    [SerializeField] List<Transform> Points = new List<Transform>();
    [SerializeField] LineRenderer lineRenderer = null;



    private void Update()
    {
        lineRenderer.positionCount = Points.Count;
        for (int i = 0; i < Points.Count; i++)
        {
            lineRenderer.SetPosition(i, Points[i].position);
        }
    }
}
