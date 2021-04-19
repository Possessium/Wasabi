using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;
using System;

public class WSB_Ban : WSB_Player
{
    public static WSB_Ban I { get; private set; }

    //[SerializeField] WSB_Spells spells = null;

    //#region Spell Charges
    //[SerializeField] int maxShrinkCharges = 10;
    //[SerializeField] int maxWindCharges = 10;
    //[SerializeField] int maxLightCharges = 10;

    //int shrinkCharges = 10;
    //int windCharges = 10;
    //int lightCharges = 10;
    //#endregion

    //#region Light Spell
    //[SerializeField] GameObject lightObject = null;

    //[SerializeField] float lightChargeDelay = 10;
    //#endregion

    //#region Shrink Spell
    //[SerializeField] float shrinkChargeDelay = 10;
    //#endregion

    //#region Wind Spell
    //[SerializeField] float windRange = 5;
    //[SerializeField] float windPower = 2;
    //[SerializeField] float windChargeDelay = 10;

    //Coroutine blowCoroutine = null;


    //[SerializeField] LayerMask windLayer = 0;

    //#endregion

    //#region RechargeSpells
    //float shrinkTimer = 0;
    //float lightTimer = 0;
    //float windTimer = 0;

    //int rechargeShrink = 0;
    //int rechargeLight = 0;
    //bool rechargeWind = false;
    //#endregion

    // Sets instance of this object
    private void Awake()
    {
        I = this;
    }

    // Setup variables and events
    public override void Start()
    {
        base.Start();

        WSB_GameManager.OnUpdate += MyUpdate;
        //WSB_GameManager.OnPause += StopSpell;

        //windCharges = maxWindCharges;
        //lightCharges = maxLightCharges;
        //shrinkCharges = maxShrinkCharges;

        //spells.UpdateChargesUI(SpellType.Light, lightCharges.ToString());
        //spells.UpdateChargesUI(SpellType.Shrink, shrinkCharges.ToString());
        //spells.UpdateChargesUI(SpellType.Wind, windCharges.ToString());
    }

    public override void Update()
    {
        // Has to be here and empty to override Unity update and use MyUpdate below
    }
    //  |
    //  |
    //  V
    // Update called on bound event
    void MyUpdate()
    {
        base.Update();

        //if(rechargeLight > 0)
        //{
        //    lightTimer += Time.deltaTime;

        //    if (lightCharges == 0)
        //        spells.UpdateEmptyCharges(SpellType.Light, lightTimer / lightChargeDelay);

        //    if (lightTimer >= lightChargeDelay)
        //    {
        //        rechargeLight--;

        //        lightTimer = 0;

        //        lightCharges++;
        //        if (lightCharges > maxLightCharges)
        //            lightCharges = maxLightCharges;

        //        spells.UpdateChargesUI(SpellType.Light, lightCharges.ToString());
        //        spells.UpdateEmptyCharges(SpellType.Light, 1);

        //        if (lightCharges == maxLightCharges)
        //            rechargeLight = 0;
        //    }
        //}
        //if(rechargeShrink > 0)
        //{
        //    shrinkTimer += Time.deltaTime;

        //    if (shrinkCharges == 0)
        //        spells.UpdateEmptyCharges(SpellType.Shrink, shrinkTimer / shrinkChargeDelay);

        //    if (shrinkTimer >= shrinkChargeDelay)
        //    {
        //        rechargeShrink--;

        //        shrinkTimer = 0;

        //        shrinkCharges++;
        //        if (shrinkCharges > maxShrinkCharges)
        //            shrinkCharges = maxShrinkCharges;

        //        spells.UpdateChargesUI(SpellType.Shrink, shrinkCharges.ToString());
        //        spells.UpdateEmptyCharges(SpellType.Shrink, 1);

        //        if (shrinkCharges == maxShrinkCharges)
        //            rechargeShrink = 0;
        //    }
        //}
        //if(rechargeWind)
        //{
        //    windTimer += Time.deltaTime;

        //    if (windCharges == 0)
        //        spells.UpdateEmptyCharges(SpellType.Wind, windTimer / windChargeDelay);

        //    if (windTimer >= windChargeDelay)
        //    {
        //        windTimer = 0;

        //        windCharges++;
        //        if (windCharges > maxWindCharges)
        //            windCharges = maxWindCharges;

        //        spells.UpdateChargesUI(SpellType.Wind, windCharges.ToString());
        //        spells.UpdateEmptyCharges(SpellType.Wind, 1);

        //        if (windCharges== maxWindCharges)
        //            rechargeWind = false;
        //    }
        //}
    }



    //public override void UseSpell(string _s)
    //{
    //    base.UseSpell(_s);
    //    if (WSB_GameManager.Paused)
    //        return;

    //    // Search for corresponding spell and calls it
    //    if (_s == "Light")
    //        Light();

    //    else if (_s == "Shrink") 
    //        Shrink();

    //    else if (_s == "Wind" && windCharges > 0) 
    //        Wind();
    //}

    //public override void StopSpell()
    //{
    //    base.StopSpell();

    //    // Stops blow method if in use
    //    if (blowCoroutine != null)
    //    {
    //        if (playerAnimator)
    //            playerAnimator.SetBool("SpellLoop", false);
    //        StopCoroutine(blowCoroutine);
    //        CanMove = true;
    //        rechargeWind = true;
    //    }
    //}


    //#region Shrink
    //void Shrink()
    //{
    //    // Checks if Ban has enough charges to do it
    //    if (shrinkCharges == 0 && !WSB_Lux.I.Shrinked)
    //        return;

    //    if(playerAnimator)
    //        playerAnimator.SetTrigger("Spell");

