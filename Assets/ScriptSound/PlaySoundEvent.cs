using AK.Wwise;
using UnityEngine;


public class PlaySoundEvent : MonoBehaviour
{
    public AK.Wwise.Event[] myEvents;

    public string WwiseVariable;

    [Header("ONLY ON FOOTSTEP SOUND")]
    public WSB_PlayerMovable movement;
  

    // Use this for initialization.
    public void PlayWwiseEvent(int index)
    {
        myEvents[index].Post(gameObject);
    }

    public void PlayAllWwiseEvents()
    {
        foreach (AK.Wwise.Event WwiseEvent in myEvents)
        {
            WwiseEvent.Post(gameObject);
        }
    }

    public void PlayAllWwiseEvents(GameObject objectToPlaceSoundsIn)
    {
        foreach (AK.Wwise.Event WwiseEvent in myEvents)
        {
            WwiseEvent.Post(objectToPlaceSoundsIn);
        }
    }

    public void StopAllWwiseEvents()
    {
        foreach (AK.Wwise.Event WwiseEvent in myEvents)
        {
            WwiseEvent.Stop(gameObject);
        }
    }

    public void StopAllWwiseEvents(GameObject objectThatHasSoundsIn)
    {
        foreach (AK.Wwise.Event WwiseEvent in myEvents)
        {
            WwiseEvent.Stop(objectThatHasSoundsIn);
        }
    }

    public void FootStep(AnimationEvent evt)
    {
        //to remove with good character
       
        AkSoundEngine.SetSwitch("Footstep_Action", "Walk", gameObject);
        movement.FootstepSound(gameObject);
        
        //AkSoundEngine.SetRTPCValue(WwiseVariable, movement.GetSpeed(), gameObject);
        MyAnimationEventCallback(evt);
    }

    void MyAnimationEventCallback(AnimationEvent evt)
    {
        if (evt.animatorClipInfo.weight > 0.5f)
        {
            foreach (AK.Wwise.Event WwiseEvent in myEvents)
            {
                WwiseEvent.Post(gameObject);
            }
            // Debug.Log("eventPlayed");
        }
    }

 
    


  

}
