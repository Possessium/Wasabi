using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WSB_Elevator : MonoBehaviour
{
    [SerializeField] GameObject pivotLeft = null;
    [SerializeField] GameObject pivotRight = null;
    [SerializeField] LineRenderer line = null;
    [SerializeField] Animator animator = null;
    [SerializeField] WSB_BreakableObject destroyEvent = null;

    private void Start()
    {
        WSB_GameManager.I.RegisterElevator(animator);
        destroyEvent.CallBack.AddListener(WSB_GameManager.I.ElevatorRepaired);
    }

    private void Update()
    {
        line.SetPosition(1, new Vector3(pivotLeft.transform.position.x, pivotLeft.transform.position.y, -5));
        line.SetPosition(2, new Vector3(pivotRight.transform.position.x, pivotRight.transform.position.y, -5));
    }


}