    //    // Ask Lux to shrink, stop here if he can't
    //    if (!WSB_Lux.I.Shrink(out bool _canShrink))
    //    {
    //        if (!_canShrink)
    //            return;

    //        rechargeShrink++;
    //        return;
    //    }

    //    // Reduce shrink charges and update corresponding UI
    //    shrinkCharges--;
    //    spells.UpdateChargesUI(SpellType.Shrink, shrinkCharges.ToString());
    //    if (shrinkCharges == 0)
    //        spells.UpdateEmptyCharges(SpellType.Shrink, shrinkTimer);
    //}

    //#endregion

    //#region Light

    //[SerializeField] LayerMask lightLayer = 0;

    //void Light()
    //{
    //    // Search and retrieve the first active light found to disable it and recharge
    //    RaycastHit2D[] _hits = new RaycastHit2D[1];
    //    _hits = Physics2D.CircleCastAll(transform.position, 5, Vector2.right, 1, lightLayer);
    //    if(_hits.Length > 0 && _hits[0])
    //    {
    //        Destroy(_hits[0].transform.gameObject);
    //        rechargeLight++;
    //        if (playerAnimator)
    //            playerAnimator.SetTrigger("Spell");
    //        return;
    //    }

    //    // Checks if Ban has enough charges to do it
    //    // Return is Ban is not grounded
    //    if (lightCharges == 0 || !isGrounded)
    //        return;

    //    if (playerAnimator)
    //        playerAnimator.SetTrigger("Spell");

    //    // Reduce earth charges and update corresponding UI
    //    lightCharges--;
    //    spells.UpdateChargesUI(SpellType.Light, lightCharges.ToString());
    //    if (lightCharges == 0)
    //        spells.UpdateEmptyCharges(SpellType.Light, lightTimer);

    //    // Create the light and start both Movement and Time coroutines
    //    GameObject _light = Instantiate(lightObject, new Vector3(transform.position.x,transform.position.y,-5), Quaternion.identity);
    //    StartCoroutine(MoveLight(_light, _light.transform.position + Vector3.up * 2));
    //}

    //IEnumerator MoveLight(GameObject _light, Vector3 _target)
    //{
    //    // Moves the light towards its target
    //    while (_light && Vector3.Distance(_light.transform.position, _target) != 0)
    //    {
    //        // Hold if game is in pause
    //        while (WSB_GameManager.Paused)
    //        {
    //            yield return new WaitForSeconds(.2f);
    //        }

    //        _light.transform.position = Vector3.MoveTowards(_light.transform.position, _target, Time.deltaTime * 2);
    //        yield return new WaitForEndOfFrame();
    //    }
    //}

    //#endregion

    //#region Wind
    //void Wind()
    //{
    //    // Checks if Ban has enough charges to do it
    //    if (windCharges == 0 || !isGrounded)
    //        return;

    //    // Reduce wind charges and update corresponding UI
    //    windCharges--;
    //    spells.UpdateChargesUI(SpellType.Wind, windCharges.ToString());
    //    if (windCharges == 0)
    //        spells.UpdateEmptyCharges(SpellType.Wind, windTimer);

    //    rechargeWind = false;
    //    if (playerAnimator)
    //    {
    //        playerAnimator.SetBool("SpellLoop", true);
    //        playerAnimator.SetTrigger("Wind");
    //    }
    //    blowCoroutine = StartCoroutine(Blow());
    //}

    //Collider2D hit;
    //RaycastHit2D[] checkPlayerOn = new RaycastHit2D[10];
    //LG_Movable physics;
    //[SerializeField] LayerMask stopWindSight = 0;

    //IEnumerator Blow()
    //{
    //    CanMove = false;

    //    // Runs until coroutine is canceled
    //    while(true)
    //    {
    //        // Hold if game is in pause
    //        while (WSB_GameManager.Paused)
    //        {
    //            yield return new WaitForSeconds(.2f);
    //        }

    //        // Find all corresponding objects in range
    //        Collider2D[] _hits = Physics2D.OverlapBoxAll(transform.position, Vector2.one * windRange, 0, windLayer);

    //        // Loops through found objects
    //        for (int i = 0; i < _hits.Length; i++)
    //        {
    //            hit = _hits[i];

    //            if (hit == collider || hit == WSB_Lux.I.MovableCollider)
    //                continue;

    //            checkPlayerOn = new RaycastHit2D[10];
    //            collider.Cast(Vector2.down, checkPlayerOn, 1);
    //            if (checkPlayerOn.Any(r => r && r.collider == hit))
    //                continue;

    //            checkPlayerOn = new RaycastHit2D[10];
    //            WSB_Lux.I.MovableCollider.Cast(Vector2.down, checkPlayerOn, 1);
    //            if (checkPlayerOn.Any(r => r && r.collider == hit))
    //                continue;

    //            // if(raycast(pos, dir(pos, _hits.pos)) pas gêné, blow
    //            Vector2 _dir = hit.transform.position - transform.position;

    //            if (Physics2D.Raycast(transform.position, _dir.normalized, Vector2.Distance(transform.position, hit.transform.position), stopWindSight))
    //                continue;

    //            // Gets physic of hit object
    //            physics = hit.gameObject.GetComponent<LG_Movable>();

    //            if (physics)
    //            {
    //                // Checks if object is a pot and try to break the seed in it
    //                if (hit.GetComponent<WSB_Pot>() && physics.CanMove)
    //                {
    //                    hit.GetComponent<WSB_Pot>().BreakSeed();
    //                }

    //                // Add vertical force on the physic of the object
    //                physics.AddForce(Vector2.up * windPower);
    //            }
    //        }
    //        yield return new WaitForEndOfFrame();
    //    }
    //}


    //#endregion

}
