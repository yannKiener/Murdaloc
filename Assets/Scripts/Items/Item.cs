using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Item : Usable {

    protected string itemName;
    protected string description;
    protected int levelRequirement;
    protected Sprite image;
    protected Stats stats;
    protected ItemType type;
    public bool isEquipped;
    public bool isInInventory;

    public Item(string itemName, string description, int levelRequirement, Stats stats, ItemType type)
    {
        this.itemName = itemName;
        this.description = description;
        this.levelRequirement = levelRequirement;
        this.stats = stats;
        this.type = type;
        isEquipped = false;
        isInInventory = true;
        
        this.image = InterfaceUtils.LoadSpriteForItem(itemName);
        if (image == null)
        {
            this.image = InterfaceUtils.LoadSpriteForItem("Default");
        }

    }
    public string GetDescription()
    {
        return type + ". " + description +"\nLevel required : " + levelRequirement+ stats.GetStatsDetail() ;
    }

    public void SetImage(Sprite image)
    {
        this.image = image;
    }

    public ItemSlot GetItemSlot()
    {
        return ItemCategories.GetCategory(type).GetSlot();
    }

    public string GetName()
    {
        return itemName;
    }

    public int GetLevelRequirement()
    {
        return levelRequirement;
    }

    public Sprite GetImageAsSprite()
    {
        return image;
    }

    public Stats GetStats()
    {
        return stats;
    }

    public bool IsUsable()
    {
        return (FindUtils.GetPlayer().GetLevel() >= levelRequirement);
    }

    public void Use(Character caster)
    {
        if (IsUsable())
        {
            if (isEquipped)
            {
                FindUtils.GetCharacterSheetGrid().RemoveItem(this);
            } else
            {
                if (isInInventory)
                {
                    FindUtils.GetCharacterSheetGrid().EquipItem(this);
                } else
                {
                    if (FindUtils.GetInventoryGrid().HasAtLeastFreeSlots(1))
                    {
                        FindUtils.GetLootGrid().GetComponent<LootInventory>().RemoveItem(this);
                        FindUtils.GetInventoryGrid().AddItem(this);
                    } else
                    {
                        MessageUtils.ErrorMessage("Not enough space in Inventory.");
                    }
                }
            }
        } else
        {
            MessageUtils.ErrorMessage("Can't use that yet.");
        }
    }
}