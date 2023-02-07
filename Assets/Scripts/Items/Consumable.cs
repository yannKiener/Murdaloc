using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumable : Item
{

    private int stacks;
    private Spell spell;
    private static float coolDown = 0;

    public Consumable(string name, string description, Spell spell)
    {
        isInInventory = false;
        stacks = 1;
        this.itemName = name;
        this.spell = spell;
        this.description = description;
        this.image = InterfaceUtils.LoadSpriteForConsumable(itemName);
    }

    public void SetImage(Sprite image)
    {
        this.image = image;
    }

    public override string GetDescription()
    {
        if(stacks > 1)
        {
            return description + "\n" + "(" + stacks + ")";
        }
        return description;
    }

    public int GetStacks()
    {
        return stacks;
    }

    public bool RemoveOne()
    {
        if (stacks > 1)
        {
            stacks--;
            return false;
        } else
        {
            FindUtils.GetInventoryGrid().RemoveItem(this);
        }
        //Remove from inventory
        return true;
    }

    public bool AddOne()
    {
        stacks++;
        return true;
    }

    public override void Use(Character caster)
    {
        if (isInInventory)
        {
            if (spell != null)
            {
                FindUtils.GetPlayer().CastSpell(spell);
                RemoveOne();
            }
        } else
        {
            GetFromLootToInventory();
        }
    }
}
