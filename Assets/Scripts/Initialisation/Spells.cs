using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public static class Spells
{
    private static Dictionary<string, Spell> spellList = new Dictionary<string, Spell>();

    public static void Add(Spell spell)
    {
        spellList.Add(spell.GetName(), spell);
    }

    public static Spell Get(string spellName)
    {
        Spell s = spellList[spellName];
        if (s.isHostile())
        {
            return new HostileSpell(s);
        }
        else
        {
            return new FriendlySpell(s);
        }
    }
}
