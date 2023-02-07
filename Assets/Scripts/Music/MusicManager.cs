using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour{
	private static AudioClip music1;
	private static AudioClip music2;
	private static AudioSource source;

	public static void playMusic(AudioClip music){
		if (source == null) {
			source = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<AudioSource> ();
		}
		if (music != source.clip) {
			//StartCoroutine (fadeIn(1f));
			//fade out, then new audioclip, then fadeIn.
		} else {
			//fade in
		}
		source.clip = music;
	}

	private static IEnumerator fadeIn(float time){
		return null;
	}

	private static IEnumerator fadeOut(float time){
		return null;
	}
}
