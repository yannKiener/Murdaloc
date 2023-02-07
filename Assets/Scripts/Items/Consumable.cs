using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Consumable : Item
{

    private int stacks;
    [System.NonSerialized]
    private Spell spell;
    private string spellName;

    public Consumable(string name, string description, Spell spell)
    {
        this.sellPrice = 5000;
        isInInventory = false;
        stacks = 1;
        this.itemName = name;
        this.spell = spell;
        this.spellName = spell.GetName();
        this.description = description;
        this.image = InterfaceUtils.LoadSpriteForConsumable(itemName);
    }

    public override void LoadImage()
    {
        this.image = InterfaceUtils.LoadSpriteForConsumable(itemName);
    }

    public void LoadSpell()
    {
        this.spell = Spells.Get(spellName);
    }

    public override string GetDescription()
    {
        return "Consumable.\n" + description;
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
            UpdateInventoryStacks();
            return false;
        } else
        {
            FindUtils.GetInventoryGrid().RemoveItem(this);
        }
        return true;
    }

    public bool AddOne()
    {
        stacks++;
        UpdateInventoryStacks();
        return true;
    }

    public void UpdateInventoryStacks()
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
