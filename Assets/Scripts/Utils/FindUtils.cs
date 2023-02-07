using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class FindUtils  {
    private static Player player;
    private static GameObject spellBook;
    private static SpellBook spellBookGrid;
    private static ActionBar actionBar;
    private static GameObject inventory;
    private static Inventory inventoryGrid;
    private static GameObject loot;
    private static GameObject lootGrid;
    private static GameObject characterSheet;
    private static Text characterSheetText;
    private static CharacterSheet characterSheetGrid;
    private static GameObject dialogBox;
    private static DialogBox dialogBoxComponent;
    private static GameObject dialogPanel;
    private static DialogPanel dialogPanelComponent;
    private static GameObject questGridGo;
    private static QuestGrid questGrid;
    private static GameObject questLog;
    private static GameObject dpsBar;
    private static Dps dps;
    private static Interface intrface;
    private static GameObject sounds;

    public static Player GetPlayer()
    {
        if(player == null)
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        return player;
    }

    public static Interface GetInterface()
    {
        if (intrface == null)
            intrface = GameObject.Find("Interface").GetComponent<Interface>();

        return intrface;
    }

    public static GameObject GetSpellBook()
    {
        if (spellBook == null)
            spellBook = GameObject.Find("SpellBook");

        return spellBook;
    }

    public static SpellBook GetSpellBookGrid()
    {
        if (spellBookGrid == null && GameObject.Find("SpellBookGrid") != null)
            spellBookGrid = GameObject.Find("SpellBookGrid").GetComponent<SpellBook>();

        return spellBookGrid;
    }

    public static ActionBar GetActionBar()
    {
        if (actionBar == null)
            actionBar = GameObject.Find("ActionBar").GetComponent<ActionBar>();

        return actionBar;
    }

    public static GameObject GetInventory()
    {
        if (inventory == null)
            inventory = GameObject.Find("Inventory");

        return inventory;
    }

    public static Inventory GetInventoryGrid()
    {
        if (inventoryGrid == null)
            inventoryGrid = GameObject.Find("InventoryGrid").GetComponent<Inventory>();

        return inventoryGrid;
    }

    public static GameObject GetCharacterSheet()
    {
        if (characterSheet == null)
            characterSheet = GameObject.Find("CharacterSheet");

        return characterSheet;
    }

    public static Text GetCharacterSheetText()
    {
        if (characterSheetText == null)
            characterSheetText = GameObject.Find("CharacterSheetText").GetComponent<Text>();

        return characterSheetText;
    }

    public static CharacterSheet GetCharacterSheetGrid()
    {
        if (characterSheetGrid == null)
            characterSheetGrid = GameObject.Find("CharacterSheetGrid").GetComponent<CharacterSheet>();

        return characterSheetGrid;
    }


    public static GameObject GetLoot()
    {
        if (loot == null)
            loot = GameObject.Find("Loot");
        
        return loot;
    }

    public static GameObject GetLootGrid()
    {
        if (lootGrid == null)
            lootGrid = GameObject.Find("LootGrid");

        return lootGrid;
    }

    public static GameObject GetDialogBox()
    {
        if(dialogBox == null)
            dialogBox = GameObject.Find("DialogBox");

        return dialogBox;
    }

    public static DialogBox GetDialogBoxComponent()
    {
        if (dialogBoxComponent == null)
            dialogBoxComponent = GetDialogBox().GetComponent<DialogBox>();

        return dialogBoxComponent;
    }

    public static GameObject GetDialogPanel()
    {
        if (dialogPanel == null)
            dialogPanel = GameObject.Find("DialogPanel");
        
        return dialogPanel;
    }
    public static DialogPanel GetDialogPanelComponent()
    {
        if (dialogPanelComponent == null)
            dialogPanelComponent = GetDialogPanel().GetComponent<DialogPanel>();
            
        return dialogPanelComponent;
    }


    public static GameObject GetQuestLog()
    {
        if (questLog == null)
            questLog = GameObject.Find("QuestLog");

        return questLog;
    }

    public static GameObject GetQuestGridGameObject()
    {
        if (questGridGo == null)
            questGridGo = GameObject.Find("QuestGrid");

        return questGridGo;
    }

    public static QuestGrid GetQuestGrid()
    {
        if (questGrid == null)
            questGrid = GetQuestGridGameObject().GetComponent<QuestGrid>();

        return questGrid;
    }

    public static GameObject GetDpsBar()
    {
        if (dpsBar == null)
            dpsBar = GameObject.Find("DpsBar");

        return dpsBar;
    }

    public static Dps GetDps()
    {
        if (dps == null)
            dps = GetDpsBar().transform.Find("DpsBackground").Find("DpsNumber").GetComponent<Dps>();

        return dps;
    }

    public static GameObject GetSoundsGameObject()
    {
        if (sounds == null)
            sounds = GameObject.Find("Sounds");
        return sounds;
    }

}