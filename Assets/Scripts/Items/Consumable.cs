using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Consumable : Item
{

    private int stacks;
    private Spell spell;
    private static float coolDownRemaing = 0;

    public Consumable(string name, string description, Spell spell)
    {
        this.sellPrice = 5000;
        isInInventory = false;
        stacks = 1;
        this.itemName = name;
        this.spell = spell;
        this.description = description;
        this.image = InterfaceUtils.LoadSpriteForConsumable(itemName);
    }

    public override string GetDescription()
    {
        return description + "\n" + getPriceSentence();
    }

    public void SetImage(Sprite image)
    {
        this.image = image;
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
            updateInventoryStacks();
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
        updateInventoryStacks();
        return true;
    }

    private void updateInventoryStacks()
    {
        if(stacks > 1)
        {
            FindUtils.GetInventoryGrid().GetSlotWith(this.GetName()).GetComponentInChildren<Text>().text = stacks.ToString();
        } else
        {
            FindUtils.GetInventoryGrid().GetSlotWith(this.GetName()).GetComponentInChildren<Text>().text = "";
        }
    }

    public override void Use(Character caster)
    {
        if (isInInventory)
        {
            if (spell != null && caster.GCDReady() && !caster.IsCasting())
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
