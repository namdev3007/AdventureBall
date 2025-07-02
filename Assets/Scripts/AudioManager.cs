using UniRx;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : SingletonDontDestroy<AudioManager>
{
    const string KEY_MUSIC = "MUSIC";
    const string KEY_SOUND = "SOUND";
    const string KEY_VIBRATION = "VIBRATION";

    public AudioMixer audioMixed;
    public AudioMixerSnapshot snapshotOff;
    public AudioMixerSnapshot snapshotOn;

    public AudioSource audioSourceClick;
    public AudioSource audioSourceHome;

    [Range(0.001f, 1)]
    public float x;

    public AudioSource audioSourceE;

    public AudioSource audioSourceCoin;

    private void OnEnable()
    {
        RxManager.playAudioE.Subscribe(audioClip => PlayAudioE(audioClip));
        RxManager.audioSnapOnOff.Subscribe(isOn => OnAudioSnapOnOff(isOn));
        RxManager.playAudioCoin.Subscribe(audioClipCoin => PlayAudioCoin(audioClipCoin));
    }

    public void ActiveAudioHome(bool isActive)
    {
        audioSourceHome.gameObject.SetActive(isActive);
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey(KEY_MUSIC))
        {
            SetMusic(0);
        }
        else
        {
            SetMusic(1);
        }

        if (PlayerPrefs.HasKey(KEY_SOUND))
        {
            SetSoundEffect(0);
        }
        else
        {
            SetSoundEffect(1);
        }

        if (PlayerPrefs.HasKey(KEY_VIBRATION))
        {

        }
        else
        {

        }

        RxManager.audioClick.Subscribe(_ => playClickAudio());

    }

    public void playClickAudio()
    {
        audioSourceClick.Play();
    }

    public void SetMusicOnorOff()
    {
        if (PlayerPrefs.HasKey(KEY_MUSIC))
        {
            SetMusic(1);
            PlayerPrefs.DeleteKey(KEY_MUSIC);
        }
        else
        {
            PlayerPrefs.SetInt(KEY_MUSIC, 1);
            SetMusic(0);
        }

        PlayerPrefs.Save();
    }


    public void SetSoundOnorOff()
    {
        if (PlayerPrefs.HasKey(KEY_SOUND))
        {
            SetSoundEffect(1);
            PlayerPrefs.DeleteKey(KEY_SOUND);
        }
        else
        {
            PlayerPrefs.SetInt(KEY_SOUND, 1);
            SetSoundEffect(0);
        }

        PlayerPrefs.Save();
    }

    public void SetVibrationOnorOff()
    {
        if (PlayerPrefs.HasKey(KEY_VIBRATION))
        {
            PlayerPrefs.DeleteKey(KEY_VIBRATION);
        }
        else
        {
            PlayerPrefs.SetInt(KEY_VIBRATION, 1);
        }

        PlayerPrefs.Save();
    }


    void PlayAudioE(AudioClip _audioClip)
    {
        audioSourceE.clip = _audioClip;
        audioSourceE.Play();
    }

    void PlayAudioCoin(AudioClip _audioClip)
    {
        audioSourceCoin.PlayOneShot(_audioClip);
    }


    private void SetAudio(string nameVolum, float volum)
    {
        audioMixed.SetFloat(nameVolum, Mathf.Log10(Mathf.Clamp(volum, 0.001f, 1)) * 20);
    }

    public void SetMusic(float volum)
    {
        SetAudio("MusicVolum", volum);
    }

    public void SetSoundEffect(float volum)
    {
        SetAudio("SoundVolum", volum);
    }

    void OnAudioSnapOnOff(bool isOn)
    {
        if (isOn)
        {
            if (snapshotOn != null)
            {
                snapshotOn.TransitionTo(1);
            }
        }
        else
        {
            if (snapshotOff != null)
            {
                snapshotOff.TransitionTo(1);
            }
        }

    }
}
