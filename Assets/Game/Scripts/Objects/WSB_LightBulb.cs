using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WSB_LightBulb : MonoBehaviour
{
    [SerializeField] private bool activeOnStart = false;

    [SerializeField] private Color activeColor = Color.green;
    [SerializeField] private Color deactiveColor = Color.red;

    [SerializeField] private Transform link = null;
    [SerializeField] private Transform bulb = null;
    [SerializeField] private MeshRenderer meshRenderer = null;
    [SerializeField] private Light bulbLight = null;

    private Material material = null;

    private void Start()
    {
        material = meshRenderer.material;


        if (activeOnStart)
            ActivateBulb();
        else
            DeactivateBulb();
    }

    private void Update()
    {
        bulb.LookAt(link, Vector3.back);
    }

    public void ActivateBulb()
    {
        material.SetColor("_EmissionColor", activeColor);
        bulbLight.color = activeColor;
    }

    public void DeactivateBulb()
    {
        material.SetColor("_EmissionColor", deactiveColor);
        bulbLight.color = deactiveColor;
    }
}