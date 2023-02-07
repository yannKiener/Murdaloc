using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Useless : Item
{

    private int stacks;

    public Useless(string name, string description)
    {
        this.sellPrice = 1000;
        isInInventory = false;
        stacks = 1;
        this.itemName = name;
        this.description = description;
        this.image = InterfaceUtils.LoadSpriteForUseless(itemName);
    }

    public override void LoadImage()
    {
        this.image = InterfaceUtils.LoadSpriteForUseless(itemName);
    }

    public override string GetDescription()
    {
        return description;
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
        }
        else
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
        if (stacks > 1)
        {
            FindUtils.GetInventoryGrid().GetSlotWith(this.GetName()).GetComponentInChildren<Text>().text = stacks.ToString();
        }
        else
        {
            FindUtils.GetInventoryGrid().GetSlotWith(this.GetName()).GetComponentInChildren<Text>().text = "";
        }
    }

    public override void Use(Character caster)
    {
        if (isInInventory)
        {

        }
        else
        {
            GetFromLootToInventory();
        }
    }
}
