using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMusicOnCollide : MonoBehaviour
{
    public AudioClip music;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            MusicManager.PlayMusic(music);
        }
    }
}
