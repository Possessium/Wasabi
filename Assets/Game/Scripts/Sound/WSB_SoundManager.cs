using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WSB_SoundManager : MonoBehaviour
{
    public static WSB_SoundManager I { get; private set; }

    [SerializeField] private AudioSource musicSource = null;
    [SerializeField] private GameObject soundSource = null;
    [SerializeField] private AudioClip test = null;
    private void Awake()
    {
        I = this;
    }

    public void Walk(bool _ban, GroundType _t)
    {

    }
    public void Run(bool _ban, GroundType _t)
    {

    }
    public void Jump(bool _ban, GroundType _t)
    {

    }
    public void Land(bool _ban, GroundType _t)
    {

    }
    
    public void Lift(bool _ban)
    {

    }
    public void Lever(bool _ban)
    {

    }

    public void ShrinkLux()
    {

    }
    public void UnshrinkLux()
    {

    }

    public void TrampoBounce(bool _ban)
    {

    }

    public void SpawnDragon(Transform _t)
    {

    }
    public void DespawnDragon(Transform _t)
    {

    }
    public void DragonEat(Transform _t)
    {

    }

    public void WindActive(Transform _t)
    {

    }
    public void WindDisable(Transform _t)
    {

    }

    public void Elevator(Transform _t)
    {

    }

    public void ValidPattern()
    {

    }
}
public enum GroundType
{
    Grass,
    Wood,
    Metal
}

/*
 * 
 * pour les loop, faire return un audio source spawné et avoir une méthode de cancel qui prend un audiosource et qui sera donné depuis l'appel du cancel (ex : wind sur stoppower) & faire en fonction de la distance ossi
 * 
 */