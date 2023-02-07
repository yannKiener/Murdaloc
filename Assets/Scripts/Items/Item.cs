using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : Usable {

    protected string itemName;
    protected string description = "MissingDesc";
    public bool isInInventory;
    protected Sprite image;
    protected int sellPrice;


    protected string getPriceSentence()
    {
        return "Price : " + InterfaceUtils.GetStringPrice(sellPrice);
    }
    public virtual int GetSellPrice()
    {
        return sellPrice;
    }

    public virtual string GetDescription()
    {
        return description;
    }

    public Sprite GetImageAsSprite()
    {
        return image;
    }

    public string GetName()
    {
        return itemName;
    }

    public void GetFromLootToInventory()
    {

        if (FindUtils.GetInventoryGrid().HasAtLeastFreeSlots(1))
        {
            FindUtils.GetLootGrid().GetComponent<LootInventory>().RemoveItem(this);
            FindUtils.GetInventoryGrid().AddItem(this);
        }
        else
        {
            MessageUtils.ErrorMessage("Not enough space in Inventory.");
        }
    }

    public abstract void Use(Character caster);
}
