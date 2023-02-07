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
	public GUIStyle buffStackStyle;
    public GUIStyle buffTimerStyle;
    public GUIStyle expBarStyle;

    public AudioClip click;
    public AudioClip cancel;
    public AudioClip openWindow;
    public AudioClip closeWindow;
    public AudioClip openVendor;
    public AudioClip closeVendor;
    public AudioClip openInventory;
    public AudioClip closeInventory;
    public AudioClip openSpellBook;
    public AudioClip closeSpellBook;
    public AudioClip openCharSheet;
    public AudioClip closeCharSheet;
    public AudioClip openQuestLog;
    public AudioClip closeQuestLog;
    public AudioClip questDone;
    public AudioClip questAdded;
    public AudioClip questReady;
    public AudioClip levelUp;

    static Player player;
	static Character target;
	int barsWidth = (int)(Screen.width * Constants.characterBarswPercent / 100);
	int barsHeight = (int)(Screen.height * Constants.characterBarshPercent / 100);
	int castBarWidth = (int)(Screen.width * Constants.castBarwPercent / 100);
	int castBarHeight = (int)(Screen.height * Constants.castBarhPercent / 100);
    int expBarHeight = (int)(Screen.height* Constants.expBarHeightPercent / 100);
    static string toolTipText;
    static string toolTipName;

	public static void LoadPlayer(){
		player = FindUtils.GetPlayer();
	}

    public void Click()
    {
        SoundManager.PlaySound(click);
    }
    public void Cancel()
    {
        SoundManager.PlaySound(cancel);
    }
    public void OpenVendor()
    {
        SoundManager.PlaySound(openVendor);
    }
    public void CloseVendor()
    {
        SoundManager.PlaySound(closeVendor);
    }
    public void OpenWindow()
    {
        SoundManager.PlaySound(openWindow);
    }
    public void CloseWindow()
    {
        SoundManager.PlaySound(closeWindow);
    }
    public void OpenInventory()
    {
        SoundManager.PlaySound(openInventory);
    }
    public void CloseInventory()
    {
        SoundManager.PlaySound(closeInventory);
    }
    public void OpenSpellbook()
    {
        SoundManager.PlaySound(openSpellBook);
    }
    public void CloseSpellbook()
    {
        SoundManager.PlaySound(closeSpellBook);
    }
    public void OpenCharsheet()
    {
        SoundManager.PlaySound(openCharSheet);
    }
    public void CloseCharsheet()
    {
        SoundManager.PlaySound(closeCharSheet);
    }
    public void OpenQuestlog()
    {
        SoundManager.PlaySound(openQuestLog);
    }
    public void CloseQuestlog()
    {
        SoundManager.PlaySound(closeQuestLog);
    }
    public void QuestDone()
    {
        SoundManager.PlaySound(questDone);
    }
    public void QuestAdded()
    {
        SoundManager.PlaySound(questAdded);
    }
    public void QuestReadyToTurnIn()
    {
        SoundManager.PlaySound(questReady);
    }
    public void LevelUp()
    {
        SoundManager.PlaySound(levelUp);
    }

    // Use this for initialization
    void Start () {
		nameBarStyle.normal.background = InterfaceUtils.GetTextureWithColor(new Color (0, 0, 0, 0.7f));
		healthBarStyle.normal.background = InterfaceUtils.GetTextureWithColor(new Color (0.1f, 0.8f, 0.1f));
		resourceBarStyle.normal.background = InterfaceUtils.GetTextureWithColor(Color.blue);
		castBarStyle.normal.background = InterfaceUtils.GetTextureWithColor(new Color(0.84f,0.72f,0.41f));
		toolTipStyle.normal.background = InterfaceUtils.GetTextureWithColor(new Color (0.01f, 0, 0.1f, 0.7f));
        expBarStyle.normal.background = InterfaceUtils.GetTextureWithColor(new Color(0.3f,0,0.5f,1));
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
		drawCharInfoAt (player, 2, 0);

		target = player.GetTarget ();
		if (target != null && !target.IsDead()) {
			drawCharInfoAt (target, 70, 0);
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
        drawExperienceBar(FindUtils.GetPlayer(),0,99);


    }

    private void drawExperienceBar(Player player, int xPercent, int yPercent)
    {

        int x = (int)(Screen.width * xPercent / 100);
        int y = (int)(Screen.height * yPercent / 100);
        GUI.Box(new Rect(x,y, Screen.width, expBarHeight), "", nameBarStyle);
        GUI.Box(new Rect(x, y, (Screen.width * player.GetExp() / 100), expBarHeight), "", expBarStyle);
        GUI.Box(new Rect(x, y, Screen.width, expBarHeight), player.GetExp().ToString("0.0") + "%", backgroundStyle);
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
        GUI.Box (new Rect(x, y, barsWidth / 5, barsHeight), c.GetLevel().ToString(), nameBarStyle);
        if (c.IsElite())
        {
            GUI.Box(new Rect(x + 3* barsWidth / 5, y, 2*barsWidth / 5, barsHeight), "Elite", nameBarStyle);
        }
        GUI.Box (new Rect(x, y + barsHeight, barsWidth, barsHeight), c.GetName(),nameBarStyle);
		GUI.Box (new Rect (x, y+2*barsHeight, barsWidth, 3*barsHeight),"",nameBarStyle);
		GUI.Box(new Rect(x,y+2*barsHeight,(int) ((float)c.GetCurrentLife()/(float)c.GetStats().MaxLife*barsWidth),barsHeight),"",healthBarStyle); 
		GUI.Box (new Rect (x,y+2*barsHeight, barsWidth, barsHeight), c.GetCurrentLife() + " / " + c.GetStats().MaxLife,backgroundStyle);
		GUI.Box(new Rect(x,y+(3*barsHeight),(int) ((float)c.GetCurrentResource()/(float)c.GetStats().MaxResource*barsWidth),barsHeight),"",resourceBarStyle); 
		GUI.Box (new Rect (x,y+(3*barsHeight), barsWidth, barsHeight), c.GetCurrentResource() + " / " + c.GetStats().MaxResource,backgroundStyle);
		List<EffectOnTime> cBuffs = c.GetBuffs ();
		List<EffectOnTime> cDebuffs = c.GetDebuffs();
		//Draw castBar
		if (c.IsCasting () && Constants.drawCastBar) {
			drawCastBar (c,x,y +(4*barsHeight) ,false, barsWidth, barsHeight);
		}
		bool hasbuffs = cBuffs.Count > 0;
		//Draw buffs
		if (hasbuffs) {
			drawEffects (cBuffs,x,y+(5*barsHeight),barsHeight,barsHeight);
		}
		//Draw Debuffs
		if (cDebuffs.Count > 0) {
			int yPosition = y + (5 * barsHeight);
			if (hasbuffs) {
				yPosition += barsHeight;
			}

			drawEffects (cDebuffs,x,yPosition,barsHeight,barsHeight);	
		}

	}

    private string getBuffFormatForTime(float timeLeft)
    {
        int minutes = (int)(timeLeft / 60);
        int seconds = (int)(timeLeft % 60);

        if(minutes >= 1)
        {
            return minutes + "m";
        }
        else
        {
            return seconds + "s";
        }

    }

    private void DrawOutline(Rect rect, string text, GUIStyle style, Color outlineColor, float size)
    {
        float halfSize = size * 0.5F;
        GUIStyle backupStyle = new GUIStyle(style);
        Color backupColor = GUI.color;

        style.normal.textColor = outlineColor;
        GUI.color = outlineColor;

        rect.x -= halfSize;
        GUI.Label(rect, text, style);

        rect.x += size;
        GUI.Label(rect, text, style);

        rect.x -= halfSize;
        rect.y -= halfSize;
        GUI.Label(rect, text, style);

        rect.y += size;
        GUI.Label(rect, text, style);

        rect.y -= halfSize;
        style.normal.textColor = backupStyle.normal.textColor;
        GUI.color = backupColor;
        GUI.Label(rect, text, style);

        style = backupStyle;
    }


    private void drawEffects(List<EffectOnTime> effects,int x, int y, int w, int h){ //Todo Refacto ca proprement je pars manger ;)
        int counter = 1;
        int i = 1;
		foreach (EffectOnTime effect in effects) {
			GUIContent content = new GUIContent (InterfaceUtils.LoadTextureForSpell(effect.GetName ()), effect.GetName().Substring(0,1).ToUpper() + effect.GetName().Substring(1) +". " + effect.GetDescription () + " Time left : " + getBuffFormatForTime(effect.GetTimeLeft ()));
			GUI.Box (new Rect (x, y, h, w), content);
			int stacks = effect.GetStacks();
			if(stacks > 1){
                DrawOutline(new Rect (x, y, h, w), stacks.ToString(), buffStackStyle, Color.white,1.5f);
            }
            //On affiche le timer
            GUI.Label(new Rect(x, y + h, h, w), getBuffFormatForTime(effect.GetTimeLeft()), buffTimerStyle);

            if (content.tooltip.Equals(GUI.tooltip))
                counter = i;
			x += w;
            i++;
        }
        //Affichage de la tooltip des buffs et debuffs seulement !
        if (GUI.tooltip.Length > 1)
        {
            GUIContent tooltip = new GUIContent(GUI.tooltip);
            Vector2 size = toolTipStyle.CalcSize(tooltip);
            GUI.Label(new Rect(x + (w * counter), y + h, size.x, size.y), tooltip, toolTipStyle);
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
