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
		fontSize = 200;
	}

    private void Update()
    {
        if (transform.parent != null)
        {

            if (transform.parent.localScale.x < 0 && transform.localScale.x > 0)
            {
                FlipTransform();
            }
            if (transform.parent.localScale.x > 0 && transform.localScale.x < 0)
            {
                FlipTransform();
            }
        }
    }

    private void FlipTransform()
    {
        transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
    }


    public void setText(string text){
		this.text = text;	
	}

	public void setColor(Color color){
		this.color = color;
	}
    
}
