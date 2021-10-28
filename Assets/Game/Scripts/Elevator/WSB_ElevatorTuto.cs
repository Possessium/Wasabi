using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WSB_ElevatorTuto : MonoBehaviour
{
    [SerializeField] private Animator animator = null;
    [SerializeField] private WSB_SceneLoader sceneLoader = null;

    private static readonly int animate_Hash = Animator.StringToHash("Animate");

    private int count = 0;

    private bool scenesLoaded = false;

    public void RegisterSocle()
    {
        count++;
        if (count >= 2)
            TriggerElevator(true);
    }

    private void ScenesLoaded()
    {
        WSB_GameManager.I.ChangeMusic(2);
        sceneLoader.OnScenesReady -= ScenesLoaded;
        sceneLoader.enabled = false;
        TriggerElevator(true);
    }

    private void TriggerElevator(bool _s = true)
    {
        if(!scenesLoaded && _s)
        {
            WSB_Power[] _powers = FindObjectsOfType<WSB_Power>();
            foreach (WSB_Power _pow in _powers)
            {
                _pow.Lock(true);
            }

            if (sceneLoader)
            {
                scenesLoaded = true;
                sceneLoader.OnScenesReady += ScenesLoaded;
                sceneLoader.NextScene();
            }
            return;
        }

        if (!animator)
            return;

        if(_s)
        {
            WSB_SoundManager.I.Elevator(transform);
            animator.SetTrigger(animate_Hash);
            animator.speed = 1;
        }
        else
        {
            WSB_SoundManager.I.StopSound(transform);
            animator.speed = 0;
        }
    }

    public void RemoveSocle()
    {
        count--;
        TriggerElevator(false);
    }
}
