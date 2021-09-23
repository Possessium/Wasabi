using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WSB_LightManager : MonoBehaviour
{
    public static WSB_LightManager I { get; private set; }

    [SerializeField] private Light luxLight = null;
    [SerializeField] private Light luxLightBack = null;
    [SerializeField] private Light banLight = null;
    [SerializeField] private Light banLightBack = null;

    [SerializeField] private float moveColorSpeed = 2;

    private Coroutine luxCoroutine = null;
    private Coroutine banCoroutine = null;

    private void Awake() => I = this;

    public void ChangeColor(Color _c, bool _lux)
    {
        if(_lux)
        {
            if (luxCoroutine != null)
                StopCoroutine(luxCoroutine);

            luxCoroutine = StartCoroutine(ChangeColor(true, _c));
        }
        else
        {
            if (banCoroutine != null)
                StopCoroutine(banCoroutine);

            banCoroutine = StartCoroutine(ChangeColor(false, _c));
        }
    }

    IEnumerator ChangeColor(bool _lux, Color _c)
    {
        Color _tempColor = Color.white;
        Color _tempColorBack = Color.white;
        if (_lux)
        {
            while(luxLight.color != _c || luxLightBack.color != _c)
            {
                _tempColor = luxLight.color;
                _tempColorBack = luxLightBack.color;
                luxLight.color = new Color(
                    Mathf.MoveTowards(_tempColor.r, _c.r, Time.deltaTime * moveColorSpeed),
                    Mathf.MoveTowards(_tempColor.g, _c.g, Time.deltaTime * moveColorSpeed),
                    Mathf.MoveTowards(_tempColor.b, _c.b, Time.deltaTime * moveColorSpeed),
                    Mathf.MoveTowards(_tempColor.a, _c.a, Time.deltaTime * moveColorSpeed));
                luxLightBack.color = new Color(
                    Mathf.MoveTowards(_tempColorBack.r, _c.r, Time.deltaTime * moveColorSpeed),
                    Mathf.MoveTowards(_tempColorBack.g, _c.g, Time.deltaTime * moveColorSpeed),
                    Mathf.MoveTowards(_tempColorBack.b, _c.b, Time.deltaTime * moveColorSpeed),
                    Mathf.MoveTowards(_tempColorBack.a, _c.a, Time.deltaTime * moveColorSpeed));
                yield return new WaitForEndOfFrame();
            }
        }
        else
        {
            while(banLight.color != _c)
            {
                _tempColor = banLight.color;
                _tempColorBack = banLightBack.color;
                banLight.color = new Color(
                    Mathf.MoveTowards(_tempColor.r, _c.r, Time.deltaTime * moveColorSpeed),
                    Mathf.MoveTowards(_tempColor.g, _c.g, Time.deltaTime * moveColorSpeed),
                    Mathf.MoveTowards(_tempColor.b, _c.b, Time.deltaTime * moveColorSpeed),
                    Mathf.MoveTowards(_tempColor.a, _c.a, Time.deltaTime * moveColorSpeed));
                banLightBack.color = new Color(
                    Mathf.MoveTowards(_tempColorBack.r, _c.r, Time.deltaTime * moveColorSpeed),
                    Mathf.MoveTowards(_tempColorBack.g, _c.g, Time.deltaTime * moveColorSpeed),
                    Mathf.MoveTowards(_tempColorBack.b, _c.b, Time.deltaTime * moveColorSpeed),
                    Mathf.MoveTowards(_tempColorBack.a, _c.a, Time.deltaTime * moveColorSpeed));
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
