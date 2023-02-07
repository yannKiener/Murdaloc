using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interface : MonoBehaviour {
	public GUIStyle nameBarStyle;
	public GUIStyle healthBarStyle;
	public GUIStyle resourceBarStyle;
	public GUIStyle backgroundStyle;
	public GUIStyle castBarStyle;
	public GUIStyle toolTipStyle;
	public GUIStyle textOverStyle;
	static Player player;
	static Character target;
	int barsWidth = (int)(Screen.width * Constants.characterBarswPercent / 100);
	int barsHeight = (int)(Screen.height * Constants.characterBarshPercent / 100);
	int castBarWidth = (int)(Screen.width * Constants.castBarwPercent / 100);
	int castBarHeight = (int)(Screen.height * Constants.castBarhPercent / 100);
    static string toolTipText;
    static string toolTipName;



	public static void LoadPlayer(){
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player>();
	}
		
	// Use this for initialization
	void Start () {
		nameBarStyle.normal.background = InterfaceUtils.GetTextureWithColor(new Color (0, 0, 0, 0.7f));
		healthBarStyle.normal.background = InterfaceUtils.GetTextureWithColor(new Color (0.1f, 0.8f, 0.1f));
		resourceBarStyle.normal.background = InterfaceUtils.GetTextureWithColor(Color.blue);
		castBarStyle.normal.background = InterfaceUtils.GetTextureWithColor(new Color(0.84f,0.72f,0.41f));
		toolTipStyle.normal.background = InterfaceUtils.GetTextureWithColor(new Color (0.01f, 0, 0.1f, 0.7f));
	}

    public static void DrawToolTip(string name, string description)
    {
        toolTipText = description;
        toolTipName = name;
    }
    
    public static void RemoveToolTip()
    {
        toolTipText = null;
        toolTipName = null;
    }


	void OnGUI()
	{
		drawCharInfoAt (player, 2, 2);

		target = player.GetTarget ();
		if (target != null && !target.IsDead()) {
			drawCharInfoAt (target, 70, 2);
			//outlining target sprite
			/*
			if (target.GetGameObject ().GetComponent<cakeslice.Outline> () == null) {
				target.GetGameObject ().AddComponent<cakeslice.Outline> ();
			}
			*/
		}

		if (player.IsCasting() && Constants.drawCastBar) {
			int x = (int)(Screen.width * 50 / 100);
			x -= (castBarWidth / 2);
			int y = (int)(Screen.height * 96 / 100);
			drawCastBar (player, x,y,true,castBarWidth,castBarHeight);
		}
        drawToolTip();

	}

    private void drawToolTip()
    {
        if (toolTipText != null && toolTipName != null)
        {
            string tooltipContent = string.Concat(toolTipName,":\n", toolTipText);
            GUIContent tooltip = new GUIContent(tooltipContent);
            Vector2 size = toolTipStyle.CalcSize(tooltip);
            GUI.Label(new Rect(getToolTipPositionX(Input.mousePosition.x, size.x), getToolTipPositionY(Input.mousePosition.y, size.y), size.x, size.y), tooltipContent, toolTipStyle);
        }
    }


    private void drawCharInfoAt(Character c, int xPercent, int yPercent){
		int x = (int)(Screen.width * xPercent / 100);
		int y = (int)(Screen.height * yPercent / 100);
		//Draw character Interface
		GUI.Box (new Rect (x, y, barsWidth, barsHeight), c.GetName(),nameBarStyle);
		GUI.Box (new Rect (x, y+barsHeight, barsWidth, 3*barsHeight),"",nameBarStyle);
		GUI.Box(new Rect(x,y+barsHeight,(int) ((float)c.GetCurrentLife()/(float)c.GetStats().MaxLife*barsWidth),barsHeight),"",healthBarStyle); 
		GUI.Box (new Rect (x,y+barsHeight, barsWidth, barsHeight), c.GetCurrentLife() + " / " + c.GetStats().MaxLife,backgroundStyle);
		GUI.Box(new Rect(x,y+(2*barsHeight),(int) ((float)c.GetCurrentResource()/(float)c.GetStats().MaxResource*barsWidth),barsHeight),"",resourceBarStyle); 
		GUI.Box (new Rect (x,y+(2*barsHeight), barsWidth, barsHeight), c.GetCurrentResource() + " / " + c.GetStats().MaxResource,backgroundStyle);
		List<EffectOnTime> cBuffs = c.GetBuffs ();
		List<EffectOnTime> cDebuffs = c.GetDebuffs();
		//Draw castBar
		if (c.IsCasting () && Constants.drawCastBar) {
			drawCastBar (c,x,y +(3*barsHeight) ,false, barsWidth, barsHeight);
		}
		bool hasbuffs = cBuffs.Count > 0;
		//Draw buffs
		if (hasbuffs) {
			drawEffects (cBuffs,x,y+(4*barsHeight),barsHeight,barsHeight);
		}
		//Draw Debuffs
		if (cDebuffs.Count > 0) {
			int yPosition = y + (4 * barsHeight);
			if (hasbuffs) {
				yPosition += barsHeight;
			}

			drawEffects (cDebuffs,x,yPosition,barsHeight,barsHeight);	
		}

	}


	private void drawEffects(List<EffectOnTime> effects,int x, int y, int w, int h){ //Todo Refacto ca proprement je pars manger ;)
		foreach (EffectOnTime effect in effects) {
			GUIContent content = new GUIContent (InterfaceUtils.LoadTextureForSpell(effect.GetName ()), effect.GetName().Substring(0,1).ToUpper() + effect.GetName().Substring(1) +". " + effect.GetDescription () + " Time left : " + effect.GetTimeLeft ().ToString ("0.0"));
			GUI.Box (new Rect (x, y, h, w), content);
			int stacks = effect.GetStacks();
			if(stacks > 1){
				GUI.Label(new Rect (x, y, h, w), stacks.ToString(),textOverStyle);
			}
			if (GUI.tooltip.Length > 1) {
				GUIContent tooltip = new GUIContent (GUI.tooltip);
				Vector2 size =  toolTipStyle.CalcSize (tooltip);
				GUI.Label(new Rect(x+w,y+h,size.x,size.y),tooltip,toolTipStyle);
			}
			x += w;
		}
	}


	private void drawCastBar(Character c, float x, float y, bool drawIcon,int castBarW ,int castBarH){

		Spell castingSpell = c.GetCastingSpell ();

		float spellCastTime = castingSpell.GetCastTime (c.GetStats());
		if (spellCastTime != 0) {
			GUI.Box (new Rect (x, y, castBarW, castBarH),"",nameBarStyle);
			GUI.Box (new Rect (x, y, c.GetCastPercent()*castBarW, castBarH),"",castBarStyle); 
			GUI.Box (new Rect (x, y, castBarW, castBarH), castingSpell.GetName () + " : " + c.GetCastingTime().ToString ("0.0") + " / " + spellCastTime.ToString("0.0"),backgroundStyle);
			if (drawIcon) {
				GUI.Box (new Rect (x - castBarH, y, castBarH, castBarH), InterfaceUtils.LoadTextureForSpell(castingSpell.GetName ()));
			}
		}
	}

    private float getToolTipPositionX(float x, float toolTipSizeX)
    {
        if (x + toolTipSizeX > Screen.width)
        {
            return x - toolTipSizeX;
        } else
        {
            return x;
        }
    }

    private float getToolTipPositionY(float y, float toolTipSizeY)
    {
        float result = Screen.height - y - toolTipSizeY;
        if (result < 0)
        {
            return result + toolTipSizeY;
        } else
        {
            return result;
        }
    }

}
