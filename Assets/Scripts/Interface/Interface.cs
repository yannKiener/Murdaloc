using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interface : MonoBehaviour {
	public GUIStyle nameBarStyle;
	public GUIStyle healthBarStyle;
	public GUIStyle resourceBarStyle;
	public GUIStyle backgroundStyle;
	public GUIStyle castBarStyle;
	static Player player;
	static Character target;
	int barsWidth = (int)(Screen.width * Constants.characterBarswPercent / 100);
	int barsHeight = (int)(Screen.height * Constants.characterBarshPercent / 100);
	int castBarWidth = (int)(Screen.width * Constants.castBarwPercent / 100);
	int castBarHeight = (int)(Screen.height * Constants.castBarhPercent / 100);


	public static void LoadPlayer(){
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player>();
	}
		
	// Use this for initialization
	void Start () {
		nameBarStyle.normal.background = getTextureWithColor(new Color (0, 0, 0, 0.7f));
		healthBarStyle.normal.background = getTextureWithColor (new Color (0.1f, 0.8f, 0.1f));
		resourceBarStyle.normal.background = getTextureWithColor (Color.blue);
		castBarStyle.normal.background = getTextureWithColor (new Color(0.84f,0.72f,0.41f));
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
		drawCharInfoAt (player, 2, 2);

		target = player.GetTarget ();
		if (target != null && !target.IsDead()) {
			drawCharInfoAt (target, 70, 2);
			//outlining target sprite
			if (target.GetGameObject ().GetComponent<cakeslice.Outline> () == null) {
				target.GetGameObject ().AddComponent<cakeslice.Outline> ();
			}
		}

		if (player.IsCasting() && Constants.drawCastBar) {
			drawCastBar (player, 50,96);
		}

	}

	private void drawCharInfoAt(Character c, int xPercent, int yPercent){
		int x = (int)(Screen.width * xPercent / 100);
		int y = (int)(Screen.height * yPercent / 100);

		GUI.Box (new Rect (x, y, barsWidth, barsHeight), c.GetName(),nameBarStyle);
		GUI.Box (new Rect (x, y+barsHeight, barsWidth, 3*barsHeight),"",nameBarStyle);
		GUI.Box(new Rect(x,y+barsHeight,(int) ((float)c.GetCurrentLife()/(float)c.GetStats().MaxLife*barsWidth),barsHeight),"",healthBarStyle); 
		GUI.Box (new Rect (x,y+barsHeight, barsWidth, barsHeight), c.GetCurrentLife() + " / " + c.GetStats().MaxLife,backgroundStyle);
		GUI.Box(new Rect(x,y+(2*barsHeight),(int) ((float)c.GetCurrentResource()/(float)c.GetStats().MaxResource*barsWidth),barsHeight),"",resourceBarStyle); 
		GUI.Box (new Rect (x,y+(2*barsHeight), barsWidth, barsHeight), c.GetCurrentResource() + " / " + c.GetStats().MaxResource,backgroundStyle);

		if (c.IsCasting ()) {
			Spell castingSpell = c.GetCastingSpell ();
			float spellCastTime = castingSpell.GetCastTime (c.GetStats());

			GUI.Box(new Rect(x,y+(3*barsHeight),c.GetCastPercent()*barsWidth,barsHeight),"",castBarStyle); 
			GUI.Box (new Rect (x,y+(3*barsHeight), barsWidth, barsHeight), castingSpell.GetName () + " : " + player.GetCastingTime().ToString ("0.0") + " / " + spellCastTime.ToString("0.0"),backgroundStyle);

		}
	}


	private void drawCastBar(Character c, float xPercent, float yPercent){
		int x = (int)(Screen.width * xPercent / 100);
		x -= (castBarWidth / 2);
		int y = (int)(Screen.height * yPercent / 100);

		Spell castingSpell = c.GetCastingSpell ();

		float spellCastTime = castingSpell.GetCastTime (c.GetStats());
		if (spellCastTime != 0) {
			GUI.Box (new Rect (x, y, castBarWidth, castBarHeight),"",nameBarStyle);
			GUI.Box (new Rect (x, y, c.GetCastPercent()*castBarWidth, castBarHeight),"",castBarStyle); 
			GUI.Box (new Rect (x, y, castBarWidth, castBarHeight), castingSpell.GetName () + " : " + c.GetCastingTime().ToString ("0.0") + " / " + spellCastTime.ToString("0.0"),backgroundStyle);
		}
	}
}
