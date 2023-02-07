using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusBar : MonoBehaviour {

	Character attachedCharacter;
	Image healthBar;
	Image resourceBar;
	Image castBar;	
	bool isDisplayed = true;

	// Use this for initialization
	void Start () {
		attachedCharacter = transform.parent.gameObject.GetComponent<Character> ();
		healthBar = this.transform.Find ("HealthBackGrnd").Find ("HealthBarFiller").GetComponent<Image> ();
		resourceBar = this.transform.Find ("ResourceBackGrnd").Find ("ResourceBarFiller").GetComponent<Image> ();
		castBar = this.transform.Find ("CastBackGrnd").Find ("CastBarFiller").GetComponent<Image> ();
	}
	
	// Update is called once per frame
	void Update () {

		if (isDisplayed) {
			healthBar.fillAmount = attachedCharacter.GetHealthPercent ();
			resourceBar.fillAmount = attachedCharacter.GetResourcePercent ();
			castBar.fillAmount = attachedCharacter.GetCastPercent ();
		} 

		if (!attachedCharacter.IsInCombat ()) {
			removeSelf ();
		}
	}


	void LateUpdate(){
		Vector3 pos = GetComponent<RectTransform> ().localPosition;
		if (pos.y < 1) {
			pos.y = 1;
		}
		GetComponent<RectTransform> ().localPosition = new Vector3 (0, pos.y);

	}

	private void removeSelf(){
		GameObject.Destroy(this.gameObject);
	}
}
