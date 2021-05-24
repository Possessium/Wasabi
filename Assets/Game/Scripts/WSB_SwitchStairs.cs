using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WSB_SwitchStairs : MonoBehaviour
{
    [SerializeField] private GameObject leftStair = null;
    [SerializeField] private Material leftStairMat = null;
    [SerializeField] private GameObject rightStair = null;
    [SerializeField] private Material rightStairMat = null;
    [SerializeField] private Color enabledColor = Color.white;
    [SerializeField] private Color disabledColor = Color.gray;

    bool left = false;

    private void Start()
    {
        if (leftStair)
            leftStairMat = leftStair.GetComponentInChildren<MeshRenderer>().material;
        if (rightStair)
            rightStairMat = rightStair.GetComponentInChildren<MeshRenderer>().material;
    }

    private void Update()
    {
        if(left)
        {
            leftStair.transform.position = Vector3.MoveTowards(leftStair.transform.position, new Vector3(leftStair.transform.position.x, leftStair.transform.position.y, 0), Time.deltaTime * 5);
            rightStair.transform.position = Vector3.MoveTowards(rightStair.transform.position, new Vector3(rightStair.transform.position.x, rightStair.transform.position.y, 5), Time.deltaTime * 5);
            leftStairMat.color = Color.Lerp(leftStairMat.color, enabledColor, Time.deltaTime * 5);
            rightStairMat.color = Color.Lerp(rightStairMat.color, disabledColor, Time.deltaTime * 5);
        }
        else
        {
            rightStair.transform.position = Vector3.MoveTowards(rightStair.transform.position, new Vector3(rightStair.transform.position.x, rightStair.transform.position.y, 0), Time.deltaTime * 5);
            leftStair.transform.position = Vector3.MoveTowards(leftStair.transform.position, new Vector3(leftStair.transform.position.x, leftStair.transform.position.y, 5), Time.deltaTime * 5);
            rightStairMat.color = Color.Lerp(rightStairMat.color, enabledColor, Time.deltaTime * 5);
            leftStairMat.color = Color.Lerp(leftStairMat.color, disabledColor, Time.deltaTime * 5);
        }
    }


    public void SwitchMaterial(bool _left) => left = _left;


}
