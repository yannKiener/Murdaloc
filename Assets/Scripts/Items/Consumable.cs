using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumable : Usable
{

    private string consumableName;
    private string description;
    private int stacks;
    private int maxStacks;
    private bool isStackable;
    private Sprite image;
    private Spell spell;

    public Consumable(string name, string description, Spell spell, int maxStacks = 0)
    {
        stacks = 1;
        this.consumableName = name;
        this.spell = spell;
        this.description = description;
        if (maxStacks <= 0)
        {
            this.maxStacks = maxStacks;
            isStackable = true;
        }
        else
        {
            isStackable = false;
        }
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
        if (isStackable && stacks < 1)
        {
            stacks--;
            return true;
        }
        //Remove from inventory
        return false;
    }

    public bool AddOne()
    {
        if (isStackable && stacks < maxStacks)
        {
            stacks++;
            return true;
        }
        //Add another to Inventory
        return false;
    }

    public void Use(Character caster)
    {
        if (spell != null)
        {
            FindUtils.GetPlayer().CastSpell(spell);
        }
    }
}
