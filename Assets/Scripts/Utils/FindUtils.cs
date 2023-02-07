using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FindUtils  {
    private static Player player;
    private static GameObject spellBook;
    private static SpellBook spellBookGrid;

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
}
