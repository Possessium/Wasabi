using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WSB_LightSwitcher : MonoBehaviour
{
    [SerializeField] private Color targetColor = Color.white;
    [SerializeField] private float targetIntensity = 2;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<WSB_Lux>())
        {
            WSB_LightManager.I.ChangeColor(targetColor, targetIntensity, true);
        }

        if (collision.GetComponent<WSB_Ban>())
        {
            WSB_LightManager.I.ChangeColor(targetColor, targetIntensity, false);
        }
    }
}
