using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "SoundBank", menuName = "ScriptableObjects/CreateSoundBank", order = 1)]
public class SO_Sounds : ScriptableObject
{
    [SerializeField] private List<AudioClip> musics = new List<AudioClip>();
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
    [SerializeField] private List<AudioClip> playerBounce = null;
    [SerializeField] private AudioClip lever = null;

    [SerializeField] private AudioClip luxShrink = null;
    [SerializeField] private AudioClip luxUnshrink = null;

    [SerializeField] private AudioClip validPattern = null;
    #endregion
    #region UI
    [SerializeField] private AudioClip button1 = null;
    [SerializeField] private AudioClip button2 = null;
    [SerializeField] private AudioClip buttonStart = null;
    [SerializeField] private AudioClip valid = null;
    [SerializeField] private AudioClip cancel = null;
    #endregion
    #region Ambiant
    [SerializeField] private AudioClip fountain = null;
    [SerializeField] private AudioClip elevator = null;
    [SerializeField] private AudioClip cog = null;
    #endregion
    #region Mixers
    [SerializeField] private AudioMixerGroup footstepsMixer = null;
    [SerializeField] private AudioMixerGroup clickMixer = null;
    [SerializeField] private AudioMixerGroup spellsMixer = null;
    [SerializeField] private AudioMixerGroup objectsMixer = null;
    [SerializeField] private AudioMixerGroup spawnMixer = null;
    [SerializeField] private AudioMixerGroup mechaMixer = null;
    #endregion

    public AudioClip GetClip(Sound type)
    {
        AudioClip _clip = null;
        switch (type)
        {
            case Sound.WalkBanDirt:
                _clip = banStepGrass[Random.Range(0, banStepGrass.Count - 1)];
                break;
            case Sound.WalkBanWood:
                _clip = banStepWood[Random.Range(0, banStepWood.Count - 1)];
                break;
            case Sound.WalkBanMetal:
                _clip = banStepMetal[Random.Range(0, banStepMetal.Count - 1)];
                break;
            case Sound.WalkLuxDirt:
                _clip = luxStepGrass[Random.Range(0, luxStepGrass.Count - 1)];
                break;
            case Sound.WalkLuxWood:
                _clip = luxStepWood[Random.Range(0, luxStepWood.Count - 1)];
                break;
            case Sound.WalkLuxMetal:
                _clip = luxStepMetal[Random.Range(0, luxStepMetal.Count - 1)];
                break;
            case Sound.RunBanDirt:
                _clip = banRunGrass[Random.Range(0, banRunGrass.Count - 1)];
                break;
            case Sound.RunBanWood:
                _clip = banRunWood[Random.Range(0, banRunWood.Count - 1)];
                break;
            case Sound.RunBanMetal:
                _clip = banRunMetal[Random.Range(0, banRunMetal.Count - 1)];
                break;
            case Sound.RunLuxDirt:
                _clip = luxRunGrass[Random.Range(0, luxRunGrass.Count - 1)];
                break;
            case Sound.RunLuxWood:
                _clip = luxRunWood[Random.Range(0, luxRunWood.Count - 1)];
                break;
            case Sound.RunLuxMetal:
                _clip = luxRunMetal[Random.Range(0, luxRunMetal.Count - 1)];
                break;
            case Sound.JumpBanDirt:
                _clip = banJumpGrass[Random.Range(0, banJumpGrass.Count - 1)];
                break;
            case Sound.JumpBanWood:
                _clip = banJumpWood[Random.Range(0, banJumpWood.Count - 1)];
                break;
            case Sound.JumpBanMetal:
                _clip = banJumpMetal[Random.Range(0, banJumpMetal.Count - 1)];
                break;
            case Sound.JumpLuxDirt:
                _clip = luxJumpGrass[Random.Range(0, luxJumpGrass.Count - 1)];
                break;
            case Sound.JumpLuxWood:
                _clip = luxJumpWood[Random.Range(0, luxJumpWood.Count - 1)];
                break;
            case Sound.JumpLuxMetal:
                _clip = luxJumpMetal[Random.Range(0, luxJumpMetal.Count - 1)];
                break;
            case Sound.LandBanDirt:
                _clip = banLandGrass[Random.Range(0, banLandGrass.Count - 1)];
                break;
            case Sound.LandBanWood:
                _clip = banLandWood[Random.Range(0, banLandWood.Count - 1)];
                break;
            case Sound.LandBanMetal:
                _clip = banLandMetal[Random.Range(0, banLandMetal.Count - 1)];
                break;
            case Sound.LandLuxDirt:
                _clip = luxLandGrass[Random.Range(0, luxLandGrass.Count - 1)];
                break;
            case Sound.LandLuxWood:
                _clip = luxLandWood[Random.Range(0, luxLandWood.Count - 1)];
                break;
            case Sound.LandLuxMetal:
                _clip = luxLandMetal[Random.Range(0, luxLandMetal.Count - 1)];
                break;
            case Sound.PowerPickup:
                _clip = powerPickup;
                break;
            case Sound.ShrinkActive:
                _clip = shrinkActive;
                break;
            case Sound.ShrinkDisable:
                _clip = shrinkDisable;
                break;
            case Sound.WindActive:
                _clip = windActive;
                break;
            case Sound.WindDisable:
                _clip = windDisable;
                break;
            case Sound.TrampoSpawn:
                _clip = trampoSpawn;
                break;
            case Sound.TrampoDespawn:
                _clip = trampoDespawn;
                break;
            case Sound.DragonSpawn:
                _clip = dragonSpawn;
                break;
            case Sound.DragonDespawn:
                _clip = dragonDespawn;
                break;
            case Sound.DragonEat:
                _clip = dragonEat;
                break;
            case Sound.PlayerBounce:
                _clip = playerBounce[Random.Range(0, playerBounce.Count - 1)]; ;
                break;
            case Sound.Lever:
                _clip = lever;
                break;
            case Sound.LuxShrink:
                _clip = luxShrink;
                break;
            case Sound.LuxUnshrink:
                _clip = luxUnshrink;
                break;
            case Sound.ValidPattern:
                _clip = validPattern;
                break;
            case Sound.Fountain:
                _clip = fountain;
                break;
            case Sound.Elevator:
                _clip = elevator;
                break;
            case Sound.Button1:
                _clip = button1;
                break;
            case Sound.Button2:
                _clip = button2;
                break;
            case Sound.ButtonStart:
                _clip = buttonStart;
                break;
            case Sound.Valid:
                _clip = valid;
                break;
            case Sound.Cancel:
                _clip = cancel;
                break;
            case Sound.Cog:
                _clip = cog;
                break;
        }
        return _clip;
    }

