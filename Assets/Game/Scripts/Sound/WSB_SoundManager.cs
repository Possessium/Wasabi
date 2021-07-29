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

    #region Characters
    public void Step(bool _ban)
    {

    }

    public void Lift(bool _ban)
    {

    }

    public void Drop(bool _ban)
    {

    }

    public void Jump(bool _ban)
    {

    }

    public void Land(bool _ban)
    {

    }

    #endregion
}

/*
 * 
 * pour les loop, faire return un audio source spawné et avoir une méthode de cancel qui prend un audiosource et qui sera donné depuis l'appel du cancel (ex : wind sur stoppower) & faire en fonction de la distance ossi
 * 
 */