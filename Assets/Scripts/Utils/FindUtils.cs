using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FindUtils  {
    private static Player player;
    private static GameObject spellBook;
    private static SpellBook spellBookGrid;
    private static GameObject inventory;
    private static GameObject characterSheet;
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
        if (spellBookGrid == null)
            spellBookGrid = GameObject.Find("SpellBookGrid").GetComponent<SpellBook>();

        return spellBookGrid;
    }

    public static GameObject GetInventory()
    {
        if (inventory == null)
            inventory = GameObject.Find("Inventory");

        return inventory;
    }

    public static GameObject GetCharacterSheet()
    {
        if (characterSheet == null)
            characterSheet = GameObject.Find("CharacterSheet");

        return characterSheet;
    }

    public static CharacterSheet GetCharacterSheetGrid()
    {
        if (characterSheetGrid == null)
            characterSheetGrid = GameObject.Find("CharacterSheetGrid").GetComponent<CharacterSheet>();

        return characterSheetGrid;
    }
}