    public AudioClip GetMusic(int _i) => musics[_i];

    public AudioMixerGroup GetMixer(Mixer _m)
    {
        AudioMixerGroup _g = null;
        switch (_m)
        {
            case Mixer.Footsteps:
                _g = footstepsMixer;
                break;
            case Mixer.Click:
                _g = clickMixer;
                break;
            case Mixer.Spells:
                _g = spellsMixer;
                break;
            case Mixer.Objects:
                _g = objectsMixer;
                break;
            case Mixer.Spawn:
                _g = spawnMixer;
                break;
            case Mixer.Mecha:
                _g = mechaMixer;
                break;
        }
        return _g;
    }
}

public enum Sound
{
    WalkBanDirt,
    WalkBanWood,
    WalkBanMetal,
    WalkLuxDirt,
    WalkLuxWood,
    WalkLuxMetal,
    RunBanDirt,
    RunBanWood,
    RunBanMetal,
    RunLuxDirt,
    RunLuxWood,
    RunLuxMetal,
    JumpBanDirt,
    JumpBanWood,
    JumpBanMetal,
    JumpLuxDirt,
    JumpLuxWood,
    JumpLuxMetal,
    LandBanDirt,
    LandBanWood,
    LandBanMetal,
    LandLuxDirt,
    LandLuxWood,
    LandLuxMetal,
    PowerPickup,
    ShrinkActive,
    ShrinkDisable,
    WindActive,
    WindDisable,
    TrampoSpawn,
    TrampoDespawn,
    DragonSpawn,
    DragonDespawn,
    DragonEat,
    PlayerBounce,
    Lever,
    LuxShrink,
    LuxUnshrink,
    ValidPattern,
    Fountain,
    Elevator,
    Button1,
    Button2,
    ButtonStart,
    Valid,
    Cancel,
    Cog
}

public enum Mixer
{
    Footsteps,
    Click,
    Spells,
    Objects,
    Spawn,
    Mecha
}