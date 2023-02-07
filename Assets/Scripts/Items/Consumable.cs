using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumable : Usable
{

    private string consumableName;
    private string description;
    private int stacks;
    private Sprite image;
    private Spell spell;

    public Consumable(string name, string description, Spell spell)
    {
        stacks = 1;
        this.consumableName = name;
        this.spell = spell;
        this.description = description;
        this.image = InterfaceUtils.LoadSpriteForItem(consumableName);
        if (image == null)
        {
            this.image = InterfaceUtils.LoadSpriteForItem("Default");
        }
    }

    public string GetName()
    {
        return consumableName;
    }

    public string GetDescription()
    {
        return description;
    }

    public void SetImage(Sprite image)
    {
        this.image = image;
    }

    public Sprite GetImageAsSprite()
    {
        return image;
    }

    public int GetStacks()
    {
        return stacks;
    }

    public bool RemoveOne()
    {
        if (stacks < 1)
        {
            stacks--;
            return false;
        }
        //Remove from inventory
        FindUtils.GetInventoryGrid().Remove(GetName());
        return true;
    }

    public bool AddOne()
    {
        stacks++;
        return true;
    }

    public void Use(Character caster)
    {
        if (spell != null)
        {
            FindUtils.GetPlayer().CastSpell(spell);
            RemoveOne();
        }
    }
}
