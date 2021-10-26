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
                SpawnSound().Init(_ban ? ban : lux, soundsData.GetClip(_ban ? Sound.WalkBanDirt : Sound.WalkLuxDirt));
                break;
            case GroundType.Wood:
                SpawnSound().Init(_ban ? ban : lux, soundsData.GetClip(_ban ? Sound.WalkBanWood : Sound.WalkLuxWood));
                break;
            case GroundType.Metal:
                SpawnSound().Init(_ban ? ban : lux, soundsData.GetClip(_ban ? Sound.WalkBanMetal : Sound.WalkLuxMetal));
                break;
        }
    }
    public void Run(bool _ban, GroundType _t)
    {
        switch (_t)
        {
            case GroundType.Grass:
                SpawnSound().Init(_ban ? ban : lux, soundsData.GetClip(_ban ? Sound.RunBanDirt : Sound.RunLuxDirt));
                break;
            case GroundType.Wood:
                SpawnSound().Init(_ban ? ban : lux, soundsData.GetClip(_ban ? Sound.RunBanWood : Sound.RunLuxWood));
                break;
            case GroundType.Metal:
                SpawnSound().Init(_ban ? ban : lux, soundsData.GetClip(_ban ? Sound.RunBanMetal : Sound.RunLuxMetal));
                break;
        }
    }
    public void Jump(bool _ban, GroundType _t)
    {
        switch (_t)
        {
            case GroundType.Grass:
                SpawnSound().Init(_ban ? ban : lux, soundsData.GetClip(_ban ? Sound.JumpBanDirt : Sound.JumpLuxDirt));
                break;
            case GroundType.Wood:
                SpawnSound().Init(_ban ? ban : lux, soundsData.GetClip(_ban ? Sound.JumpBanWood : Sound.JumpLuxWood));
                break;
            case GroundType.Metal:
                SpawnSound().Init(_ban ? ban : lux, soundsData.GetClip(_ban ? Sound.JumpBanMetal : Sound.JumpLuxMetal));
                break;
        }
    }
    public void Land(bool _ban, GroundType _t)
    {
        switch (_t)
        {
            case GroundType.Grass:
                SpawnSound().Init(_ban ? ban : lux, soundsData.GetClip(_ban ? Sound.LandBanDirt : Sound.LandLuxDirt));
                break;
            case GroundType.Wood:
                SpawnSound().Init(_ban ? ban : lux, soundsData.GetClip(_ban ? Sound.LandBanWood : Sound.LandLuxWood));
                break;
            case GroundType.Metal:
                SpawnSound().Init(_ban ? ban : lux, soundsData.GetClip(_ban ? Sound.LandBanMetal : Sound.LandLuxMetal));
                break;
        }
    }
    
    public void Lift(bool _ban) => SpawnSound().Init(_ban ? ban : lux, soundsData.GetClip(Sound.PowerPickup));
    public void Lever(Transform _t) => SpawnSound().Init(_t, soundsData.GetClip(Sound.Lever));

    #region Powers
    public void TrampoBounce(Transform _t) => SpawnSound().Init(_t, soundsData.GetClip(Sound.PlayerBounce));
    public void TrampoSpawn(Transform _t) => SpawnSound().Init(_t, soundsData.GetClip(Sound.TrampoSpawn));
    public void TrampoDespawn(Transform _t) => SpawnSound().Init(_t, soundsData.GetClip(Sound.TrampoDespawn));

    public void ShrinkSpawn(Transform _t) => SpawnSound().Init(_t, soundsData.GetClip(Sound.ShrinkActive));
    public void ShrinkDespawn(Transform _t) => SpawnSound().Init(_t, soundsData.GetClip(Sound.ShrinkDisable));

    public void ShrinkLux() => SpawnSound().Init(lux, soundsData.GetClip(Sound.LuxUnshrink));
    public void UnshrinkLux() => SpawnSound().Init(lux, soundsData.GetClip(Sound.LuxShrink));

    public void SpawnDragon(Transform _t) => SpawnSound().Init(_t, soundsData.GetClip(Sound.DragonSpawn));
    public void DespawnDragon(Transform _t) => SpawnSound().Init(_t, soundsData.GetClip(Sound.DragonDespawn));
    public void DragonEat(Transform _t) => SpawnSound().Init(_t, soundsData.GetClip(Sound.DragonEat));

    public void WindActive(Transform _t) => SpawnSound().Init(_t, soundsData.GetClip(Sound.WindActive));
    public void WindDisable(Transform _t) => SpawnSound().Init(_t, soundsData.GetClip(Sound.WindDisable));
    #endregion

    public void Elevator(Transform _t) => SpawnSound().Init(_t, soundsData.GetClip(Sound.Elevator), true);
    public void StopSound(Transform _t)
    {
        if (_t.GetComponentInChildren<WSB_SoundPlayer>())
            _t.GetComponentInChildren<WSB_SoundPlayer>().Stop();
    }

    public void ValidPattern() => SpawnSound().Init(transform, soundsData.GetClip(Sound.ValidPattern));

    WSB_SoundPlayer SpawnSound(Transform _t = null)
    {
        GameObject _go = Instantiate(soundSource, transform.position, Quaternion.identity);
        
        if (_t)
            _go.transform.parent = _t;

        return _go.GetComponent<WSB_SoundPlayer>();
    }

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
