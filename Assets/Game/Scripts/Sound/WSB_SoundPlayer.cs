using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WSB_SoundPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource source = null;
    [SerializeField] private float minDist = 3;
    [SerializeField] private float maxDist = 50;

    private Transform p1 = null;
    private Transform p2 = null;
    private Transform obj = null;

    private void Start()
    {
        p1 = WSB_Lux.I?.transform;
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
                source.volume = 1;

            else
            {
                float dist = Mathf.Min(distP1, distP2);

                source.volume = (dist - maxDist) / (minDist - maxDist);
            }
        }
    }

    public void Init(Transform _obj, AudioClip _clip, bool _loop = false)
    {
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
