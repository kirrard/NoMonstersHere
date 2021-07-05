using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip audioClip;

    public static AudioController instance;

    private void Awake()
    {
        instance = this;
    }

    public void SetAudioSource(string name)
    {
        audioClip = ResourcesCache.GetObject(name) as AudioClip;
        audioSource.clip = audioClip;
    }

    public void PlayAudio()
    {
        audioSource.Play();
    }

    public void StopAudio()
    {
        audioSource.Stop();
    }

    public void SetLoop(SfxLoop condition)
    {
        if (condition == SfxLoop.On)
            audioSource.loop = true;
        else
            audioSource.loop = false;
    }

    public void SetVolume(float value)
    {
        audioSource.volume = value;
    }
}
