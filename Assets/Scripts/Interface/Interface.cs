using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interface : MonoBehaviour {
	static Player player;
	static Character target;
	public GUIStyle nameBarStyle;
	public GUIStyle healthBarStyle;
	public GUIStyle resourceBarStyle;
	public GUIStyle backgroundStyle;


	public static void LoadPlayer(){
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player>();
	}


	// Use this for initialization
	void Start () {
		nameBarStyle.normal.background = getTextureWithColor(new Color (0, 0, 0, 0.7f));
		healthBarStyle.normal.background = getTextureWithColor (new Color (0.1f, 0.8f, 0.1f));
		resourceBarStyle.normal.background = getTextureWithColor (Color.blue);
	}

	private Texture2D getTextureWithColor(Color col){
		Color[] uselessColorArray = new Color[1];
		uselessColorArray [0] = col;
		Texture2D texture = new Texture2D (1, 1);
		texture.SetPixels (uselessColorArray);
		texture.Apply ();
		return texture;
	}

	void OnGUI()
	{
		target = player.GetTarget ();

		drawCharInfoAt (player, 2, 2);

		if (target != null && !target.IsDead()) {

			drawCharInfoAt (target, 30, 2);

			//test outlining target
			if (target.GetGameObject ().GetComponent<cakeslice.Outline> () == null) {
				target.GetGameObject ().AddComponent<cakeslice.Outline> ();
			}
		}

		if (player.IsCasting()) {
			drawCastBar ();
		}
	}

	private void drawCharInfoAt(Character c, int xPercent, int yPercent){
		int x = (int)(Screen.width * xPercent / 100);
		int y = (int)(Screen.height * yPercent / 100);

		GUI.Box (new Rect (x, y, 200, 20), c.GetName(),nameBarStyle);
		GUI.Box (new Rect (x, y+20, 200, 40),"",nameBarStyle);
		GUI.Box(new Rect(x,y+20,(int) ((float)c.GetCurrentLife()/(float)c.GetStats().MaxLife*200),20),"",healthBarStyle); 
		GUI.Box (new Rect (x,y+20, 200, 20), c.GetCurrentLife() + " / " + c.GetStats().MaxLife,backgroundStyle);
		GUI.Box(new Rect(x,y+40,(int) ((float)c.GetCurrentResource()/(float)c.GetStats().MaxResource*200),20),"",resourceBarStyle); 
		GUI.Box (new Rect (x,y+40, 200, 20), c.GetCurrentResource() + " / " + c.GetStats().MaxResource,backgroundStyle);


	}


	private void drawCastBar(){

		Spell castingSpell = player.GetCastingSpell ();

		float spellCastTime = castingSpell.GetCastTime (player.GetStats());
		if (spellCastTime != 0) {

			GUI.Box (new Rect (Screen.width / 2, 93	* Screen.height / 100, 200, 30), castingSpell.GetName () + " : " + player.GetCastingTime().ToString ("0.0") + " / " + spellCastTime.ToString("0.0"));
			GUI.Box (new Rect (Screen.width / 2, 93 * Screen.height / 100, player.GetCastPercent()*200, 30), new Texture2D (1, 1)); 
		}
	}
}
