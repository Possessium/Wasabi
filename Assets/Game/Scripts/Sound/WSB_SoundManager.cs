using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WSB_SoundManager : MonoBehaviour
{
    public static WSB_SoundManager I { get; private set; }

    [SerializeField] private SO_Sounds soundsData = null;

    [SerializeField] private AudioSource musicSource = null;
    [SerializeField] private AudioSource ambiantSource = null;
    [SerializeField] private GameObject soundSource = null;

    private Transform ban = null;
    private Transform lux = null;

    private void Awake()
    {
        I = this;
    }

    private void Start()
    {
        if (WSB_Ban.I)
            ban = WSB_Ban.I.transform;
        if (WSB_Lux.I)
            lux = WSB_Lux.I.transform;
    }


    public void ChangeMusicVolume(float f) => musicSource.outputAudioMixerGroup.audioMixer.SetFloat("VolumeMusic", f);
    public void ChangeSoundVolume(float f) => musicSource.outputAudioMixerGroup.audioMixer.SetFloat("VolumeSound", f);
    public float GetMusicVolume()
    {
        float _f = 0;
        musicSource.outputAudioMixerGroup.audioMixer.GetFloat("VolumeMusic", out _f);
        return _f;
    }
    public float GetSoundVolume()
    {
        float _f = 0;
        musicSource.outputAudioMixerGroup.audioMixer.GetFloat("VolumeSound", out _f);
        return _f;
    }
    public void ChangeMusic(int _i)
    {
        musicSource.Stop();
        musicSource.clip = soundsData.GetMusic(_i);
        musicSource.Play();
    }

    public void Walk(bool _ban, GroundType _t)
    {
        switch (_t)
        {
            case GroundType.Grass:
                SpawnSound().Init(_ban ? ban : lux, soundsData.GetClip(_ban ? Sound.WalkBanDirt : Sound.WalkLuxDirt), soundsData.GetMixer(Mixer.Footsteps));
                break;
            case GroundType.Wood:
                SpawnSound().Init(_ban ? ban : lux, soundsData.GetClip(_ban ? Sound.WalkBanWood : Sound.WalkLuxWood), soundsData.GetMixer(Mixer.Footsteps));
                break;
            case GroundType.Metal:
                SpawnSound().Init(_ban ? ban : lux, soundsData.GetClip(_ban ? Sound.WalkBanMetal : Sound.WalkLuxMetal), soundsData.GetMixer(Mixer.Footsteps));
                break;
        }
    }
    public void Run(bool _ban, GroundType _t)
    {
        switch (_t)
        {
            case GroundType.Grass:
                SpawnSound().Init(_ban ? ban : lux, soundsData.GetClip(_ban ? Sound.RunBanDirt : Sound.RunLuxDirt), soundsData.GetMixer(Mixer.Footsteps));
                break;
            case GroundType.Wood:
                SpawnSound().Init(_ban ? ban : lux, soundsData.GetClip(_ban ? Sound.RunBanWood : Sound.RunLuxWood), soundsData.GetMixer(Mixer.Footsteps));
                break;
            case GroundType.Metal:
                SpawnSound().Init(_ban ? ban : lux, soundsData.GetClip(_ban ? Sound.RunBanMetal : Sound.RunLuxMetal), soundsData.GetMixer(Mixer.Footsteps));
                break;
        }
    }
    public void Jump(bool _ban, GroundType _t)
    {
        switch (_t)
        {
            case GroundType.Grass:
                SpawnSound().Init(_ban ? ban : lux, soundsData.GetClip(_ban ? Sound.JumpBanDirt : Sound.JumpLuxDirt), soundsData.GetMixer(Mixer.Footsteps));
                break;
            case GroundType.Wood:
                SpawnSound().Init(_ban ? ban : lux, soundsData.GetClip(_ban ? Sound.JumpBanWood : Sound.JumpLuxWood), soundsData.GetMixer(Mixer.Footsteps));
                break;
            case GroundType.Metal:
                SpawnSound().Init(_ban ? ban : lux, soundsData.GetClip(_ban ? Sound.JumpBanMetal : Sound.JumpLuxMetal), soundsData.GetMixer(Mixer.Footsteps));
                break;
        }
    }
    public void Land(bool _ban, GroundType _t)
    {
        switch (_t)
        {
            case GroundType.Grass:
                SpawnSound().Init(_ban ? ban : lux, soundsData.GetClip(_ban ? Sound.LandBanDirt : Sound.LandLuxDirt), soundsData.GetMixer(Mixer.Footsteps));
                break;
            case GroundType.Wood:
                SpawnSound().Init(_ban ? ban : lux, soundsData.GetClip(_ban ? Sound.LandBanWood : Sound.LandLuxWood), soundsData.GetMixer(Mixer.Footsteps));
                break;
            case GroundType.Metal:
                SpawnSound().Init(_ban ? ban : lux, soundsData.GetClip(_ban ? Sound.LandBanMetal : Sound.LandLuxMetal), soundsData.GetMixer(Mixer.Footsteps));
                break;
        }
    }
    
    public void Lift(bool _ban) => SpawnSound().Init(_ban ? ban : lux, soundsData.GetClip(Sound.PowerPickup), soundsData.GetMixer(Mixer.Objects));
    public void Lever(Transform _t) => SpawnSound().Init(_t, soundsData.GetClip(Sound.Lever), soundsData.GetMixer(Mixer.Objects));

    #region Powers
    public void TrampoBounce(Transform _t) => SpawnSound().Init(_t, soundsData.GetClip(Sound.PlayerBounce), soundsData.GetMixer(Mixer.Spells));
    public void TrampoSpawn(Transform _t) => SpawnSound().Init(_t, soundsData.GetClip(Sound.TrampoSpawn), soundsData.GetMixer(Mixer.Spawn));
    public void TrampoDespawn(Transform _t) => SpawnSound().Init(_t, soundsData.GetClip(Sound.TrampoDespawn), soundsData.GetMixer(Mixer.Spawn));

    public void ShrinkSpawn(Transform _t) => SpawnSound().Init(_t, soundsData.GetClip(Sound.ShrinkActive), soundsData.GetMixer(Mixer.Spawn));
    public void ShrinkDespawn(Transform _t) => SpawnSound().Init(_t, soundsData.GetClip(Sound.ShrinkDisable), soundsData.GetMixer(Mixer.Spawn));

    public void ShrinkLux() => SpawnSound().Init(lux, soundsData.GetClip(Sound.LuxUnshrink), soundsData.GetMixer(Mixer.Spells));
    public void UnshrinkLux() => SpawnSound().Init(lux, soundsData.GetClip(Sound.LuxShrink), soundsData.GetMixer(Mixer.Spells));

    public void SpawnDragon(Transform _t) => SpawnSound().Init(_t, soundsData.GetClip(Sound.DragonSpawn), soundsData.GetMixer(Mixer.Spawn));
    public void DespawnDragon(Transform _t) => SpawnSound().Init(_t, soundsData.GetClip(Sound.DragonDespawn), soundsData.GetMixer(Mixer.Spawn));
    public void DragonEat(Transform _t) => SpawnSound().Init(_t, soundsData.GetClip(Sound.DragonEat), soundsData.GetMixer(Mixer.Spells));

    public void WindActive(Transform _t) => SpawnSound().Init(_t, soundsData.GetClip(Sound.WindActive), soundsData.GetMixer(Mixer.Vent));
    public void WindDisable(Transform _t) => SpawnSound().Init(_t, soundsData.GetClip(Sound.WindDisable), soundsData.GetMixer(Mixer.Vent));
    #endregion

    public void Elevator(Transform _t) => SpawnSound().Init(_t, soundsData.GetClip(Sound.Elevator), soundsData.GetMixer(Mixer.Mecha), true);
    public void StopSound(Transform _t)
    {
        if (_t.GetComponentInChildren<WSB_SoundPlayer>())
            _t.GetComponentInChildren<WSB_SoundPlayer>().Stop();
    }

    public void ValidPattern() => SpawnSound().Init(transform, soundsData.GetClip(Sound.ValidPattern), soundsData.GetMixer(Mixer.Click));

    WSB_SoundPlayer SpawnSound(Transform _t = null)
    {
        GameObject _go = Instantiate(soundSource, transform.position, Quaternion.identity);
        
        if (_t)
            _go.transform.parent = _t;

        return _go.GetComponent<WSB_SoundPlayer>();
    }


    public void Button1() => SpawnSound().Init(null, soundsData.GetClip(Sound.Button1), soundsData.GetMixer(Mixer.Click));
    public void Button2() => SpawnSound().Init(null, soundsData.GetClip(Sound.Button2), soundsData.GetMixer(Mixer.Click));
    public void ButtonStart() => SpawnSound().Init(null, soundsData.GetClip(Sound.ButtonStart), soundsData.GetMixer(Mixer.Click));
    public void Cog(Transform _t) => SpawnSound(_t).Init(_t, soundsData.GetClip(Sound.Cog), soundsData.GetMixer(Mixer.Mélenchon));

    public void StartAmbiant()
    {
        ambiantSource.Play();
    }
    public void StopAmbiant()
    {
        StartCoroutine(LowerAmbiant());
    }

    IEnumerator LowerAmbiant()
    {
        while(ambiantSource.volume > .1f)
        {
            ambiantSource.volume = Mathf.MoveTowards(ambiantSource.volume, 0, Time.deltaTime * .5f);
            yield return new WaitForEndOfFrame();
        }
        ambiantSource.Stop();
    }
}
public enum GroundType
{
    Grass,
    Wood,
    Metal
}
