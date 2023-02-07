using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SoundManager 
{

    static float volume = 0;
    
    public static void PlaySound(AudioClip sound)
    {
        if(sound != null)
        {
            AudioSource audioSource = FindUtils.GetSoundsGameObject().AddComponent<AudioSource>();
            audioSource.volume = volume;
            audioSource.PlayOneShot(sound, 1);
            audioSource.clip = sound;
            GameObject.Destroy(audioSource, sound.length);
        }
    }

    public static void PlaySound(List<AudioClip> sounds)
    {
        if (sounds != null)
        {
            PlaySound(GetRandomClip(sounds));
        }
    }

    public static void SetVolume(float vol)
    {
        volume = vol;
    }

    public static float GetVolume()
    {
        return volume;
    }

    public static void StopAll()
    {
        GameObject sounds = FindUtils.GetSoundsGameObject();
        foreach (AudioSource audio in sounds.GetComponents<AudioSource>())
        {
            GameObject.Destroy(audio);
        }
    }

    public static void StopSound(AudioClip sound)
    {
        if(sound != null)
        {
            List<AudioSource> audioSources = FindUtils.GetSoundsGameObject().GetComponents<AudioSource>().ToList<AudioSource>();
            foreach (AudioSource audioSource in audioSources)
            {
                if (audioSource.clip != null && audioSource.clip == sound)
                {
                    audioSource.Stop();
                    GameObject.Destroy(audioSource);
                }
            }
        }
    }

    private static AudioClip GetRandomClip(List<AudioClip> audioClips)
    {
        if(audioClips != null && audioClips.Count > 0)
        {
            return audioClips[Random.Range(0, audioClips.Count)];
        } else
        {
            return null;
        }
    }

}
