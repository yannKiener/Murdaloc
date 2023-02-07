using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrollGameOverScreen : MonoBehaviour {

	SpriteRenderer spRenderer;
	float alpha = 0;
    bool isFreeSpiritRunning = false;
    AudioClip deadMusic;

    // Use this for initialization
    void Start () {
        deadMusic = (AudioClip)Resources.Load("Musics/Spirit");
        MusicManager.PlayMusic(deadMusic);
        spRenderer = this.GetComponent<SpriteRenderer> ();
		spRenderer.color = new Color(1,1,1,alpha);
        Time.timeScale = 0.6f;
    }
	
	// Update is called once per frame
	void Update ()
    {
        transform.position = new Vector3(Camera.main.gameObject.transform.position.x, Camera.main.gameObject.transform.position.y, 0);

        MusicManager.PlayMusic(deadMusic);
        if (spRenderer.color.a < 0.7f) {
			alpha += Time.deltaTime / 4;
			spRenderer.color = new Color(1,1,1,alpha);
		}

        if (!isFreeSpiritRunning)
        {
            StartCoroutine(FreeSpirit());
        }
    }

    IEnumerator FreeSpirit()
    {
        isFreeSpiritRunning = true;
        yield return new WaitForSecondsRealtime(5);
        Time.timeScale = 1;

        Action resetDefaultsAction = new Action(() => GameUtils.ReloadScene());
        Interface.DrawModalDialog("Load checkpoint ?", resetDefaultsAction);
        isFreeSpiritRunning = false;
    }
}
