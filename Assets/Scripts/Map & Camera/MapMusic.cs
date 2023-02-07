using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMusic : MonoBehaviour {
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            MusicManager.PlayMusic(transform.GetComponentInParent<MapGenerator>().GetMapMusic());
        }
    }
}
