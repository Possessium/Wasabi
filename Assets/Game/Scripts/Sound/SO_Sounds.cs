using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundBank", menuName = "ScriptableObjects/CreateSoundBank", order = 1)]
public class SO_Sounds : ScriptableObject
{
    #region Steps
    #region Ban
    [SerializeField] private List<AudioClip> banStepGrass = new List<AudioClip>();
    [SerializeField] private List<AudioClip> banStepMetal = new List<AudioClip>();
    [SerializeField] private List<AudioClip> banStepWood = new List<AudioClip>();

    [SerializeField] private List<AudioClip> banRunGrass = new List<AudioClip>();
    [SerializeField] private List<AudioClip> banRunMetal = new List<AudioClip>();
    [SerializeField] private List<AudioClip> banRunWood = new List<AudioClip>();

    [SerializeField] private List<AudioClip> banJumpGrass = new List<AudioClip>();
    [SerializeField] private List<AudioClip> banJumpMetal = new List<AudioClip>();
    [SerializeField] private List<AudioClip> banJumpWood = new List<AudioClip>();

    [SerializeField] private List<AudioClip> banLandGrass = new List<AudioClip>();
    [SerializeField] private List<AudioClip> banLandMetal = new List<AudioClip>();
    [SerializeField] private List<AudioClip> banLandWood = new List<AudioClip>();
    #endregion
    #region Lux

    [SerializeField] private List<AudioClip> luxStepGrass = new List<AudioClip>();
    [SerializeField] private List<AudioClip> luxStepMetal = new List<AudioClip>();
    [SerializeField] private List<AudioClip> luxStepWood = new List<AudioClip>();
                                             
    [SerializeField] private List<AudioClip> luxRunGrass = new List<AudioClip>();
    [SerializeField] private List<AudioClip> luxRunMetal = new List<AudioClip>();
    [SerializeField] private List<AudioClip> luxRunWood = new List<AudioClip>();
                                             
    [SerializeField] private List<AudioClip> luxJumpGrass = new List<AudioClip>();
    [SerializeField] private List<AudioClip> luxJumpMetal = new List<AudioClip>();
    [SerializeField] private List<AudioClip> luxJumpWood = new List<AudioClip>();
                                             
    [SerializeField] private List<AudioClip> luxLandGrass = new List<AudioClip>();
    [SerializeField] private List<AudioClip> luxLandMetal = new List<AudioClip>();
    [SerializeField] private List<AudioClip> luxLandWood = new List<AudioClip>();
    #endregion
    #endregion
    #region Powers
    [SerializeField] private AudioClip powerPickup = null;

    [SerializeField] private AudioClip shrinkActive = null;
    [SerializeField] private AudioClip shrinkDisable = null;

    [SerializeField] private AudioClip windActive = null;
    [SerializeField] private AudioClip windDisable = null;

    [SerializeField] private AudioClip trampoSpawn = null;
    [SerializeField] private AudioClip trampoDespawn = null;

    [SerializeField] private AudioClip dragonSpawn = null;
    [SerializeField] private AudioClip dragonDespawn = null;
    [SerializeField] private AudioClip dragonEat = null;
    #endregion
    #region Players
    [SerializeField] private AudioClip playerBounce = null;
    [SerializeField] private AudioClip lever = null;

    [SerializeField] private AudioClip luxShrink = null;
    [SerializeField] private AudioClip luxUnshrink = null;

    [SerializeField] private AudioClip validPattern = null;
    #endregion
    #region UI
    [SerializeField] private AudioClip button = null;
    [SerializeField] private AudioClip valid = null;
    [SerializeField] private AudioClip cancel = null;
    #endregion
    #region Ambiant
    [SerializeField] private AudioClip fountain = null;
    [SerializeField] private AudioClip elevator = null;
    #endregion
}
