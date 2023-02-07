using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : Usable {

    protected string itemName;
    protected string description;
    protected int levelRequirement;
    protected Sprite image;
    protected Stats stats;
    protected string type;
    public bool isEquipped;

    public Item(string itemName, string description, int levelRequirement, Stats stats, string type)
    {
        this.itemName = itemName;
        this.description = description;
        this.levelRequirement = levelRequirement;
        this.stats = stats;
        this.type = type;
        isEquipped = false;
        
        this.image = InterfaceUtils.LoadSpriteForItem(itemName);
        if (image == null)
        {
            Debug.Log("Image null");
        }

    }
    public string GetDescription()
    {
        return description +"\nLevel required : " + levelRequirement+ stats.GetStatsDetail() ;
    }

    public string GetItemType()
    {
        return type;
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
              //  FindUtils.GetInventory().GetComponent<Inventory>().AddItem(this);
            } else
            {
                FindUtils.GetCharacterSheetGrid().EquipItem(this);
                    // FindUtils.GetInventory().GetComponent<Inventory>().RemoveItem(this);
            }
        } else
        {
            MessageUtils.ErrorMessage("Can't use that yet.");
        }
    }
}