using UnityEngine;
using UnityEngine.Audio;

public class PlayAudioSnapOnActive : MonoBehaviour
{
    public AudioMixerSnapshot audioSnap;


    void OnEnable()
    {
        if (audioSnap != null)
        {
            audioSnap.TransitionTo(1);
        }
    }
}
