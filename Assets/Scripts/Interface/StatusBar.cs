using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusBar : MonoBehaviour {

	Character attachedCharacter;
	Character player;
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

	void OnMouseDown(){
		GameObject characterGameObject = transform.parent.gameObject;
		if (characterGameObject.tag == "Enemy") {
			Debug.Log ("true");
			player.SetTarget(characterGameObject.GetComponent<Character>());
		}
	}

	// Update is called once per frame
	void Update () {
		if (player == null) {
			player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ();
		} else {
			UpdateTargeting ();
		}

		if (isDisplayed) {
			healthBar.fillAmount = attachedCharacter.GetHealthPercent ();
			resourceBar.fillAmount = attachedCharacter.GetResourcePercent ();
			castBar.fillAmount = attachedCharacter.GetCastPercent ();
		} 

		if (!attachedCharacter.IsInCombat ()) {
			removeSelf ();
		}
	}

	void UpdateTargeting(){
		if (player.GetTarget () == attachedCharacter) {
			SetTarget ();
		} else {
			UnSetTarget ();
		}
	}


	void LateUpdate(){
		Vector3 pos = GetComponent<RectTransform> ().localPosition;
		if (pos.y < 1) {
			pos.y = 1;
		}
		GetComponent<RectTransform> ().localPosition = new Vector3 (0, pos.y);

	}

	private void SetTarget(){
		GetComponent<Canvas> ().sortingOrder = 101;
		transform.Find("Selection1").GetComponent<Image> ().color = new Color (0, 0, 0, 0.70f);
		transform.Find("Selection2").GetComponent<Image> ().color = new Color (0, 0, 0, 0.70f);
		transform.Find("Selection3").GetComponent<Image> ().color = new Color (0, 0, 0, 0.70f);
		transform.Find("Selection4").GetComponent<Image> ().color = new Color (0, 0, 0, 0.70f);
	}

	private void UnSetTarget(){
		GetComponent<Canvas> ().sortingOrder = 100;
		transform.Find("Selection1").GetComponent<Image> ().color = new Color (0, 0, 0, 0);
		transform.Find("Selection2").GetComponent<Image> ().color = new Color (0, 0, 0, 0);
		transform.Find("Selection3").GetComponent<Image> ().color = new Color (0, 0, 0, 0);
		transform.Find("Selection4").GetComponent<Image> ().color = new Color (0, 0, 0, 0);
	}

	private void removeSelf(){
		GameObject.Destroy(this.gameObject);
	}
}
