using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    
    public SoundData soundData;
    
    public AudioSource oneShotSounds;
    
    private void Awake()
    {
        Instance = this;
    }

    public void PlayOneShot(SoundType type, float volumeScale = 0)
    {
        if(!DataManager.GetSoundStatus()) return;
        
        if (!soundData.Sounds.ContainsKey(type))
            return;
        if (volumeScale != 0)
            oneShotSounds.PlayOneShot(soundData.Sounds[type], volumeScale: volumeScale);
        else
            oneShotSounds.PlayOneShot(soundData.Sounds[type]);
    }
    public void PlayOneShot(AudioClip clip, float volumeScale = 0)
    {
        if(!DataManager.GetSoundStatus()) return;
        
        if (volumeScale != 0)
            oneShotSounds.PlayOneShot(clip, volumeScale: volumeScale);
        else
            oneShotSounds.PlayOneShot(clip);
    }

    private readonly Dictionary<SoundType, AudioSource> tempAudioSources = new();
    public void PlayLoop(SoundType type, float volumeScale = 1)
    {
        if (!soundData.Sounds.TryGetValue(type, out var sound))
            return;
        if (tempAudioSources.ContainsKey(type))
        {
            tempAudioSources[type].volume = volumeScale;
            tempAudioSources[type].enabled = true;
        }
        else
        {
            var audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = sound;
            audioSource.volume = volumeScale;
            audioSource.loop = true;
            audioSource.mute = !DataManager.GetMusicStatus();
            audioSource.Play();
            tempAudioSources.Add(type, audioSource);
        }
    }

    public void StopLoop(SoundType type)
    {
        if (tempAudioSources.TryGetValue(type, out var source))
        {
            source.enabled = false;
        }
    }

    public void BGMVolume(bool haveSound)
    {
        foreach (var _audio in tempAudioSources)
        {
            _audio.Value.mute = !haveSound;
        }
    }
}