using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class WSB_SoundPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource source = null;
    [SerializeField] private float minDist = 3;
    [SerializeField] private float maxDist = 50;
    [SerializeField] private float maxVolume = 1;

    private Transform p1 = null;
    private Transform p2 = null;
    [SerializeField] private Transform obj = null;

    private void Start()
    {
        if(WSB_Lux.I)
            p1 = WSB_Lux.I?.transform;
        if(WSB_Ban.I)
            p2 = WSB_Ban.I?.transform;
    }

    private void Update()
    {
        if (!source.isPlaying)
            Stop();

        if(p1 && p2 && obj)
        {
            float distP1 = Vector2.Distance(p1.position, obj.position);
            float distP2 = Vector2.Distance(p2.position, obj.position);

            if (distP1 > maxDist && distP2 > maxDist)
                source.volume = 0;

            else if (distP1 > minDist && distP2 < minDist)
                source.volume = maxVolume;

            else
            {
                float dist = Mathf.Min(distP1, distP2);

                source.volume = ((dist - maxDist) / (minDist - maxDist)) * maxVolume;
            }
        }
    }

    public void Init(Transform _obj, AudioClip _clip, AudioMixerGroup _m = null, bool _loop = false)
    {
        if(_m)
            source.outputAudioMixerGroup = _m;

        source.clip = _clip;
        source.loop = _loop;
        source.enabled = true;
        obj = _obj;
    }

    public void Stop()
    {
        Destroy(this.gameObject);
    }
}
