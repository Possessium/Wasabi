﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using TMPro;
using UnityEngine.UI;

public class WSB_Spells : MonoBehaviour
{

    [SerializeField] WSB_Player owner = null;

    [SerializeField] Image[] images = new Image[4];

    [SerializeField] TMP_Text[] texts = new TMP_Text[4];

    [SerializeField] float rotationSpeed = 2;

    int currentSpell = 0;
    Coroutine animateCoroutine = null;

    [SerializeField] long target = 0;
    [SerializeField] float z = 0;

    private void Awake()
    {
        // Checks owner, destroy itself if not found
        if(!owner)
        {
            Debug.LogError($"Erreur, component Wsb_Player manquant sur {transform.name} parent");
            Destroy(this);
        }

        // Checks Image components, destroy itself if not found
        if (!images[0] || !images[1] || !images[2] || !images[3])
        {
            Debug.LogError($"Erreur, component Image manquant sur {transform.name} parent");
            Destroy(this);
        }

        // Checks TMP_Text components, destroy itself if not found
        if (!texts[0] || !texts[1] || !texts[2] || !texts[3])
        {
            Debug.LogError($"Erreur, component TMP_Text manquant sur {transform.name} parent");
            Destroy(this);
        }

        // Sets the correct tags for each players
        if(owner.GetComponent<WSB_Ban>())
        {
            images[0].tag = "Earth";
            images[1].tag = "Wind";
            images[2].tag = "Light";
            images[3].tag = "Shrink";
        }
        else
        {
            currentSpell = 3;
            images[0].tag = "Ladder";
            images[1].tag = "Bridge";
            images[2].tag = "Carnivore";
            images[3].tag = "Trampoline";
        }
    }


    public void UseSpell(InputAction.CallbackContext _context)
    {
        // If input is canceled stop any current spell
        if (_context.canceled) 
            owner.StopSpell();

        // Exit if paused or input isn't started
        if (!_context.started || WSB_GameManager.Paused) return;

        owner.UseSpell(images[currentSpell].tag);
    }

    public void RotateSpells(InputAction.CallbackContext _context)
    {
        // Exit is paused or input isn't started
        if (!_context.started || WSB_GameManager.Paused)
            return;

        int _v = (int)_context.ReadValue<float>();

        // Increment the current spell and keep it between 0 and 3
        currentSpell += _v;
        if (currentSpell > 3)
            currentSpell = 0;
        if (currentSpell < 0)
            currentSpell = 3;

        // Adds to the target z rotation
        target += _v > 0 ? -90 : 90;

        if (animateCoroutine != null)
            StopCoroutine(animateCoroutine);

        animateCoroutine = StartCoroutine(AnimateWheel(_v > 0));

    }

    IEnumerator AnimateWheel(bool _right)
    {
        // Rotates the wheel toward the correct z target
        while (true)
        {
            if (!_right && z < target)
            {
                z = Mathf.Lerp(z, target, Time.deltaTime * (rotationSpeed * 10));
                transform.eulerAngles = new Vector3(0, 0, z);
            }

            else if (_right && z > target)
            {
                z = Mathf.Lerp(z, target, Time.deltaTime * (rotationSpeed * 10));
                transform.eulerAngles = new Vector3(0, 0, z);
            }

            yield return new WaitForEndOfFrame();
        }
    }

    public void UpdateChargesUI(SpellType _type, string _value)
    {
        switch (_type)
        {
            case SpellType.Earth:
            case SpellType.Ladder:
                texts[1].text = _value.ToString();
                break;
            case SpellType.Wind:
            case SpellType.Bridge:
                texts[2].text = _value.ToString();
                break;
            case SpellType.Light:
            case SpellType.Carnivore:
                texts[3].text = _value.ToString();
                break;
            case SpellType.Shrink:
            case SpellType.Trampoline:
                texts[0].text = _value.ToString();
                break;
        }
    }

    public void UpdateEmptyCharges(SpellType _type, float _value)
    {
        switch (_type)
        {
            case SpellType.Earth:
            case SpellType.Ladder:
                images[1].fillAmount = _value;
                break;
            case SpellType.Wind:
            case SpellType.Bridge:
                images[2].fillAmount = _value;
                break;
            case SpellType.Light:
            case SpellType.Carnivore:
                images[3].fillAmount = _value;
                break;
            case SpellType.Shrink:
            case SpellType.Trampoline:
                images[0].fillAmount = _value;
                break;
        }
    }
}

public enum SpellType
{
    Earth,
    Wind,
    Light,
    Shrink,
    Ladder,
    Bridge,
    Trampoline,
    Carnivore
}