using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrollGameOverScreen : MonoBehaviour {

	SpriteRenderer spRenderer;
	float alpha = 0;

	// Use this for initialization
	void Start () {
        AudioClip deadMusic = (AudioClip)Resources.Load("Musics/Spirit");
        MusicManager.PlayMusic(deadMusic);
        spRenderer = this.GetComponent<SpriteRenderer> ();
		spRenderer.color = new Color(1,1,1,alpha);
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.position = new Vector3(Camera.main.gameObject.transform.position.x, Camera.main.gameObject.transform.position.y, 0);
        if (spRenderer.color.a < 0.7f) {
			alpha += Time.deltaTime / 4;
			spRenderer.color = new Color(1,1,1,alpha);
		}
	}
}
