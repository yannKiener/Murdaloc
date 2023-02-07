using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interface : MonoBehaviour
{
    public Texture copperIcon;
    public Texture silverIcon;
    public Texture goldIcon;

    public GUIStyle nameBarStyle;
	public GUIStyle healthBarStyle;
	public GUIStyle resourceBarStyle;
	public GUIStyle backgroundStyle;
	public GUIStyle castBarStyle;
	public GUIStyle toolTipStyle;
	public GUIStyle buffStackStyle;
    public GUIStyle buffTimerStyle;
    public GUIStyle expBarStyle;
    public GUIStyle menuStyle;
    public GUIStyle buttonStyle;
    public List<GameObject> backgroundObjectsListDefault;

    public AudioClip click;
    public AudioClip cancel;
    public AudioClip openWindow;
    public AudioClip closeWindow;
    public AudioClip openVendor;
    public AudioClip closeVendor;
    public AudioClip coinSound;
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
    int mainMenuWidth = (int)(Screen.width * Constants.mainMenuSizeXPercent / 100);
    int mainMenuHeight = (int)(Screen.width * Constants.mainMenuSizeYPercent / 100);
    int modalDialogWidth = (int)(Screen.width * Constants.modalDialogSizeXPercent / 100);
    int modalDialogHeight = (int)(Screen.width * Constants.modalDialogSizeYPercent / 100);
    int optionMenuWidth = (int)(Screen.width * Constants.optionMenuSizeXPercent / 100);
    int optionMenuHeight = (int)(Screen.width * Constants.optionMenuSizeYPercent / 100);
    private static string toolTipText;
    private static string toolTipName;
    private static int toolTipPrice;
    private int cashIconSize;

    private static string ModalText;
    private static Action ModalButtonAction;
    private static bool isMenuOpen = false;
    private static bool isOptionMenuOpen = false;


    public static void LoadPlayer(){
		player = FindUtils.GetPlayer();
	}

    public static List<GameObject> GetDefaultBackgroundsGameObjects()
    {
        return FindUtils.GetInterface().backgroundObjectsListDefault;
    }

    public static void Click()
    {
        SoundManager.PlaySound(FindUtils.GetInterface().click);
    }
    public static void Cancel()
    {
        SoundManager.PlaySound(FindUtils.GetInterface().cancel);
    }
    public static void OpenVendor()
    {
        SoundManager.PlaySound(FindUtils.GetInterface().openVendor);
    }
    public static void CloseVendor()
    {
        SoundManager.PlaySound(FindUtils.GetInterface().closeVendor);
    }
    public static void CoinSound()
    {
        SoundManager.PlaySound(FindUtils.GetInterface().coinSound);
    }
    public static void OpenWindow(bool playSound = true)
    {
        if (playSound)
        {
            SoundManager.PlaySound(FindUtils.GetInterface().openWindow);
        }
    }
    public static void CloseWindow(bool playSound = true)
    {
        if (playSound)
        {
            SoundManager.PlaySound(FindUtils.GetInterface().closeWindow);
        }
    }
    public static void OpenInventory()
    {
        SoundManager.PlaySound(FindUtils.GetInterface().openInventory);
    }
    public static void CloseInventory()
    {
        SoundManager.PlaySound(FindUtils.GetInterface().closeInventory);
    }
    public static void OpenSpellbook()
    {
        SoundManager.PlaySound(FindUtils.GetInterface().openSpellBook);
    }
    public static void CloseSpellbook()
    {
        SoundManager.PlaySound(FindUtils.GetInterface().closeSpellBook);
    }
    public static void OpenCharsheet()
    {
        SoundManager.PlaySound(FindUtils.GetInterface().openCharSheet);
    }
    public static void CloseCharsheet()
    {
        SoundManager.PlaySound(FindUtils.GetInterface().closeCharSheet);
    }
    public static void OpenQuestlog()
    {
        SoundManager.PlaySound(FindUtils.GetInterface().openQuestLog);
    }
    public static void CloseQuestlog()
    {
        SoundManager.PlaySound(FindUtils.GetInterface().closeQuestLog);
    }
    public static void QuestDone()
    {
        SoundManager.PlaySound(FindUtils.GetInterface().questDone);
    }
    public static void QuestAdded()
    {
        SoundManager.PlaySound(FindUtils.GetInterface().questAdded);
    }
    public static void QuestReadyToTurnIn()
    {
        SoundManager.PlaySound(FindUtils.GetInterface().questReady);
    }
    public static void LevelUp()
    {
        SoundManager.PlaySound(FindUtils.GetInterface().levelUp);
    }

    // Use this for initialization
    void Start () {

        if (menuStyle.normal.background == null)
        {
            menuStyle.normal.background = InterfaceUtils.GetTextureWithColor(new Color(0.1f, 0.1f, 0.1f, 0.9f));
        }

        if (buttonStyle.normal.background == null)
        {
            buttonStyle.normal.background = InterfaceUtils.GetTextureWithColor(new Color(0.7f, 0.1f, 0.1f, 0.9f));
        }

        cashIconSize = (int)(Screen.height * 4 / 100);
    }   

    public static void OpenCloseMenu()
    {
        if (isMenuOpen)
        {
            CloseMenu();
        } else
        { 
            OpenMenu();
        }
    }

    public static void OpenMenu(bool playSound = true)
    {
        Interface.OpenWindow(playSound);
        isMenuOpen = true;
    }

    public static void CloseMenu(bool playSound = true)
    {
        Interface.CloseWindow(playSound);
        isMenuOpen = false;
    }

    public static void OpenOptionMenu()
    {
        isOptionMenuOpen = true;
    }

    public static void CloseOptionMenu()
    {
        isOptionMenuOpen = false;
    }


    public static void DrawModalDialog(string modalText, Action modalButtonAction)
    {
        ModalText = modalText;
        ModalButtonAction = modalButtonAction;
    }

    public static bool CloseModalDialog()
    {
        bool result = ModalText != null;
        ModalText = null;
        ModalButtonAction = null;
        return result;
    }

    public static void DrawToolTip(string name, string description, int price = 0)
    {
        toolTipText = description;
        toolTipName = name;
        toolTipPrice = price;
    }
    
    public static void RemoveToolTip()
    {
        toolTipText = null;
        toolTipName = null;
        toolTipPrice = 0;
    }

    public void ForceBarStyles()
    {

        if (nameBarStyle.normal.background == null)
        {
            nameBarStyle.normal.background = InterfaceUtils.GetTextureWithColor(new Color(0, 0, 0, 0.7f));
        }
        if (healthBarStyle.normal.background == null) 
        {
            healthBarStyle.normal.background = InterfaceUtils.GetTextureWithColor(new Color(0.1f, 0.8f, 0.1f));
        }
        if (resourceBarStyle.normal.background == null)
        {
            resourceBarStyle.normal.background = InterfaceUtils.GetTextureWithColor(Color.blue);
        }
        if (castBarStyle.normal.background == null)
        {
            castBarStyle.normal.background = InterfaceUtils.GetTextureWithColor(new Color(0.84f, 0.72f, 0.41f));
        }
        if (toolTipStyle.normal.background == null)
        {
            toolTipStyle.normal.background = InterfaceUtils.GetTextureWithColor(new Color(0.01f, 0, 0.1f, 0.7f));
        }
        if (expBarStyle.normal.background == null)
        {
            expBarStyle.normal.background = InterfaceUtils.GetTextureWithColor(new Color(0.3f, 0, 0.5f, 1));
        }
    }


	void OnGUI()
	{
        ForceBarStyles();


        drawCharInfoAt(player, 2, 0);

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
        drawModalDialog();
        drawMenus();


    }

    private void drawMenus()
    {
        if (isMenuOpen)
        {
            CloseOptionMenu();
            CloseModalDialog();
            Rect windowRect = new Rect((Screen.width - mainMenuWidth) / 2, (Screen.height - mainMenuHeight) / 2, mainMenuWidth, mainMenuHeight);

            GUI.Window(1, windowRect, InGameMenuWindow, "Menu", menuStyle);
        }
        if (isOptionMenuOpen)
        {
            Rect windowRect = new Rect((Screen.width - optionMenuWidth) / 2, (Screen.height - optionMenuHeight) / 2, optionMenuWidth, optionMenuHeight);

            GUI.Window(2, windowRect, InGameOptionMenuWindow, "Options", menuStyle);
        }

    }

    private Rect getRectForIngameOptionMenuPosition(int position)
    {
        int paddingX = optionMenuWidth / 10;
        int paddingY = optionMenuHeight / 10;
        int buttonSizeX = optionMenuWidth;
        int buttonSizeY = optionMenuHeight / 10;
        return new Rect(paddingX, paddingY + (position - 1) * (buttonSizeY), buttonSizeX - paddingX * 2, buttonSizeY);
    }

    void InGameOptionMenuWindow(int windowID)
    {
        float soundVolumeLevel = SoundManager.GetVolume();
        GUI.Label(getRectForIngameOptionMenuPosition(1), "Sound effects volume:");
        SoundManager.SetVolume(GUI.HorizontalScrollbar(getRectForIngameOptionMenuPosition(2), soundVolumeLevel, 0.1f, 0, 1));
        float musicVolumeLevel = MusicManager.GetVolume();
        GUI.Label(getRectForIngameOptionMenuPosition(4), "Music volume:");
        MusicManager.SetVolume(GUI.HorizontalScrollbar(getRectForIngameOptionMenuPosition(5), musicVolumeLevel, 0.1f, 0, 1));

        if (GUI.Button(getRectForIngameOptionMenuPosition(8), "Return", buttonStyle))
        {
            OpenMenu(false);
        }
    }

    private Rect getRectForIngameMenuPosition(int position)
    {
        int paddingX = mainMenuWidth / 10;
        int paddingY = mainMenuHeight / 10;
        int buttonSizeX = mainMenuWidth;
        int buttonSizeY = mainMenuHeight / 10;
        return new Rect(paddingX, paddingY + (position - 1) * (buttonSizeY), buttonSizeX - paddingX * 2, buttonSizeY);
    }

    void InGameMenuWindow(int windowID)
    {
        if (GUI.Button(getRectForIngameMenuPosition(2), "Options", buttonStyle))
        {
            CloseMenu(false);
            OpenOptionMenu();
        }
        if (GUI.Button(getRectForIngameMenuPosition(4), "Main Menu", buttonStyle))
        {
            CloseMenu();
            GameUtils.LoadScene("MainMenu");
        }

        if (GUI.Button(getRectForIngameMenuPosition(5), "Exit", buttonStyle))
        {
            CloseMenu();
            Action quitAction = new Action(() => Application.Quit());
            DrawModalDialog("Are you sure you want to quit ? ", quitAction);
        }


        if (GUI.Button(getRectForIngameMenuPosition(8), "Return", buttonStyle))
        {
            CloseMenu();
        }
    }

        private void drawModalDialog()
    {
        if (ModalText != null && ModalButtonAction != null)
        {
            Rect windowRect = new Rect((Screen.width - modalDialogWidth) / 2, (Screen.height - modalDialogHeight) / 2, modalDialogWidth, modalDialogHeight);
            GUI.Window(0, windowRect, YesNoWindow, "", menuStyle);

        }

    }

    void YesNoWindow(int windowID)
    {
        int buttonXSize = modalDialogWidth / 6;
        int buttonYSize = modalDialogWidth / 6;
        int textXSize = modalDialogWidth - 20;
        int testYSize = modalDialogHeight - 15 - buttonYSize;
        GUI.Label(new Rect(10, 5, textXSize, testYSize), ModalText);

        if (GUI.Button(new Rect(modalDialogWidth/3 - buttonXSize/2, 4* modalDialogHeight/5 - buttonYSize / 2, buttonXSize, buttonYSize), "Yes", buttonStyle))
        {
            ModalButtonAction();
            CloseModalDialog();
        }
        if(GUI.Button(new Rect(2* modalDialogWidth/3 - buttonXSize/2, 4 * modalDialogHeight / 5 - buttonYSize/2, buttonXSize, buttonYSize), "No", buttonStyle))
        {
            CloseModalDialog();
        }
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
            Rect labelRect = new Rect(getToolTipPositionX(Input.mousePosition.x, size.x), getToolTipPositionY(Input.mousePosition.y, size.y + cashIconSize), size.x, size.y);
            GUI.Label(labelRect, tooltipContent, toolTipStyle);
            if(toolTipPrice > 0)
            {
                int copper = InterfaceUtils.GetCopper(toolTipPrice);
                int silver = InterfaceUtils.GetSilver(toolTipPrice);
                int gold = InterfaceUtils.GetGold(toolTipPrice);

                Rect rect = new Rect(labelRect.x + labelRect.size.x - cashIconSize, labelRect.y + labelRect.size.y, cashIconSize, cashIconSize);

                GUIContent copperText = new GUIContent(copper.ToString());
                Vector2 c = toolTipStyle.CalcSize(copperText);
                Rect rect2 = new Rect(rect.x - c.x, rect.y, cashIconSize, cashIconSize);
                GUI.Label(rect2, copperText, toolTipStyle);
                GUI.Label(rect, copperIcon, toolTipStyle);

                if (silver > 0 || gold > 0)
                {
                    GUIContent silverText = new GUIContent(silver.ToString());
                    Vector2 silverSize = toolTipStyle.CalcSize(silverText);
                    rect = new Rect(rect2.x - cashIconSize, rect2.y, cashIconSize, cashIconSize);
                    rect2 = new Rect(rect.x - silverSize.x, rect.y, cashIconSize, cashIconSize);
                    GUI.Label(rect2, silverText, toolTipStyle);
                    GUI.Label(rect, silverIcon, toolTipStyle);

                    if (gold > 0)
                    {
                        GUIContent goldText = new GUIContent(gold.ToString());
                        Vector2 goldSize = toolTipStyle.CalcSize(goldText);
                        rect = new Rect(rect2.x - cashIconSize, rect2.y, cashIconSize, cashIconSize);
                        rect2 = new Rect(rect.x - goldSize.x, rect.y, cashIconSize, cashIconSize);
                        GUI.Label(rect2, goldText, toolTipStyle);
                        GUI.Label(rect, goldIcon, toolTipStyle);
                    }
                }
            }
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
        int bonusForAndroid = 0;
        if (SystemInfo.deviceType == DeviceType.Handheld)
        {
            bonusForAndroid = 64;
        }

        y += bonusForAndroid;

        float result = Screen.height - y - toolTipSizeY;
        if (result < 0)
        {
            return result + toolTipSizeY + 32 + bonusForAndroid;
        } else
        {
            return result;
        }
    }
}
