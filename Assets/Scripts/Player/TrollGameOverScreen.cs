using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrollGameOverScreen : MonoBehaviour {

	SpriteRenderer spRenderer;
	float alpha = 0;

	// Use this for initialization
	void Start () {
		spRenderer = this.GetComponent<SpriteRenderer> ();
		spRenderer.color = new Color(1,1,1,alpha);
		transform.position = new Vector3 ( Camera.main.gameObject.transform.position.x, Camera.main.gameObject.transform.position.y,0);;
	}
	
	// Update is called once per frame
	void Update () {
		if (spRenderer.color.a < 0.7f) {
			alpha += Time.deltaTime / 4;
			spRenderer.color = new Color(1,1,1,alpha);
		}
	}
}
