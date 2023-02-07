using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingText : MonoBehaviour {
	GameObject textGameObj;
	Text textComponent;
	int fontSize = 120;
	string text;
	Color color;

	// Use this for initialization
	void Start () {
		textGameObj = this.transform.Find ("Text").gameObject;
		textComponent = textGameObj.GetComponent<Text> ();
		float clipLength = textGameObj.GetComponent<Animator> ().GetCurrentAnimatorClipInfo (0) [0].clip.length;
		textComponent.text = text;
		textComponent.fontSize = fontSize;
		textComponent.color = color;
		Destroy (this.gameObject, clipLength);
	}

	public void SetCrit ()
	{	
		fontSize = 250;
	}

	public void setText(string text){
		this.text = text;	
	}

	public void setColor(Color color){
		this.color = color;
	}

	// Update is called once per frame
	void Update () {
		
	}
}
