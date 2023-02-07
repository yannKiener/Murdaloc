using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class FindUtils  {
    private static Player player;
    private static GameObject spellBook;
    private static SpellBook spellBookGrid;
    private static GameObject inventory;
    private static Inventory inventoryGrid;
    private static GameObject loot;
    private static GameObject lootGrid;
    private static GameObject characterSheet;
    private static Text characterSheetText;
    private static CharacterSheet characterSheetGrid;

    public static Player GetPlayer()
    {
        if(player == null)
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        return player;
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
}