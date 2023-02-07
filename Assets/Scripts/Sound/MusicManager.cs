using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {

    private static MusicManager instance;
	private AudioSource audioSource;
	public AudioClip StartingMusic;

    private bool isFading = false;

    public void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        instance = this;
		if(StartingMusic != null){
			PlayMusic(StartingMusic);
		}
    }
    

	public static void PlayMusic(AudioClip music){

		if (music != instance.audioSource.clip && instance.audioSource.isPlaying && !instance.isFading)
        {
            instance.isFading = true;
            instance.StartCoroutine(fadeOutThenFadeIn(music, 1f));
        } else
        {
            instance.StartCoroutine(fadeIn(music,1f));
        }
        if (!instance.audioSource.isPlaying)
        {
            instance.audioSource.clip = music;
            instance.audioSource.Play();
        }
	}
    

	private static IEnumerator fadeIn(AudioClip music, float fadeTime)
    {
        instance.isFading = false;
        instance.audioSource.clip = music;
        if (!instance.isFading && !instance.audioSource.isPlaying)
        {
            instance.audioSource.Play();
        }
        while (!instance.isFading && instance.audioSource.volume < 1f)
        {
            instance.audioSource.volume +=  Time.deltaTime / fadeTime;

            yield return null;
        }

    }

    private static IEnumerator fadeOutThenFadeIn(AudioClip music, float fadeTime)
    {
        while (instance.isFading && instance.audioSource.volume > 0.1)
        {
            instance.audioSource.volume -= Time.deltaTime / fadeTime;

            yield return null;
        }
        yield return null;
        if (instance.isFading) { 
            instance.StartCoroutine(fadeIn(music, fadeTime));
        }
    }
}
