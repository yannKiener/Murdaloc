using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interface : MonoBehaviour {
	Player player;
	Character target;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player>();
		Debug.Log (player.GetName ());
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnGUI()
	{
		target = player.GetTarget ();

		GUI.Box (new Rect (0, 0, 200, 20), name);
		GUI.Box (new Rect (0, 20, 200, 20), player.GetCurrentLife() + " / " + player.GetStats().MaxLife);
		GUI.Box(new Rect(0,20,(int) ((float)player.GetCurrentLife()/(float)player.GetStats().MaxLife*200),20), new Texture2D(1,1)); 
		GUI.Box (new Rect (0, 40, 200, 20), player.GetCurrentResource() + " / " + player.GetStats().MaxResource);
		GUI.Box(new Rect(0,40,(int) ((float)player.GetCurrentResource()/(float)player.GetStats().MaxResource*200),20), new Texture2D(1,1)); 

		if (target != null && !target.IsDead()) {
			GUI.Box (new Rect (400, 0, 200, 20), target.GetName());
			GUI.Box (new Rect (400, 20, 200, 20), target.GetCurrentLife() + " / " + target.GetMaxLife());
			GUI.Box (new Rect (400, 20, (int) ((float)target.GetCurrentLife()/(float)target.GetMaxLife()*200), 20), new Texture2D(1,1));
			GUI.Box (new Rect (400, 40, 200, 20), target.GetCurrentResource() + " / " + target.GetMaxResource());
			GUI.Box(new Rect(400,40,(int) ((float)target.GetCurrentResource()/(float)target.GetMaxResource()*200),20), new Texture2D(1,1)); 

			//test outlining target
			if (target.GetGameObject ().GetComponent<cakeslice.Outline> () == null) {
				target.GetGameObject ().AddComponent<cakeslice.Outline> ();
			}
		}

		if (player.IsCasting()) {
			float spellCastTime = player.GetCastingSpell().GetCastTime (player.GetStats());
			if (spellCastTime != 0) {

				GUI.Box (new Rect (Screen.width / 2, 93	* Screen.height / 100, 100, 30), player.GetCastingSpell().GetName () + " : " + player.GetCastingTime().ToString ("0.0") + " / " + spellCastTime);
				GUI.Box (new Rect (Screen.width / 2, 93 * Screen.height / 100, player.GetCastPercent()*100, 30), new Texture2D (1, 1)); 
			}
		}

	}
}
